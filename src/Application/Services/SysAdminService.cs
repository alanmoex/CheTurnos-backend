using Application.Interfaces;
using Application.Models;
using Application.Models.Requests;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Interfaces;
using System; 
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Application.Services
{
    public class SysAdminService: ISysAdminService
    {
        private readonly IRepositoryUser _repositoryUser;
        private readonly IEmailService _emailService;
        private readonly IShopService _shopService;
        private readonly IEmployeeService _employeeService;

        public SysAdminService(IRepositoryUser repositoryUser, IEmailService emailService, IShopService shopService, IEmployeeService employeeService)
        {
            _repositoryUser = repositoryUser;
            _emailService = emailService;
            _shopService = shopService;
            _employeeService = employeeService;
        }

        public List<SysAdminDTO?> GetAll()
        {
            var list = _repositoryUser.GetAll();

            return SysAdminDTO.CreateList(list);
        }

        public SysAdminDTO? GetById(int id)
        {
            var admin = _repositoryUser.GetById(id)
                ?? throw new Exception("Not Found Admin");
            return SysAdminDTO.Create(admin);
        }



        public void Create(SysAdminCreateRequestDTO request)
        {
            bool validationFlag = ValidatePassword(request.Password);
            if (validationFlag)
            {
                validationFlag = ValidateName(request.Name);
                if (validationFlag)
                {
                    validationFlag = ValidateEmail(request.Email);
                    if (validationFlag)
                    {
                        var existentUser = _repositoryUser.GetByEmail(request.Email);

                        if (existentUser != null)
                        {
                            throw new Exception("El email que intenta utilizar ya existe");
                        }
                    }
                }
            }

            if (validationFlag)
            {
                var newAdmin = new User();

                newAdmin.Name = request.Name;
                newAdmin.Email = request.Email;
                newAdmin.Password = request.Password;
                newAdmin.Type = UserType.SysAdmin;
                newAdmin.ImgUrl = "";
                newAdmin.PasswordResetCode = Guid.NewGuid().ToString().Substring(0, 6);
                
                _repositoryUser.Add(newAdmin);
                _emailService.AccountCreationConfirmationEmail(request.Email, request.Name);
            }

            else
            {
                throw new ValidationException("Los datos ingresados no son válidos");
            }
        }



        public void Update(int id, SysAdminUpdateDTO request)
        {
            var admin = _repositoryUser.GetById(id)
                ?? throw new Exception("Not found");

            if (!string.IsNullOrEmpty(request.Name.Trim())) admin.Name = request.Name;

            if (!string.IsNullOrEmpty(request.Password.Trim()))
            {
                if (ValidatePassword(request.Password))
                {
                    admin.Password = request.Password;
                }
                else
                {
                    throw new ValidationException("Los datos ingresados no son válidos");
                }
            }

            _repositoryUser.Update(admin);
        }


        public void Delete(int id)
        {
            var user = _repositoryUser.GetById(id) ?? throw new Exception("User not found");

            switch (user.Type)
            {
                case UserType.Employee:
                    _employeeService.Delete(user.Id);
                    break;

                case UserType.Owner:
                    if (user is Owner owner && owner.ShopId != -1)
                    {
                        _shopService.PermanentDeletionShop(owner.ShopId);
                    }
                    else
                    {
                        throw new Exception("Owner must have a valid ShopId to delete.");
                    }
                    break;

                case UserType.Client:
                case UserType.SysAdmin:
                    _repositoryUser.Delete(user);
                    break;

                default:
                    throw new Exception("Unsupported user type.");
            }
        }

        public void LogicalDelete(int id)
        {
            var admin = _repositoryUser.GetById(id)
                ?? throw new Exception("not found");

            if (admin.Status == Status.Inactive)
                throw new Exception("El cliente especificado ya se encuentra inactivo");

            admin.Status = Status.Inactive;
            _repositoryUser.Update(admin);
        }




        private bool ValidatePassword(string password)
        {
            //comprobamos si la contraseña es nula o tiene menos de 8 caracteres
            if (string.IsNullOrEmpty(password) || password.Length < 8)
            {
                return false;
            }

            /*con esta expresión regular verificaremos que la contraseña contenga al menos una letra y un número*/
            string pattern = @"^(?=.*[a-zA-Z])(?=.*\d).+$";
            //la siguiente función devolverá true si hay match, y false en caso contrario
            return Regex.IsMatch(password, pattern);
        }
        private bool ValidateName(string name)
        {
            if (string.IsNullOrEmpty(name.Trim()))
            {
                return false;
            }
            return true;
        }
        private bool ValidateEmail(string email)
        {
            if (string.IsNullOrEmpty(email.Trim()))
            {
                return false;
            }
            else
            {
                try
                {
                    MailAddress mail = new MailAddress(email);
                    return true;
                }
                catch (FormatException)
                {
                    return false;
                }
            }
        }

    }
}
