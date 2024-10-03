using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Models
{
    public class AppointmentDTO
    {
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
            public TimeSpan Duration { get; set; }


        public static AppointmentDTO Create(Appointment Appointment)
        {
            var dto = new AppointmentDTO();
            dto.Id = Appointment.Id;
            dto.Status = Appointment.Status;
            dto.ServiceId = Appointment.ServiceId;
            dto.ProviderId = Appointment.ProviderId;
            dto.ClientId = Appointment.ClientId;
            dto.ShopId = Appointment.ShopId;
            dto.DateAndHour = Appointment.DateAndHour;
            dto.Duration = Appointment.Duration;

            return dto;
        }

        public static List<AppointmentDTO?> CreateList(IEnumerable<Appointment> Appointment)
        {
            List<AppointmentDTO?> listDto = [];

            foreach (var a in Appointment)
            {
                listDto.Add(Create(a));
            }

            return listDto;
        }

    }
}
