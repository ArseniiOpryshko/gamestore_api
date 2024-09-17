using System.ComponentModel.DataAnnotations;
using GameStore.Models.Users;

namespace GameStore.Models.Games
{
    public class Review
    {
        [Key]
        public int Id { get; set; }
        public int? GameId { get; set; }
        public int? UserId { get; set; }
        public string Content { get; set; }
        public bool IsRecommended { get; set; }
        public Game Game { get; set; }
        public User User { get; set; }
    }
}
