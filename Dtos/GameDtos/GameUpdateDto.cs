namespace GameStore.Dtos.GameDtos
{
    public class GameUpdateDto
    {
        public int Id { get; set; }
        public int? CompanyId { get; set; }
        public decimal Price { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<string> Tags { get; set; }
    }
}
