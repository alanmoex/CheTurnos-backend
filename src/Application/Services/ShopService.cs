using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using Application.Models.Requests;
using Application.Models;
using Domain.Enums;

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

            if (!string.IsNullOrEmpty(shopUpdateRequest.Name.Trim())) shop.Name = shopUpdateRequest.Name;

            if (!string.IsNullOrEmpty(shopUpdateRequest.Address.Trim())) shop.Address = shopUpdateRequest.Address;

            if (!string.IsNullOrEmpty(shopUpdateRequest.Phone.Trim())) shop.Phone = shopUpdateRequest.Phone;

            if (!string.IsNullOrEmpty(shopUpdateRequest.Email.Trim())) shop.Email = shopUpdateRequest.Email;

            if (shopUpdateRequest.AppoimentFrecuence > 0) shop.AppoimentFrecuence = shopUpdateRequest.AppoimentFrecuence;

            if (!string.IsNullOrEmpty(shopUpdateRequest.TimeEnd.ToString().Trim())) shop.TimeEnd = shopUpdateRequest.TimeEnd;

            if (!string.IsNullOrEmpty(shopUpdateRequest.TimeStart.ToString().Trim())) shop.TimeStart = shopUpdateRequest.TimeStart;

            if (shopUpdateRequest.WorkDays.Count > 0) shop.WorkDays = shopUpdateRequest.WorkDays;

            _shopRepository.Update(shop);
        }

        public void PermanentDeletionShop(int id)
        {
            var shop = _shopRepository.GetById(id);

            if (shop == null)
                throw new NotFoundException(nameof(Shop), id);

            _shopRepository.Delete(shop);
        }

        public void LogicalDeletionShop(int id)
        {
            var shop = _shopRepository.GetById(id);

            if (shop == null)
                throw new NotFoundException(nameof(Shop), id);

            if (shop.Status == Status.Inactive)
            {
                throw new Exception("El negocio especificado ya se encuentra inactivo");
            }

            shop.Status = Status.Inactive;
            _shopRepository.Update(shop);
        }
    }
}
