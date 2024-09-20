using GameStore.Models.Games;

namespace GameStore.Dtos.GameDtos
{
    public class GameCreateDto
    {
        public int? CompanyId { get; set; }
        public decimal Price { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<string> Tags { get; set; }
    }
}
