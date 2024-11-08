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
    public class OwnerServiceTests : CrudTestsBase<Owner, OwnerDTO, OwnerUpdateRequest, OwnerService, IOwnerRepository>
    {
        private readonly Mock<IShopService> _shopServiceMock;
        private readonly Mock<IEmployeeService> _employeeServiceMock;
        private readonly Mock<IAppointmentService> _appointmentServiceMock;
        private readonly Mock<IAppointmentRepository> _appointmentRepositoryMock;
        private readonly Mock<IRepositoryUser> _userRepositoryMock;
        private readonly Mock<IEmailService> _emailServiceMock;

        public OwnerServiceTests()
            : base(new Mock<IOwnerRepository>())
        {
            _userRepositoryMock = new Mock<IRepositoryUser>();
            _emailServiceMock = new Mock<IEmailService>();
            _shopServiceMock = new Mock<IShopService>();
            _employeeServiceMock = new Mock<IEmployeeService>();
            _appointmentServiceMock = new Mock<IAppointmentService>();
            _appointmentRepositoryMock = new Mock<IAppointmentRepository>();

            _service = CreateService(_repositoryMock.Object);
        }

        // Implementación del método abstracto para crear el servicio con los repositorios simulados (mocks)
        protected override OwnerService CreateService(IOwnerRepository repository)
        {
            return new OwnerService(_shopServiceMock.Object, _employeeServiceMock.Object,
                _appointmentServiceMock.Object, _appointmentRepositoryMock.Object, repository,
                _userRepositoryMock.Object, _emailServiceMock.Object);
        }

        protected override string EntityName => nameof(Owner);
        protected override Owner CreateEntity()
        {
            return mockOwner;
        }

        protected override List<Owner> CreateEntities()
        {
            return mockOwners;
        }
        protected override OwnerUpdateRequest CreateUpdateRequest()
        {
            return mockOwnerUpdateRequest;
        }

        protected override List<OwnerDTO?> GetAllFromService()
        {
            return _service.GetAllOwners();
        }

        protected override OwnerDTO? GetByIdFromService(int id)
        {
            return _service.GetOwnerById(id);
        }

        protected override Owner GetByIdOrThrow(int id)
        {
            return _service.GetOwnerByIdOrThrow(id);
        }

        protected override OwnerDTO UpdateFromService(OwnerUpdateRequest updateRequest, int id)
        {
            _service.ModifyOwnerData(id, updateRequest);
            return _service.GetOwnerById(id);
        }

        protected override void DeleteFromService(int id)
        {
            _service.PermanentDeletionOwner(id);
        }

        public List<Owner> mockOwners = new List<Owner>
        {
            new Owner
            {
                Id = 1,
                Name = "John Doe",
                Email = "john.doe@example.com",
                Password = "Password123!",
                Type = UserType.Owner,
                Status = Status.Active,
                PasswordResetCode = "ABC123",
                ResetCodeExpiration = DateTime.UtcNow.AddHours(1),
                ImgUrl = "http://example.com/profile.jpg"
            },
            new Owner
            {
                Id = 2,
                Name = "Adam Smith",
                Email = "adam.smith@example.com",
                Password = "Password123!",
                Type = UserType.Owner,
                Status = Status.Active,
                PasswordResetCode = "ABC123",
                ResetCodeExpiration = DateTime.UtcNow.AddHours(1),
                ImgUrl = "http://example.com/profile.jpg"
            }
        };

        public Owner mockOwner = new Owner
        {
            Id = 1,
            Name = "John Doe",
            Email = "john.doe@example.com",
            Password = "Password123!",
            Type = UserType.Owner,
            Status = Status.Active,
            PasswordResetCode = "ABC123",
            ResetCodeExpiration = DateTime.UtcNow.AddHours(1),
            ImgUrl = "http://example.com/profile.jpg"
        };


        public OwnerUpdateRequest mockOwnerUpdateRequest = new OwnerUpdateRequest
        {
            Name = "George Washington",
            Password = "Password456!"
        };

    }
}
