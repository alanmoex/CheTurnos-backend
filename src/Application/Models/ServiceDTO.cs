using Domain.Entities;
using Domain.Enums;

namespace Application;

public class ServiceDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public TimeSpan Duration { get; set; }
    public Status Status { get; set; }

    public static ServiceDTO Create(Service service)
    {
        var dto = new ServiceDTO();
        dto.Id = service.Id;
        dto.Name = service.Name;
        dto.Description = service.Description;
        dto.Price = service.Price;
        dto.Duration = service.Duration;
        dto.Status = service.Status;
        return dto;
    }

    public static List<ServiceDTO> CreateList(IEnumerable<Service> services)
    {
        List<ServiceDTO> listDto = new List<ServiceDTO>();
        foreach (var s in services)
        {
            listDto.Add(Create(s));
        }

        return listDto;
    }
}