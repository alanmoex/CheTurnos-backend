using Domain.Enums;

namespace Application.Models.Requests
{
    public class ShopUpdateRequest
    {
        public string Name { get; set; }
        public ShopType Type { get; set; }
        public string Address { get; set; } 
        public string Phone { get; set; }    
        public string Email { get; set; }
        public bool IsPremium { get; set; }
        public int AppoimentFrecuence { get; set; }
        public int StartHour { get; set; }
        public int StartMin { get; set; }
        public int EndHour { get; set; }
        public int EndMin { get; set; }
        public List<Days> WorkDays { get; set; }
    }
}
