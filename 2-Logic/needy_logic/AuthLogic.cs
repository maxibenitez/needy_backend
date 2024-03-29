﻿using needy_dataAccess.Interfaces;
using needy_dto;
using needy_logic_abstraction;
using needy_logic_abstraction.Parameters;
using needy_logic_abstraction.Enumerables;
using System.Text.Json;
using Microsoft.IdentityModel.Tokens;

namespace needy_logic
{
    public class AuthLogic : IAuthLogic
    {
        #region Properties and Fields

        private readonly IUserRepository _userRepository;
        private readonly ITokenLogic _tokenLogic;

        #endregion

        #region Builders

        public AuthLogic(IUserRepository userRepository, ITokenLogic tokenLogic)
        {
            _userRepository = userRepository;
            _tokenLogic = tokenLogic;
        }

        #endregion

        #region Implements IAuthorizationLogic

        public async Task<Session> LoginAsync(LoginParameters parameters)
        {
            UserData user = await _userRepository.GetUserByEmailAsync(parameters.Email);

            if (user is not null)
            {
                if (BCrypt.Net.BCrypt.Verify(parameters.Password, user.Password))
                {
                    string token = await _tokenLogic.GenerateJwtToken(user);

                    if (token.IsNullOrEmpty())
                    {
                        return null;
                    }

                    return SetSession(token, user.CI);
                }

                return null;
            }

            return null;
        }

        public async Task<ErrorStatus> RegisterAsync(RegisterParameters parameters)
        {
            if (await IsDuplicateCI(parameters.CI))
            {
                return ErrorStatus.UserAlreadyExist;
            }

            if (await IsDuplicateEmail(parameters.Email))
            {
                return ErrorStatus.EmailAlreadyExist;
            }

            var hashpwd = BCrypt.Net.BCrypt.HashPassword(parameters.Password);
            parameters.Password = hashpwd;

            if (await _userRepository.InsertUserAsync(parameters))
            {
                if(parameters.SkillsId.Count() > 0)
                {
                    foreach (int skillId in parameters.SkillsId)
                    {
                        await _userRepository.InsertUserSkillAsync(parameters.CI, skillId);
                    }
                }

                return ErrorStatus.Success;
            }

            return ErrorStatus.InternalServerError;
        }

        #endregion

        #region Private Methods

        private async Task<bool> IsDuplicateCI(string userCI)
        {
            UserData user = await _userRepository.GetUserByCIAsync(userCI);

            return user is not null ? true : false;
        }

        private async Task<bool> IsDuplicateEmail(string email)
        {
            UserData user = await _userRepository.GetUserByEmailAsync(email);

            return user is not null ? true : false;
        }

        private Session SetSession(string token, string userCI)
        {
            var userSession = new Session { 
                Token = JsonSerializer.Serialize(token),
                ExpiresIn = DateTime.UtcNow.AddSeconds(7200),
                userCI = userCI,
            };

            return userSession;
        }

        #endregion
    }
}
