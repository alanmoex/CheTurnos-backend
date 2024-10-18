using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Org.BouncyCastle.Crypto;
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
        }public List<Appointment> GetAvailableAppointmentsByClientId(int ClientId)
        {
            var appDbContext = (AppDbContext)_dbContext;
            return appDbContext.Appointments.Where(a => a.ClientId == ClientId && a.Status == Status.Active).ToList();
        }

        public Appointment? GetLastAppointmentByShopId(int shopId)
        {
            return _dbContext.Set<Appointment>()
                .Where(a => a.ShopId == shopId)
                .OrderByDescending(a => a.Id)
                .FirstOrDefault();
        }

        public List<Appointment> GetAllAppointmentsByShopId(int shopId)
        {
            return _dbContext.Set<Appointment>()
                .Where(a => a.ShopId == shopId)
                .ToList();
        }
    }
}
