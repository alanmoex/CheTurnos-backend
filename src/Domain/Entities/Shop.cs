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
        public string Name { get; set; }

        public ShopType Type { get; set; }
        public Status Status {  get; set; } = Status.Active;

        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
        public ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
        public ICollection<User> Employees { get; set; } = new List<User>();

        public Shop() { }

        public Shop(string name, ShopType type)
        {
            Name = name;
            Type = type;
        }
    }
}
