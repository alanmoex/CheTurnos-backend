using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Requests
{
    public class AssignClientRequestDTO
    {
        [Required]
        public int IdAppointment { get; set; }

        [Required]
        public int ServiceId { get; set; }

        [Required]
        public int ClientId { get; set; } 
    }
}
