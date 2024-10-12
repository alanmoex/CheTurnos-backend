using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Requests
{
    public class AppointmentCreateRequest
    {
        public int? ServiceId { get; set; } //FK
        public int ProviderId { get; set; } //FK
        public int? ClientId { get; set; } //FK

        [Required]
        public int ShopId { get; set; } //FK

        [Required]
        public DateTime DateAndHour { get; set; }

        //[Required]
        //public TimeSpan? Duration { get; set; }
    }
}
