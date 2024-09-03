using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enums;

namespace Domain.Entities
{
    public class ShopsReview
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
        [ForeignKey(nameof(ClientId))]
        public int ClientId { get; set;}
        public User Client { get; set; }

        [Required]
        [ForeignKey(nameof(ShopId))]
        public int ShopId { get; set; }
        public Shop Shop { get; set; }

        [Required]
        public StatusType Status { get; set; } = StatusType.Active;
    }
}
