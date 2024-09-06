using Domain.Entities;
using Domain.Interfaces;

namespace Infrastructure.Data;

public class ServiceRepository : RepositoryBase<Service>, IServiceRepository
{
    public ServiceRepository(AppDbContext context) : base(context)
    {
    }
}