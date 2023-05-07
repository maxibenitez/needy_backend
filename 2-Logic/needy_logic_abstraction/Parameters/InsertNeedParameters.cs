using needy_dto;

namespace needy_logic_abstraction.Parameters
{
    public class InsertNeedParameters
    {
        public int RequestorId { get; set; }

        public string Status { get; set; }

        public DateTime CreationDate { get; set; }

        public Skill RequestedSkill { get; set; }
    }
}
