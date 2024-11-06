using Application.Interfaces;
using Application.Models;
using Application.Models.Requests;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Application.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IRepositoryUser _userRepository;
        private readonly IEmailService _emailService;
        private readonly IOwnerRepository _ownerRepository;
        private readonly IAppointmentRepository _appointmentRepository;

        public EmployeeService(IEmployeeRepository employeeRepository, IRepositoryUser userRepository, IEmailService emailService, IOwnerRepository ownerRepository, IAppointmentRepository appointmentRepository)
        {
            _employeeRepository = employeeRepository;
            _userRepository = userRepository;
            _emailService = emailService;
            _ownerRepository = ownerRepository;
            _appointmentRepository = appointmentRepository;
        }

        public bool Create(EmployeeCreateRequestDTO request)
        {
            if (request.Name == null || request.Email == null || request.Password == null || request.ShopId == null)
            {
                return false; //no se puede crear
            }

            var newEmployee = new Employee();
            newEmployee.Name = request.Name;
            newEmployee.Email = request.Email;
            newEmployee.Password = request.Password;
            newEmployee.ShopId = request.ShopId;
            newEmployee.Type = Domain.Enums.UserType.Employee;
            //Nuevos atributos de Usuario.
            newEmployee.ImgUrl = "";
            newEmployee.PasswordResetCode = Guid.NewGuid().ToString().Substring(0, 6);
            try
            {
                _employeeRepository.Add(newEmployee);
                //_emailService.AccountCreationConfirmationEmail(newEmployee.Email, newEmployee.Name);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public bool Delete(int id)
        {
            var employee = _employeeRepository.GetById(id);
            var employeeAppointmentList = _appointmentRepository.GetAllAppointmentsByProviderId(employee.Id);

            if (employee == null)
            {
                return false;
            }

            try
            {
                foreach (var appointment in employeeAppointmentList)
                {
                    _appointmentRepository.Delete(appointment);
                }

                _employeeRepository.Delete(employee);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public List<EmployeeResponseDTO?> GetAll()
        {
            
            var list = _employeeRepository.GetAll();
            if (list == null || !list.Any())
            {
                throw new NotFoundException($"No se encontro ningun {nameof(Employee)}");
            }

            return EmployeeResponseDTO.CreateList(list);
            
        }

        public Employee GetEmployeeByIdOrThrow(int id)
        {
            var employee = _employeeRepository.GetById(id);
            if (employee == null)
            {
                throw new NotFoundException(nameof(Employee), id);
            }
            return employee;
        }

        public List<EmployeeResponseDTO?> GetAllByShopId(int shopId)
        {
            try
            {
                var list = _employeeRepository.GetAllByShopId(shopId);

                return EmployeeResponseDTO.CreateList(list);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public EmployeeResponseDTO? GetById(int id)
        {
            var employee = GetEmployeeByIdOrThrow(id);

            return EmployeeResponseDTO.Create(employee);

        }

        public List<EmployeeResponseDTO?> GetAvailables(int shopId)
        {
            try
            {
                var appointments = _employeeRepository.GetAvailables(shopId);

                if (appointments != null)
                {
                    List<EmployeeResponseDTO?> employeesAvailable = new List<EmployeeResponseDTO>();

                    foreach (var a in appointments)
                    {
                        var employee = _employeeRepository.GetById(a.ProviderId);
                        var employeeToAdd = EmployeeResponseDTO.Create(employee);
                        employeesAvailable.Add(employeeToAdd);
                    };
                    return employeesAvailable;
                }
                return null;
            }
            catch (Exception ex)
            {
               throw new Exception(ex.ToString());
            }

        }

        public void Update(int id, EmployeeUpdateRequest request)
        {
            var employee = _employeeRepository.GetById(id) ?? throw new NotFoundException("User not Found");

            if (employee.Password != request.ConfirmationPassword) throw new Exception("Passwords do not match");
    
            if (!string.IsNullOrEmpty(request.Name.Trim())) employee.Name = request.Name;
            
            if (string.IsNullOrEmpty(request.NewPassword.Trim())) throw new Exception("Empty NewPassword.");
            if (!ValidatePassword(request.NewPassword)) throw new Exception("NewPassword is not validate");
            employee.Password = request.NewPassword;

            _employeeRepository.Update(employee);
        }

        public List<EmployeeResponseDTO?> GetMyShopEmployees(int ownerId)
        {
            var owner = _ownerRepository.GetById(ownerId);
            var myEmployees = _employeeRepository.GetAllByShopId(owner.ShopId);

            return EmployeeResponseDTO.CreateList(myEmployees);
        }

        private bool ValidatePassword(string password)
        {
            if (string.IsNullOrEmpty(password) || password.Length < 8)
            {
                return false;
            }

            string pattern = @"^(?=.*[a-zA-Z])(?=.*\d).+$";
            return Regex.IsMatch(password, pattern);
        }
    }
}
