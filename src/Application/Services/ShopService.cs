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

        public List<ShopDTO?> GetAll()
        {
            var shopsList = _shopRepository.GetAll();

            return ShopDTO.CreateList(shopsList);
        }

        public ShopDTO GetById(int id)
        {
            var shop = _shopRepository.GetById(id);

            if (shop == null)
                throw new NotFoundException(nameof(Shop), id);

            return ShopDTO.Create(shop);
        }

        public ShopDTO Create(ShopCreateRequest shopCreateRequest)
        {
            var shop = new Shop
            {
                Name = shopCreateRequest.Name,
                Type = shopCreateRequest.Type,
                Address = shopCreateRequest.Address, 
                Phone = shopCreateRequest.Phone,     
                Email = shopCreateRequest.Email,
                IsPremium = shopCreateRequest.IsPremium,
                AppoimentFrecuence = shopCreateRequest.AppoimentFrecuence,
                TimeEnd = shopCreateRequest.TimeEnd,
                TimeStart = shopCreateRequest.TimeStart,
                WorkDays = shopCreateRequest.WorkDays,
            };

            _shopRepository.Add(shop);

            return ShopDTO.Create(shop);
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
            shop.IsPremium = shopUpdateRequest.IsPremium;
            shop.AppoimentFrecuence = shopUpdateRequest.AppoimentFrecuence;
            shop.TimeEnd = shopUpdateRequest.TimeEnd;
            shop.WorkDays = shopUpdateRequest.WorkDays;
            shop.TimeStart = shopUpdateRequest.TimeStart;

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
