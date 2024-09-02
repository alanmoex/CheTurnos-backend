using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Shops
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
       public int Id { get; set; }
        [Required]
        public User Owner { get; set; }
        [Required]
        public int IdOwner { get; set; }
        public ICollection<ShopsReviews> Reviews { get; set; } = null;
        public ICollection<Appointments> Appointments { get; set; } = null;

        public StatusType State {  get; set; } = StatusType.Active;
    }
}
