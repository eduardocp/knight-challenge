namespace Knight.Domain.Models.Dto.Results
{
    public class KnightResult : Entities.Knight
    {
        public new string Id { get; set; }

        public int Age { get; set; }

        public int Experience { get; set; }

        public int Attack { get; set; }
    }
}