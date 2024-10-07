using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class AppointmentRepository : RepositoryBase<Appointment>, IAppointmentRepository
    {

        public AppointmentRepository(AppDbContext context): base(context) 
        {
        }


        public List<Appointment> GetAvailableAppointmentsByEmployeeId(int employeeId)
        {
            var appDbContext = (AppDbContext)_dbContext;
            return appDbContext.Appointments.Where(a => a.ProviderId == employeeId && a.Status == Status.Active).ToList();
        }

    }
}
