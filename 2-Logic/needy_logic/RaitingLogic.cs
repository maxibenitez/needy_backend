using needy_dataAccess.Interfaces;
using needy_dataAccess.Repositories;
using needy_logic_abstraction;
using needy_logic_abstraction.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace needy_logic
{
    public class RaitingLogic : IRaitingLogic
    {
        #region Properties and Fields

        private readonly IRaitingRepository _raitingRepository;
        private readonly IUserRepository _userRepository;

        #endregion

        #region Builders

        public RaitingLogic(IRaitingRepository raitingRepository, IUserRepository userRepository)
        {
            _raitingRepository = raitingRepository;
            _userRepository = userRepository;
        }

        #endregion

        #region Implements IRaitingLogic

        public async Task<bool> InsertRaitingAsync(InsertRaitingParameters parameters)
        {
            //Controlar el user
            return await _raitingRepository.InsertRaitingAsync(parameters);
        }

        #endregion
    }
}
