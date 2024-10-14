using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Models
{
    public class OwnerDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ShopId { get; set; }
        public string Email { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public UserType Type { get; set; }
        public string ImgUrl { get; set; }

        public static OwnerDTO Create(Owner owner)
        {
            var dto = new OwnerDTO();
            dto.Id = owner.Id;
            dto.Name = owner.Name;
            dto.ShopId = owner.ShopId;
            dto.Email = owner.Email;
            dto.Type = owner.Type;
            dto.ImgUrl = owner.ImgUrl;
            return dto;
        }

        public static List<OwnerDTO?> CreateList(IEnumerable<Owner> owners)
        {
            List<OwnerDTO?> listDto = [];

            foreach (var o in owners)
            {
                listDto.Add(Create(o));
            }

            return listDto;
        }
    }

    
}