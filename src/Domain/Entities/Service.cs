﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Enums;

namespace Domain.Entities
{
    public class Service
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

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
        public Status Status { get; set; } = Status.Active;

        [Required]
        public ServiceType Type { get; set; }

        public List<User> ProvideBy { get; set; }

        public Service() { }

        public Service(string name, string description, decimal price, TimeSpan duration, ServiceType type)
        {
            Name = name;
            Description = description;
            Price = price;
            Duration = duration;
            Type = type;
            ProvideBy = new List<User>();
        }
        
    }
}
