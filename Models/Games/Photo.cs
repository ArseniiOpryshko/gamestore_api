using System.ComponentModel.DataAnnotations;

namespace GameStore.Models.Games
{
    public class Photo
    {
        [Key]
        public int Id { get; set; }
        public byte[] Image { get; set; }

        public int? GameId { get; set; }
        public Game Game { get; set; }
    }
}
