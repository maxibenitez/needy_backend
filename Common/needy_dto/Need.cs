namespace needy_dto
{
    public class Need
    {
        public int Id { get; set; }

        public User Requestor { get; set; }

        public IEnumerable<User> Appliers { get; set; }

        public User? AcceptedApplier { get; set; }

        public string Status { get; set; }

        public string? Description { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime? NeedDate { get; set; }

        public DateTime? AcceptedDate { get; set; }

        public Skill RequestedSkill { get; set; }
    }
}
