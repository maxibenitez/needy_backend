namespace needy_dto
{
    public class Rating
    {
        public string GiverCI { get; set; }

        public string ReceiverCI { get; set; }

        public int NeedId { get; set; }

        public double Stars { get; set; }

        public string Comment { get; set; }
    }
}
