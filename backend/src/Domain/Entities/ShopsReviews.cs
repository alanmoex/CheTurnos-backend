using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ShopsReviews
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required, MaxLength (90)]
        public string Coment { get; set; }
        [Required]
        [Range( 0, 5, ErrorMessage = "Value must be between 0 and 5")]
        public int Stars { get; set; }

        [Required]
        [ForeignKey(nameof(IdClient))]
        public int IdClient { get; set;}
        public User Client { get; set; }

        [Required]
        [ForeignKey(nameof(IdShop))]
        public int IdShop { get; set; }
        public Shops Shop { get; set; }

        [Required]
        public StatusType Status { get; set; } = StatusType.Active;
    }
}
