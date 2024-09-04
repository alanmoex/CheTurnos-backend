using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Enums;

namespace Domain.Entities
{
    public class Service
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "must be a positive value.")]
        public decimal Price { get; set; }

        [Required]
        public ICollection<User> Staff { get; set; }

        [Required]
        public StatusType Status { get; set; } = StatusType.Active;

        [Required]
        public ServiceType ServiceType { get; set; }

        public Shop Shop { get; set; }    

    }
}
