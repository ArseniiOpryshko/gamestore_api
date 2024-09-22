using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using GameStore.Models.Games;
using GameStore.Models.Users;

namespace GameStore.Models.Orders
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        public int? CartId { get; set; }
        public int? GameId { get; set; }
        public DateTime OrderDate { get; set; }
        [JsonIgnore]
        public Cart Cart { get; set; }
        public Game Game { get; set; }
    }
}
