using System.ComponentModel.DataAnnotations;

namespace GameStore.Models.Games
{
    public class Tag
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Game> Games { get; set; }
    }
}
