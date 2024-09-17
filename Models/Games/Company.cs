using System.ComponentModel.DataAnnotations;

namespace GameStore.Models.Games
{
    public class Company
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string About { get; set; }
        public byte[] Photo { get; set; }
        public ICollection<Game> Games { get; set; }

    }
}
