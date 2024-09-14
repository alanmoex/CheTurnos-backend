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

        public Service Service { get; set; }
        public int EmployeeId { get; set; }
        public int ClientId { get; set; }
        public int ShopId { get; set; }
         
        [Required]
        public DateTime DateAndHour { get; set; }

        [Required]
        public TimeSpan Duration { get; set; }

        public Appointment() { }

        public Appointment(Service service, int employee, int client, int shop, DateTime dateAndHour) 
        {
            Service = service;                                                          
            EmployeeId = employee;
            ClientId = client;
            ShopId = shop;
            DateAndHour = dateAndHour;
        }

        
    }
}
