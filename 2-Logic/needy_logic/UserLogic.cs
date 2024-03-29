﻿using needy_dataAccess.Interfaces;
using needy_dto;
using needy_logic_abstraction;
using needy_logic_abstraction.Parameters;

namespace needy_logic
{
    public class UserLogic : IUserLogic
    {
        #region Properties and Fields

        private readonly IUserRepository _userRepository;
        private readonly ISkillRepository _skillRepository;
        private readonly IRatingRepository _ratingRepository;
        private readonly ITokenLogic _tokenLogic;

        #endregion

        #region Builders

        public UserLogic(IUserRepository userRepository,
            ISkillRepository skillRepository,
            IRatingRepository ratingRepository,
            ITokenLogic tokenLogic)
        {
            _userRepository = userRepository;
            _skillRepository = skillRepository;
            _ratingRepository = ratingRepository;
            _tokenLogic = tokenLogic;
        }

        #endregion

        #region Implements IUserLogic

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            string userCI = await _tokenLogic.GetUserCIFromToken();

            List<UserData> data = (await _userRepository.GetUsersAsync(userCI)).ToList();
            List<User> users = new List<User>();

            foreach (UserData user in data)
            {
                users.Add(await UserBuilderAsync(user));
            }

            return users;
        }

        public async Task<IEnumerable<User>> GetUsersBySkillAsync(int skillId)
        {
            List<UserData> data = (await _userRepository.GetUsersBySkillAsync(skillId)).ToList();
            List<User> users = new List<User>();

            foreach (UserData user in data)
            {
                users.Add(await UserBuilderAsync(user));
            }

            return users;
        }

        public async Task<User> GetUserByCIAsync(string userCI)
        {
            UserData data = await _userRepository.GetUserByCIAsync(userCI);

            return await UserBuilderAsync(data);
        }

        public async Task<IEnumerable<User>> GetUsersBySkillNameAsync(string skillName)
        {
            List<UserData> data = (await _userRepository.GetUsersBySkillNameAsync(skillName)).ToList();
            List<User> users = new List<User>();

            foreach (UserData user in data)
            {
                users.Add(await UserBuilderAsync(user));
            }

            return users;
        }

        public async Task<bool> UpdateUserAsync(UpdateUserParameters parameters)
        {
            string userCI = await _tokenLogic.GetUserCIFromToken();

            if (await _userRepository.UpdateUserAsync(userCI, parameters))
            {
                if (await _userRepository.DeleteUserSkillsAsync(userCI))
                {
                    if (parameters.SkillsId.Count() > 0)
                    {
                        foreach (int skillId in parameters.SkillsId)
                        {
                            await _userRepository.InsertUserSkillAsync(userCI, skillId);
                        }
                    }

                    return true;
                }

                return false;
            }

            return false;
        }

        #endregion

        #region Private Methods

        private async Task<User> UserBuilderAsync(UserData data)
        {
            var user = new User
            {
                CI = data.CI,
                FirstName = data.FirstName,
                LastName = data.LastName,
                Address = data.Address,
                Zone = data.Zone,
                Phone = data.Phone,
                Age = await GetUserAgeAsync(data.BirthDate),
                AboutMe = data.AboutMe,
                Email = data.Email,
            };

            user.Skills = await _skillRepository.GetUserSkillsAsync(data.CI);
            user.AvgRating = await GetRatingAverageAsync(data.CI);

            return user;
        }

        private async Task<int> GetUserAgeAsync(DateTime birthDate)
        {
            var today = DateTime.Today;
            var age = today.Year - birthDate.Year;

            if (birthDate.Date > today.AddYears(-age))
            {
                age--;
            }

            return age;
        }

        private async Task<double> GetRatingAverageAsync(string userCI)
        {
            List<Rating> ratings = (await _ratingRepository.GetUserRatingsAsync(userCI)).ToList();
            double total = 0;

            if (ratings.Count > 0)
            {
                foreach (Rating rating in ratings)
                {
                    total += rating.Stars;
                }

                return total / ratings.Count();
            }

            return total;
        }

        #endregion
    }
}
