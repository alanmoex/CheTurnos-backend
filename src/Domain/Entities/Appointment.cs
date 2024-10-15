using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Domain.Entities
{
    public class Appointment
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

        public Appointment(int providerId, int shopId, string dateOnly, string timeOnly) 
        {
            // se obtendran del front, del dueño/empleado que este iniciado sesion (JWT)                                                       
            ProviderId = providerId;  
            ShopId = shopId;
            // se inician en null y se llenan cuando el cliente reserva
            ServiceId = null;
            ClientId = null;
            // dato que viene del front o se autogenera
            DateAndHour = new DateTime(DateOnly.Parse(dateOnly), TimeOnly.Parse(timeOnly));
            // duration --> en RepositoryAppoiment en la funcion CreateAppoiment (buscar en la tabla Shops 
            // por el id del shop del turno y asignar a la prop Duration la duracion de turnos del negocio (AppoimentFrecuence)
        }
    }
}
