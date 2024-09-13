using Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

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
    [RegularExpression(@"^([0-1]?[0-9]|2[0-3]):[0-5][0-9]:[0-5][0-9]$",
            ErrorMessage = "The time must be in the format HH:mm:ss")]
    public string Duration { get; set; }

    [Required]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ServiceType ServiceType { get; set; }
}

