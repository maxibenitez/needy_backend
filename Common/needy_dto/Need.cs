namespace needy_dto
{
    public class Need
    {
        public int Id { get; set; }

        public User Requestor { get; set; }

        public IEnumerable<User> Appliers { get; set; }

        public User Applier { get; set; }

        public string Status { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime AcceptedDate { get; set; }

        public Skill RequestedSkill { get; set; }
    }
}
