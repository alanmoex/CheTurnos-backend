using Application.Models;
using Application.Models.Requests;
using Domain.Entities;
using System.Collections.Generic;

namespace Application
{
    public interface IShopService
    {
        List<ShopDTO> GetAll();
        ShopDTO GetById(int id);
        ShopDTO Create(ShopCreateRequest shopCreateRequest);
        void Update(int id, ShopUpdateRequest shopUpdateRequest);
        void Delete(int id);
    }
}
