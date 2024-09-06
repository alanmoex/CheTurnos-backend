using Application.Models.Requests;
using Domain.Entities;

namespace Application;

public interface IServiceService
{
    List<ServiceDTO> GetAll();
    ServiceDTO GetById(int id);
    ServiceDTO Create(ServiceCreateRequest serviceCreateRequest);
    void Update(int id, ServiceUpdateRequest serviceUpdateRequest);
    void Delete(int id);
}