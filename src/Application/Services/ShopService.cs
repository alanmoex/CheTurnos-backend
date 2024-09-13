using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using Application.Models.Requests;
using Application.Models;

namespace Application
{
    public class ShopService : IShopService
    {
        private readonly IShopRepository _shopRepository;

        public ShopService(IShopRepository shopRepository)
        {
            _shopRepository = shopRepository;
        }

        public List<ShopDTO> GetAll()
        {
            return _shopRepository.GetAll().Select(shop => new ShopDTO
            {
                Id = shop.Id,
                Name = shop.Name,
                Type = shop.Type,
                Status = shop.Status,
                Address = shop.Address, 
                Phone = shop.Phone,     
                Email = shop.Email      
            }).ToList();
        }

        public ShopDTO GetById(int id)
        {
            var shop = _shopRepository.GetById(id);
            if (shop == null)
                throw new NotFoundException("Shop", id);

            return new ShopDTO
            {
                Id = shop.Id,
                Name = shop.Name,
                Type = shop.Type,
                Status = shop.Status,
                Address = shop.Address, 
                Phone = shop.Phone,    
                Email = shop.Email      
            };
        }

        public ShopDTO Create(ShopCreateRequest shopCreateRequest)
        {
            var shop = new Shop
            {
                Name = shopCreateRequest.Name,
                Type = shopCreateRequest.Type,
                Address = shopCreateRequest.Address, 
                Phone = shopCreateRequest.Phone,     
                Email = shopCreateRequest.Email      
            };

            _shopRepository.Add(shop);

            return new ShopDTO
            {
                Id = shop.Id,
                Name = shop.Name,
                Type = shop.Type,
                Status = shop.Status,
                Address = shop.Address, 
                Phone = shop.Phone,     
                Email = shop.Email      
            };
        }

        public void Update(int id, ShopUpdateRequest shopUpdateRequest)
        {
            var shop = _shopRepository.GetById(id);
            if (shop == null)
                throw new NotFoundException("Shop", id);

            shop.Name = shopUpdateRequest.Name;
            shop.Type = shopUpdateRequest.Type;
            shop.Address = shopUpdateRequest.Address; 
            shop.Phone = shopUpdateRequest.Phone;    
            shop.Email = shopUpdateRequest.Email;     

            _shopRepository.Update(shop);
        }

        public void Delete(int id)
        {
            var shop = _shopRepository.GetById(id);
            if (shop == null)
                throw new NotFoundException("Shop", id);

            _shopRepository.Delete(shop);
        }
    }
}
