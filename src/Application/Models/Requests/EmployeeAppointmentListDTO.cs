using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Models.Requests
{
    public class EmployeeAppointmentListDTO
    {

        public int Id { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))] //Para que se vea el string del enum y no un numero.
        public Status Status { get; set; } = Status.Active;
        public int? ServiceId { get; set; } //FK
        public int ProviderId { get; set; } //FK
        public int? ClientId { get; set; } //FK
        public string ServiceName { get; set; } = string.Empty;
        public string ShopName { get; set; } = string.Empty;
        public string ClientName { get; set; } = string.Empty;
        public int ShopId { get; set; } //FK

        public DateTime DateAndHour { get; set; }

        public TimeSpan Duration { get; set; }


        public static EmployeeAppointmentListDTO Create(Appointment Appointment, string serviceName, string shopName, string clientName)
        {
            var dto = new EmployeeAppointmentListDTO();
            dto.Id = Appointment.Id;
            dto.Status = Appointment.Status;
            dto.ServiceId = Appointment.ServiceId;
            dto.ProviderId = Appointment.ProviderId;
            dto.ClientId = Appointment.ClientId;
            dto.ShopId = Appointment.ShopId;
            dto.DateAndHour = Appointment.DateAndHour;
            dto.ServiceName = serviceName;
            dto.ShopName = shopName;
            dto.ClientName = clientName;
            
            //dto.Duration = Appointment.Duration;

            return dto;
        }
    }
}
