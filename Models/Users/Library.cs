using GameStore.Models.Games;
using System.ComponentModel.DataAnnotations;

namespace GameStore.Models.Users
{
    public class Library
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public ICollection<Game> Games { get; set; }
    }
}
