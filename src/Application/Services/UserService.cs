using Application.Interfaces;
using Application.Models;
using Application.Models.Requests;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public List<UserDto?> GetAllUsers()
        {
            var usersList = _userRepository.GetAll();

            return UserDto.CreateList(usersList);
        }

        public UserDto? GetUserById(int id)
        {
            var user = _userRepository.GetById(id);

            if (user == null)
                throw new NotFoundException(nameof(User), id);

            return UserDto.Create(user);
        }

        public UserDto CreateNewUser(UserCreateRequest userCreateRequest)
        {
            var newUser = new User();
            newUser.Name = userCreateRequest.Name;
            newUser.Email = userCreateRequest.Email;
            newUser.Password = userCreateRequest.Password;
            newUser.Type = UserType.Client;

            return UserDto.Create(_userRepository.Add(newUser));
        }

        public void ModifyUserData(int id, UserUpdateRequest userUpdateRequest)
        {
            var user = _userRepository.GetById(id);

            if (user == null)
                throw new NotFoundException(nameof(User), id);

            if (userUpdateRequest.Name != string.Empty) user.Name = userUpdateRequest.Name;

            if (userUpdateRequest.Password != string.Empty) user.Password = userUpdateRequest.Password;

            _userRepository.Update(user);
        }

        public void DeleteUser(int id)
        {
            var user = _userRepository.GetById(id);

            if (user == null)
                throw new NotFoundException(nameof(User), id);

            _userRepository.Delete(user);
        }
    }
}