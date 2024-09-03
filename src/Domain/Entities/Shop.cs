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
    public class Shop
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
       public int Id { get; set; }
        [Required]
        public User Owner { get; set; }
        [Required]
        [ForeignKey(nameof(OwnerId))]
        public int OwnerId { get; set; }

        public ICollection<ShopsReview> Reviews { get; set; } = null;

        public ICollection<Appointment> Appointments { get; set; } = null;

        public ShopTypes Type { get; set; }
        public StatusType State {  get; set; } = StatusType.Active;
    }
}
