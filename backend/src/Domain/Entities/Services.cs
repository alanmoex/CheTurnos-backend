using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Services
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
        public User ProvidedBy { get; set; }
        [Required]
        [ForeignKey(nameof(IdProvidedBy))]
        public int IdProvidedBy { get; set; }

        [Required]
        public StatusType Status { get; set; } = StatusType.Active;
    }
}
