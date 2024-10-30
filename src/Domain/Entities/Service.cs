using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Enums;
using Domain.Interfaces;

namespace Domain.Entities
{
    public class Service : IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int ShopId { get; set; } //clave foranea para saber de que negocio es

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "deberia ser un valor positivo")]
        public decimal Price { get; set; }

        [Required]
        public TimeSpan Duration { get; set; } //en minutos

        [Required]
        public Status Status { get; set; } = Status.Active;

        public Service() { }

        public Service(string name, string description, decimal price, TimeSpan duration, int shopId)
        {
            Name = name;
            Description = description;
            Price = price;
            Duration = duration;
            ShopId = shopId;
        }
        
    }
}
