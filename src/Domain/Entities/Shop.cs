using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Enums;

namespace Domain.Entities
{
    public class Shop
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public ShopType Type { get; set; }

        [Required]
        public Status Status { get; set; } = Status.Active;

        [Required]
        public string Address { get; set; }

        [Required]
        public string Phone { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
        public ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
        public ICollection<User> Employees { get; set; } = new List<User>();

        public Shop() { }

        public Shop(string name, ShopType type, string address = null, string phone = null, string email = null)
        {
            Name = name;
            Type = type;
            Address = address;
            Phone = phone;
            Email = email;
        }
    }
}
