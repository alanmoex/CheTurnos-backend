using Application.Models.Requests;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;

namespace Application;

public class ServiceService : IServiceService
{
    private readonly IServiceRepository _serviceRepository;
    public ServiceService(IServiceRepository serviceRepository)
    {
        _serviceRepository = serviceRepository;
    }

    public ServiceDTO Create(ServiceCreateRequest request)
    {
        var newService = new Service(
            request.Name, 
            request.Description, 
            request.Price, 
            request.Duration, 
            request.ServiceType);

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

        if (request.Duration.HasValue)
            service.Duration = request.Duration.Value;

        if (request.ServiceType.HasValue)
            service.ServiceType = request.ServiceType.Value;

        if (request.Status.HasValue)
            service.Status = request.Status.Value;

        _serviceRepository.Update(service);
    }
}