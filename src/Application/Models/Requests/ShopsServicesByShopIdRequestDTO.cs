using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Models.Requests
{
    public class ShopsServicesByShopIdRequestDTO
    {
        public int ServiceId { get; set; }
        public int ShopId { get; set; }
        public string ShopName {  get; set; }
        public string ServiceName {  get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public TimeSpan Duration { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Status Status { get; set; }


        public static ShopsServicesByShopIdRequestDTO Create(Service service, string shopName, int idShop)
        {
            var dto = new ShopsServicesByShopIdRequestDTO();
            dto.ShopId = idShop;
            dto.ServiceId = service.Id;
            dto.ShopName = shopName;
            dto.ServiceName = service.Name;
            dto.Description = service.Description;
            dto.Price = service.Price;
            dto.Duration = service.Duration;
            dto.Status = service.Status;
            return dto;
        }

        public static List<ShopsServicesByShopIdRequestDTO> CreateList(IEnumerable<Service> services, string shopName, int idShop)
        {
            List<ShopsServicesByShopIdRequestDTO> listDto = new List<ShopsServicesByShopIdRequestDTO>();
            foreach (var s in services)
            {
                listDto.Add(Create(s, shopName, idShop));
            }

            return listDto;
        }

    }
}
