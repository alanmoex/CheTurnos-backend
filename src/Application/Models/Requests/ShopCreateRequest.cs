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
        public bool IsPremium { get; set; }
        public int AppoimentFrecuence { get; set; }
        public TimeSpan TimeStart { get; set; }
        public TimeSpan TimeEnd { get; set; }
        public List<Days> WorkDays { get; set; }
    }
}
