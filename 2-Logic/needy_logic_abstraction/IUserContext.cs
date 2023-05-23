using needy_dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace needy_logic_abstraction
{
    public interface IUserContext
    {
        Session GetUserSession();

        void SetUserSession(Session userSession);

        void NewSession();
    }
}
