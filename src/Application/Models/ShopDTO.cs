using Domain.Entities;
using Domain.Enums;

namespace Application.Models
{
    public class ShopDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ShopType Type { get; set; }
        public Status Status { get; set; }
        public string Address { get; set; } 
        public string Phone { get; set; }    
        public string Email { get; set; }
    }
}
