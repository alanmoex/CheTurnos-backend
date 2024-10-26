using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Models.Requests
{
    public class AllApointmentsOfMyShopRequestDTO
    {
        public int Id { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))] //Para que se vea el string del enum y no un numero.
        public Status Status { get; set; } = Status.Active;
        public string ServiceName { get; set; } = string.Empty;
        public string ProviderId { get; set; } = string.Empty;
        public string ClientName { get; set; } = string.Empty;
        public DateTime DateAndHour { get; set; }

        public TimeSpan Duration { get; set; }


        public static AllApointmentsOfMyShopRequestDTO Create(Appointment Appointment, string user, string client, string service)
        {
            var dto = new AllApointmentsOfMyShopRequestDTO();
            dto.Id = Appointment.Id;
            dto.Status = Appointment.Status;
            dto.ServiceName = service;
            dto.ProviderId = user;
            dto.ClientName = client;
            dto.DateAndHour = Appointment.DateAndHour;
            dto.Duration = Appointment.Duration;

            return dto;
        }

    }
}
