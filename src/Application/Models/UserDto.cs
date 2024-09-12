using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public UserType Type { get; set; }

        public static UserDto Create(User user)
        {
            var dto = new UserDto();
            dto.Id = user.Id;
            dto.Name = user.Name;
            dto.Email = user.Email;
            dto.Type = user.Type;

            return dto;
        }

        public static List<UserDto?> CreateList(IEnumerable<User> users)
        {
            List<UserDto?> listDto = [];

            foreach (var u in users)
            {
                listDto.Add(Create(u));
            }

            return listDto;
        }
    }
}