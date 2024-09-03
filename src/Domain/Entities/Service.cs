using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public float Price { get; set; }

        [Required]
        public ICollection<User> Staff { get; set; }

        [Required]
        public StatusType Status { get; set; } = StatusType.Active;
        [Required]
        public ServiceType ServiceType { get; set; }

        [Required]
        public Shop shop { get; set; }
        [Required]
        [ForeignKey (nameof(ShopId))]
        public int ShopId { get; set; }


    }
}
