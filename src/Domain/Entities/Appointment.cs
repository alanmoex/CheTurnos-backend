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
        public User Employee { get; set; }
        public User Client { get; set; }
        public Shop Shop { get; set; }

        [Required]
        public DateTime DateAndHour { get; set; }

        [Required]
        public TimeSpan Duration { get; set; }

        public Appointment() { }

        public Appointment(Service service, User employee, User client, Shop shop, DateTime dateAndHour) 
        {
            Service = service;
            Employee = employee;
            Client = client;
            Shop = shop;
            DateAndHour = dateAndHour;
        }

        
    }
}
