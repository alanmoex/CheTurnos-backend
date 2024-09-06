using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Application.Models.Requests;

public class ServiceCreateRequest
{
    [Required]
    public string Name { get; set; }

    [Required]
    public string Description { get; set; }

    [Required]
    [Range(0, int.MaxValue, ErrorMessage = "must be a positive value.")]
    public decimal Price { get; set; }

    [Required]
    public TimeSpan Duration { get; set; }

    [Required]
    public ServiceType ServiceType { get; set; }
}

