using Domain.Enums;

namespace Application.Models.Requests
{
    public class ShopCreateRequest
    {
        public string Name { get; set; }
        public ShopType Type { get; set; }
        public string Address { get; set; } 
        public string Phone { get; set; }    
        public string Email { get; set; }    
    }
}
