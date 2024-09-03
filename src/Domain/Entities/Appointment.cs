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
    public class Appointment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public StatusType Status { get; set; } = StatusType.Active;

        [Required]
        [ForeignKey(nameof(ServiceId))]
        public int ServiceId { get; set; }
        public Service Service { get; set; }

        [Required]
        [ForeignKey(nameof(ProvidedById))]
        public int ProvidedById { get; set; }
        public User ProvidedBy { get; set; }

        [Required]
        [ForeignKey(nameof(ShopId))]
        public int ShopId { get; set; }
        public Shop Shop { get; set; }

        [Required]
        [ForeignKey(nameof(ClientId))]
        public int ClientId { get; set; }
        public User Client {  get; set; }

        [Required]
        public DateTime AppointmentDate { get; set; }

        [Required]
        public TimeSpan AppointmentTime { get; set; }

    }
}
