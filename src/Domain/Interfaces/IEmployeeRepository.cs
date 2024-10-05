using Domain.Entities;
using Domain.Interface;

namespace Infrastructure.Data
{
    public interface IEmployeeRepository : IRepositoryBase<Employee>
    {
        List<Employee>? GetAllByShopId(int shopId);
    }
}