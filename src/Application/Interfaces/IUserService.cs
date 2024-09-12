using Application.Models.Requests;
using Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IUserService
    {
        UserDto? GetUserById(int id);

        List<UserDto?> GetAllUsers();

        UserDto CreateNewUser(UserCreateRequest userCreateRequest);

        void ModifyUserData(int id, UserUpdateRequest userUpdateRequest);

        void DeleteUser(int id);
    }
}