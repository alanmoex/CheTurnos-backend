using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Requests
{
    public class EmployeeUpdateRequest
    {
        public string Name { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmationPassword { get; set; }
    }
}
