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
        public User Owner { get; set; }

        public ICollection<Appointment> Appointments { get; set; } = null;

        public ShopTypes Type { get; set; }
        public StatusType State {  get; set; } = StatusType.Active;

        public ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
    }
}
