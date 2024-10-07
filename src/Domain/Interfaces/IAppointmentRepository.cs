using Domain.Entities;
using Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IAppointmentRepository : IRepositoryBase<Appointment>
    {
        List<Appointment> GetAvailableAppointmentsByEmployeeId(int employeeId);
    }
}
