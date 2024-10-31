using Application.Interfaces;
using Application.Models.Requests;
using Application.Models;
using Application.Services;
using Application.UnitTests;
using Domain.Entities;
using Domain.Interfaces;
using Moq;
using Domain.Enums;
using Infrastructure.Data;

namespace Application.UnitTests
{
    public class EmployeeServiceTests : CrudTestsBase<Employee, EmployeeResponseDTO, EmployeeUpdateRequest, EmployeeService, IEmployeeRepository>
    {
        private readonly Mock<IRepositoryUser> _userRepositoryMock;
        private readonly Mock<IEmailService> _emailServiceMock;
        private readonly Mock<IOwnerRepository> _ownerRepositoryMock;

        public EmployeeServiceTests()
            : base(new Mock<IEmployeeRepository>())
        {
            _userRepositoryMock = new Mock<IRepositoryUser>();
            _emailServiceMock = new Mock<IEmailService>();
            _ownerRepositoryMock = new Mock<IOwnerRepository>();

            _service = CreateService(_repositoryMock.Object);
        }

        // Implementación del método abstracto para crear el servicio con los repositorios simulados (mocks)
        protected override EmployeeService CreateService(IEmployeeRepository repository)
        {
            return new EmployeeService(repository, _userRepositoryMock.Object, _emailServiceMock.Object, _ownerRepositoryMock.Object);
        }

        protected override string EntityName => nameof(Employee);
        protected override Employee CreateEntity()
        {
            return mockEmployee;
        }

        protected override List<Employee> CreateEntities()
        {
            return mockEmployees;
        }
        protected override EmployeeUpdateRequest CreateUpdateRequest()
        {
            return mockEmployeeUpdateRequest;
        }

        protected override List<EmployeeResponseDTO?> GetAllFromService()
        {
            return _service.GetAll();
        }

        protected override EmployeeResponseDTO? GetByIdFromService(int id)
        {
            return _service.GetById(id);
        }

        protected override Employee GetByIdOrThrow(int id)
        {
            return _service.GetEmployeeByIdOrThrow(id);
        }

        protected override EmployeeResponseDTO UpdateFromService(EmployeeUpdateRequest updateRequest, int id)
        {
            _service.Update(id, updateRequest);
            return _service.GetById(id);
        }

        protected override void DeleteFromService(int id)
        {
            _service.Delete(id);
        }

        public List<Employee> mockEmployees = new List<Employee>
        {
            new Employee
            {
                Id = 1,
                Name = "John Doe",
                Email = "john.doe@example.com",
                Password = "Password123!",
                Type = UserType.Employee,
                Status = Status.Active,
                PasswordResetCode = "ABC123",
                ResetCodeExpiration = DateTime.UtcNow.AddHours(1),
                ImgUrl = "http://example.com/profile.jpg"
            },
            new Employee
            {
                Id = 2,
                Name = "Adam Smith",
                Email = "adam.smith@example.com",
                Password = "Password123!",
                Type = UserType.Employee,
                Status = Status.Active,
                PasswordResetCode = "ABC123",
                ResetCodeExpiration = DateTime.UtcNow.AddHours(1),
                ImgUrl = "http://example.com/profile.jpg"
            }
        };

        public Employee mockEmployee = new Employee
        {
            Id = 1,
            Name = "John Doe",
            Email = "john.doe@example.com",
            Password = "Password123!",
            Type = UserType.Employee,
            Status = Status.Active,
            PasswordResetCode = "ABC123",
            ResetCodeExpiration = DateTime.UtcNow.AddHours(1),
            ImgUrl = "http://example.com/profile.jpg"
        };


        public EmployeeUpdateRequest mockEmployeeUpdateRequest = new EmployeeUpdateRequest
        {
            Name = "George Washington",
            Password = "Password456!"
        };

    }
}
