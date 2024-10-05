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
    }
}
