namespace needy_dto
{
    public class User
    {
        public string CI { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Address { get; set; }

        public string Zone { get; set; }

        public string Phone { get; set; }

        public int Age { get; set; }

        public Skill? Skill { get; set; }

        public double? AvgRating { get; set; }
    }
}