using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using GameStore.Models.Games;

namespace GameStore.Models.Users
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Nickname { get; set; }
        public string Email { get; set; }
        [JsonIgnore]
        public byte[] PasswordHash { get; set; }
        [JsonIgnore]
        public byte[] PasswordSalt { get; set; }
        public Cart? Cart { get; set; } = new Cart();
        public Library? Library { get; set; } = new Library();
        public ICollection<Review>? Reviews { get; set; }
    }
}
