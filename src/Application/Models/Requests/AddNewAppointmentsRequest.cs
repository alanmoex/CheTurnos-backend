using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Requests
{
    public class AddNewAppointmentsRequest
    {
        [Required]
        public string DateStart { get; set; }
        [Required]
        public string DateEnd { get; set; }
    }
}
