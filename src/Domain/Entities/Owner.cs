using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Owner : User
    {
        public int ShopId { get; set; } //FK
        public ICollection<Schedule>? WorkSchedules { get; set; }

        public ICollection<Appointment>? Appointments { get; set; }
        public ICollection<Service>? Services { get; set; }
    }
}
