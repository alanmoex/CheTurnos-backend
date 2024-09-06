using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Application.Models.Requests;

public class ServiceUpdateRequest
{
    public string? Name { get; set; }
    public string? Description { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Price must be a positive value.")]
    public decimal? Price { get; set; }

    public TimeSpan? Duration { get; set; }

    public ServiceType? ServiceType { get; set; }

    public Status? Status { get; set; }
}

