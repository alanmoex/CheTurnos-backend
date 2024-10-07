using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(AppDbContext context) : base(context)
        {
        }

        public List<Employee>? GetAllByShopId(int shopId)
        {
            var appDbContext = (AppDbContext)_dbContext;
            return appDbContext.Users.OfType<Employee>().Where(s => s.ShopId == shopId).ToList();
        }

        public List<Appointment>? GetAvailables(int shopId) 
        {
            var appDbContext = (AppDbContext)_dbContext;

            var appointments = appDbContext.Appointments
                .Where(a => a.ProviderId != 0 && a.Status == Status.Active && a.ShopId == shopId)
                .ToList();
            return appointments;
        }
    }
}
