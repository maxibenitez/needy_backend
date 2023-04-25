using needy_logic_abstraction.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace needy_dataAccess.Interfaces
{
    public interface IRaitingRepository
    {
        Task<bool> InsertRaitingAsync(InsertRaitingParameters parameters);
    }
}
