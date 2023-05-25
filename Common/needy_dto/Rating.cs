namespace needy_dto
{
    public class Rating
    {
        public User Giver { get; set; }

        public double Stars { get; set; }

        public string? Comment { get; set; }
    }
}
