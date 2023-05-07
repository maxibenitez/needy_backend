namespace needy_dto
{
    public class Rating
    {
        public int Id { get; set; }

        public User User { get; set; }

        public decimal Average { get; set; }

        public string Comment { get; set; }
    }
}
