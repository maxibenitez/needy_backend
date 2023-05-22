using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace needy_dto
{
    public class NeedData
    {
        public int Id { get; set; }

        public string RequestorCI { get; set; }

        public IEnumerable<string> AppliersCI { get; set; }

        public string? AcceptedApplierCI { get; set; }

        public string Status { get; set; }

        public string? Description { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime? NeedDate { get; set; }

        public DateTime? AcceptedDate { get; set; }

        public int RequestedSkillId { get; set; }
    }
}
