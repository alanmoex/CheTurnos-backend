using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using Application.Models.Requests;
using Application.Models;
using Domain.Enums;
using Infrastructure.Data;

namespace Application
{
    public class ShopService : IShopService
    {
        private readonly IShopRepository _shopRepository;
        private readonly IOwnerRepository _ownerRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IServiceRepository _serviceRepository;
        private readonly IAppointmentRepository _appointmentRepository;

        public ShopService(IShopRepository shopRepository, IOwnerRepository ownerRepository, IEmployeeRepository employeeRepository, IServiceRepository serviceRepository, IAppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
            _shopRepository = shopRepository;
            _ownerRepository = ownerRepository;
            _employeeRepository = employeeRepository;
            _serviceRepository = serviceRepository;
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
                TimeStart = new TimeOnly(shopCreateRequest.StartHour, shopCreateRequest.StartMin),
                TimeEnd = new TimeOnly(shopCreateRequest.EndHour, shopCreateRequest.EndMin),
                WorkDays = shopCreateRequest.WorkDays,
                ImgUrl = shopCreateRequest.ImgUrl,
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

            if (shopUpdateRequest.StartHour >= 0 && shopUpdateRequest.StartMin >= 0 && shopUpdateRequest.EndHour >= 0 && shopUpdateRequest.EndMin >= 0)
            {
                if (shopUpdateRequest.StartHour < shopUpdateRequest.EndHour)
                {
                    shop.TimeStart = new TimeOnly(shopUpdateRequest.StartHour, shopUpdateRequest.StartMin);
                    shop.TimeEnd = new TimeOnly(shopUpdateRequest.EndHour, shopUpdateRequest.EndMin);
                }
            }

            //if (!string.IsNullOrEmpty(shopUpdateRequest.TimeEnd.ToString().Trim())) shop.TimeEnd = shopUpdateRequest.TimeEnd;

            //if (!string.IsNullOrEmpty(shopUpdateRequest.TimeStart.ToString().Trim())) shop.TimeStart = shopUpdateRequest.TimeStart;

            if (shopUpdateRequest.WorkDays.Count > 0) shop.WorkDays = shopUpdateRequest.WorkDays;

            _shopRepository.Update(shop);
        }

        public void PermanentDeletionShop(int id)
        {
            var shop = _shopRepository.GetById(id)
                ?? throw new NotFoundException(nameof(Shop), id);

            var listService = _serviceRepository.GetAllByShopId(shop.Id)
                ?? throw new NotFoundException(nameof(Service), id);

            var listEmploye = _employeeRepository.GetAllByShopId(shop.Id) ?? new List<Employee>();

            var ownerShop = _ownerRepository.GetAll().FirstOrDefault(owner => owner.ShopId == shop.Id)
                ?? throw new NotFoundException("Not found Owner.");

            var appointmentLsit = _appointmentRepository.GetAllAppointmentsByShopId(shop.Id) ?? new List<Appointment>();

            listService.ForEach(s => _serviceRepository.Delete(s));
            
            listEmploye.ForEach(e => _employeeRepository.Delete(e));

            appointmentLsit.ForEach(a=>_appointmentRepository.Delete(a));

            _ownerRepository.Delete(ownerShop);
            
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

        public ShopDTO? getShopWithoutOwner() 
        {
            var listShop = _shopRepository.GetAll();

            var newShop = listShop.OrderByDescending(s => s.Id).FirstOrDefault(); 

            if (newShop == null) return null;

            return ShopDTO.Create(newShop);
        }
    }
}
