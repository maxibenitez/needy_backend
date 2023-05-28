﻿using needy_dataAccess.Interfaces;
using needy_dto;
using needy_logic_abstraction;
using needy_logic_abstraction.Parameters;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using needy_logic_abstraction.Enumerables;

namespace needy_logic
{
    public class AuthLogic : IAuthLogic
    {
        #region Properties and Fields

        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        #endregion

        #region Builders

        public AuthLogic(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
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
                    return GenerateJwtToken(user);
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

        private string GenerateJwtToken(UserData user)
        {
            var claims = new List<Claim>
            {
                new Claim("CI", user.CI),
                new Claim(ClaimTypes.Name, user.FirstName),
                new Claim(ClaimTypes.Surname, user.LastName),
                new Claim(ClaimTypes.Email, user.Email),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Token"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                _configuration["JWT:Issuer"],
                _configuration["JWT:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(60),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

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
