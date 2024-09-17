using System.ComponentModel.DataAnnotations;
using GameStore.Models.Games;

namespace GameStore.Models.Users
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Nickname { get; set; }
        public string Email { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public Cart? Cart { get; set; }
        public ICollection<Review>? Reviews { get; set; }
    }
}
