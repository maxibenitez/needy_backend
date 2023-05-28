﻿using needy_dataAccess.Interfaces;
using needy_dto;
using needy_logic_abstraction;
using needy_logic_abstraction.Parameters;
using needy_logic_abstraction.Enumerables;

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

        public async Task<string> LoginAsync(LoginParameters parameters)
        {
            UserData user = await _userRepository.GetUserByEmailAsync(parameters.Email);
            string token = "";

            if (user != null)
            {
                var result = BCrypt.Net.BCrypt.Verify(parameters.Password, user.Password);

                if (result)
                {
                    return await _tokenLogic.GenerateJwtToken(user);
                }

                return token;
            }

            return token;
        }

        public async Task<RegisterStatus> RegisterAsync(RegisterParameters parameters)
        {
            if (await CheckCIExists(parameters.CI))
            {
                return RegisterStatus.UserAlreadyExist;
            }

            if (await CheckEmailExists(parameters.Email))
            {
                return RegisterStatus.EmailAlreadyExist;
            }

            var hashpwd = BCrypt.Net.BCrypt.HashPassword(parameters.Password);
            parameters.Password = hashpwd;

            var result = await _userRepository.InsertUserAsync(parameters);

            if (result)
            {
                return RegisterStatus.Success;
            }

            return RegisterStatus.InternalServerError;
        }

        #endregion

        #region Private Methods

        private async Task<bool> CheckCIExists(string userCI)
        {
            UserData user = await _userRepository.GetUserByCIAsync(userCI);

            return user is not null ? true : false;
        }

        private async Task<bool> CheckEmailExists(string email)
        {
            UserData user = await _userRepository.GetUserByEmailAsync(email);

            return user is not null ? true : false;
        }

        #endregion
    }
}
