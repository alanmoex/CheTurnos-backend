using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Requests
{
    public class AppointmentUpdateRequest
    {
        public Status Status { get; set; } = Status.Active;
        public int? ServiceId { get; set; } //FK
        public int ProviderId { get; set; } //FK
        public int? ClientId { get; set; } //FK
        public DateTime DateAndHour { get; set; }
        public TimeSpan Duration { get; set; }

    }
}
