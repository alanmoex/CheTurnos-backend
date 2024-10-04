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
using System.Threading.Tasks;

namespace Application.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public bool Create(EmployeeCreateRequestDTO request)
        {
            if (request.Name == null || request.Email == null || request.Password == null || request.Password == null)
            {
                return false; //no se puede crear
            }

            var newEmployee = new Employee();
            newEmployee.Name = request.Name;
            newEmployee.Email = request.Email;
            newEmployee.Password = request.Password;
            newEmployee.ShopId = request.ShopId;
            newEmployee.Type = Domain.Enums.UserType.Employee;

            try
            {
                _employeeRepository.Add(newEmployee);
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

            if (employee == null)
            {
                return false;
            }

            try
            {
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
            try
            {
                var list = _employeeRepository.GetAll();

                return EmployeeResponseDTO.CreateList(list);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
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
            var employee = _employeeRepository.GetById(id);

            return EmployeeResponseDTO.Create(employee);

        }

        public bool Update(int id, EmployeeUpdateRequest request)
        {
            var employee = _employeeRepository.GetById(id);

            if (employee == null) return false;

            employee.Email = request.Email;
            employee.Name = request.Name;
            employee.Password = request.Password;

            try
            {
                _employeeRepository.Update(employee);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

        }
    }
}
