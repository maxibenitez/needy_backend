using needy_dto;
using needy_logic_abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace needy_logic
{
    public class UserContextLogic : IUserContext
    {
        #region Properties and Fields

        private Session _session;

        #endregion

        #region Implements IUserContext

        public Session GetUserSession()
        {
            return _session;
        }

        public void SetUserSession(Session userSession)
        {
            _session = userSession;
        }

        public void NewSession()
        {
            _session = null;
        }

        #endregion
    }
}
