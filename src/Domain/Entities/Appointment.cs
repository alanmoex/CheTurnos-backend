using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Domain.Enums;
using Domain.Interfaces;

namespace Domain.Entities
{
    public class Appointment : IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public Status Status { get; set; } = Status.Active;

        public int? ServiceId { get; set; } //FK
        public int ProviderId { get; set; } //FK
        public int? ClientId { get; set; } //FK
        public int ShopId { get; set; } //FK

        [Required]
        public DateTime DateAndHour { get; set; }

        [Required]
        public TimeSpan Duration { get; set; } = TimeSpan.Zero;

        public Appointment() { }

    }
}
