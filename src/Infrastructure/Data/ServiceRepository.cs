using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

namespace Infrastructure.Data;

public class ServiceRepository : RepositoryBase<Service>, IServiceRepository
{

    public ServiceRepository(AppDbContext context) : base(context)
    {
    }


    public List<Service>? GetAllByShopId(int shopId)
    {
        var appDbContext = (AppDbContext)_dbContext;
        return appDbContext.Services.Where(s => s.ShopId == shopId).ToList();
    }


}