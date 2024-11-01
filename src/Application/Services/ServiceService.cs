using Application.Models.Requests;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;

namespace Application;

public class ServiceService : IServiceService
{
    private readonly IServiceRepository _serviceRepository;
    private readonly IShopRepository _shopRepository;
    private readonly IShopService _shopService;
    public ServiceService(IServiceRepository serviceRepository, IShopRepository shopRepository, IShopService shopService)
    {
        _serviceRepository = serviceRepository;
        _shopRepository = shopRepository;
        _shopService= shopService;
    }

    public ServiceDTO Create(ServiceCreateRequest request) //CREA SOLO EL PRIMER SHOP EN EL REGISTRO
    {
        var duration = TimeSpan.Parse(request.Duration);
        var idShop = _shopService.getShopWithoutOwner().Id; //haca hay un bug en el futuro, busca el ulitmo shop y lo hagrega al nuevo owner. 
        
        var newService = new Service(
            request.Name, 
            request.Description, 
            request.Price, 
            duration,
            idShop
        );
        var obj = _serviceRepository.Add(newService);
        return ServiceDTO.Create(obj);
    }

    public ServiceDTO CreateOwnerService(ServiceCreateRequest request) //EL DUEÑO CREA UN SHOP
    {
        var duration = TimeSpan.Parse(request.Duration);
        var newService = new Service(
            request.Name,
            request.Description,
            request.Price,
            duration,
            request.ShopId
        );
        var obj = _serviceRepository.Add(newService);
        return ServiceDTO.Create(obj);
    }

    public void Delete(int id)
    {
        var obj = _serviceRepository.GetById(id)
            ?? throw new NotFoundException(typeof(Service).ToString(), id);

        _serviceRepository.Delete(obj);
    }

    public List<ServiceDTO> GetAll()
    {
        var list = _serviceRepository.GetAll();

        return ServiceDTO.CreateList(list);
    }



    public List<ServiceDTO> GetAllByShopId(int shopId)
    {
        var list = _serviceRepository.GetAllByShopId(shopId);

        return ServiceDTO.CreateList(list);
    }

    public ServiceDTO GetById(int id)
    {
        var obj = _serviceRepository.GetById(id)
            ?? throw new NotFoundException(typeof(Service).ToString(), id);

        return ServiceDTO.Create(obj);
    }

    public void Update(int id, ServiceUpdateRequest request)
    {
        var service = _serviceRepository.GetById(id)
            ?? throw new NotFoundException(typeof(Service).ToString(), id);

        

        if (!string.IsNullOrEmpty(request.Name))
            service.Name = request.Name;

        if (!string.IsNullOrEmpty(request.Description))
            service.Description = request.Description;

        if (request.Price.HasValue)
            service.Price = request.Price.Value;

        if (!string.IsNullOrEmpty(request.Duration))
        {
            var duration = TimeSpan.Parse(request.Duration);
            service.Duration = duration;
        }

        if (request.Status.HasValue)
            service.Status = request.Status.Value;

        _serviceRepository.Update(service);
    }


    public List<ShopsServicesByShopIdRequestDTO> GetServicesOfShop(int shopId) 
    {
        var shop = _shopRepository.GetById(shopId)
            ?? throw new Exception("Shop Not Found");
        var service = _serviceRepository.GetAllByShopId(shopId)
            ?? throw new Exception("Service Not Found");

        var newShopsService = ShopsServicesByShopIdRequestDTO.CreateList(service, shop.Name, shop.Id);
        return newShopsService;
    }


}