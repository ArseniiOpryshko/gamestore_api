using Azure;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameStore.Models.Games
{
    public class Game
    {
        [Key]
        public int Id { get; set; }
        [Column(TypeName = "decimal(5, 2)")]
        public decimal Price { get; set; }
        public int? CompanyId { get; set; }
        public int? MainPhotoId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime ReleaseDate { get; set; }
        public ICollection<Photo>? Photos { get; set; }
        public ReviewStatus ReviewStatus { get; set; }
        public Company Company { get; set; }
        public ICollection<Tag> Tags { get; set; }
        public ICollection<Review> Reviews { get; set; }
    }
    public enum ReviewStatus
    {
        Neutral = 0,
        VeryPositive = 1,
        Positive = 2,
        Negative = 3,
        VeryNegative = 4,
    }
}
