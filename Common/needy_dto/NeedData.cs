namespace needy_dto
{
    public class NeedData
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string RequestorCI { get; set; }

        public string? AcceptedApplierCI { get; set; }

        public string Status { get; set; }

        public string? Description { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime NeedDate { get; set; }

        public DateTime? AcceptedDate { get; set; }

        public int RequestedSkillId { get; set; }

        public string NeedAddress { get; set; }

        public string Modality { get; set; }
    }
}
