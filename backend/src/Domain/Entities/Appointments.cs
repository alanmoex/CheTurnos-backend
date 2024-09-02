using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Appointments
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public StatusType Status { get; set; } = StatusType.Active;

        [Required]
        [ForeignKey(nameof(IdService))]
        public int IdService { get; set; }
        public Services Service { get; set; }

        [Required]
        [ForeignKey(nameof(IdStaff))]
        public int IdStaff { get; set; }
        public User User { get; set; }

        [Required]
        [ForeignKey(nameof(IdShop))]
        public int IdShop { get; set; }
        public Shops Shop { get; set; }

        [Required]
        [ForeignKey(nameof(IdClient))]
        public int IdClient { get; set; }
        public User Client {  get; set; }

        [Required]
        public DateTime AppointmentDate { get; set; }

        [Required]
        public TimeSpan AppointmentTime { get; set; }

    }
}
