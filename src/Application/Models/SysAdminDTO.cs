using Application.Interfaces;
using Application.Models.Requests;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Application.Models
{
    public class SysAdminDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [EmailAddress(ErrorMessage = "invalid Email Address")]
        public string Email { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public UserType Type { get; set; }

        public Status Status { get; set; } = Status.Active;

        public string ImgUrl { get; set; }


        public static SysAdminDTO Create(User admin)
        {
            var dto = new SysAdminDTO();
            dto.Id = admin.Id;
            dto.Name = admin.Name;
            dto.Email = admin.Email;
            dto.Type = admin.Type;
            dto.Status = admin.Status; 
            dto.ImgUrl = admin.ImgUrl;
            return dto;
        }

        public static List<SysAdminDTO?> CreateList(IEnumerable<User> admin)
        {
            List<SysAdminDTO?> listDto = [];

            foreach (var a in admin)
            {
                listDto.Add(Create(a));
            }

            return listDto;
        }








    }
}
