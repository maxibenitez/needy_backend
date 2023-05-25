namespace needy_dto
{
    public class RatingData
    {
        public int Id { get; set; }

        public string GiverCI { get; set; }

        public string ReceiverCI { get; set; }

        public int NeedId { get; set; }

        public double Stars { get; set; }

        public string? Comment { get; set; }
    }
}
