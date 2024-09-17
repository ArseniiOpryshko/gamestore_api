using System.ComponentModel.DataAnnotations;
using GameStore.Models.Orders;

namespace GameStore.Models.Users
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }
        public int? UserId { get; set; }
        public User User { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}
