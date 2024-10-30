using Application.Interfaces;
using Application.Models;
using Application.Models.Requests;
using Application.Services;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Interface;
using Domain.Interfaces;
using Moq;

namespace Application.UnitTests
{
    public class AppointmentServiceTests : CrudTestsBase<Appointment, AppointmentDTO, AppointmentUpdateRequest, AppointmentService, IAppointmentRepository>
    {
        private readonly Mock<IOwnerRepository> _ownerRepositoryMock;
        private readonly Mock<IServiceRepository> _serviceRepositoryMock;
        private readonly Mock<IRepositoryUser> _userRepositoryMock;
        private readonly Mock<IClientRepository> _clientRepositoryMock;
        private readonly Mock<IShopRepository> _shopRepositoryMock;
        private readonly Mock<IEmailService> _emailServiceMock;

        public AppointmentServiceTests()
            : base(new Mock<IAppointmentRepository>())
        {
            _ownerRepositoryMock = new Mock<IOwnerRepository>();
            _emailServiceMock = new Mock<IEmailService>();
            _clientRepositoryMock = new Mock<IClientRepository>();
            _serviceRepositoryMock = new Mock<IServiceRepository>();
            _userRepositoryMock = new Mock<IRepositoryUser>();
            _shopRepositoryMock = new Mock<IShopRepository>();

            _service = CreateService(_repositoryMock.Object);
        }

        // Implementación del método abstracto para crear el servicio con los repositorios simulados (mocks)
        protected override AppointmentService CreateService(IAppointmentRepository repository)
        {
            return new AppointmentService(repository, _ownerRepositoryMock.Object, _serviceRepositoryMock.Object, _userRepositoryMock.Object, _clientRepositoryMock.Object, _shopRepositoryMock.Object, _emailServiceMock.Object);
        }

        protected override string EntityName => nameof(Appointment);
        protected override Appointment CreateEntity()
        {
            return mockAppointment;
        }

        protected override List<Appointment> CreateEntities()
        {
            return mockAppointments;
        }
        protected override AppointmentUpdateRequest CreateUpdateRequest()
        {
            return mockAppointmentUpdateRequest;
        }

        protected override List<AppointmentDTO?> GetAllFromService()
        {
            return _service.GetAllAppointment();
        }

        protected override AppointmentDTO? GetByIdFromService(int id)
        {
            return _service.GetAppointmentById(id);
        }

        protected override Appointment GetByIdOrThrow(int id)
        {
            return _service.GetAppointmentByIdOrThrow(id);
        }

        protected override AppointmentDTO UpdateFromService(AppointmentUpdateRequest updateRequest, int id)
        {
            return _service.UpdateAppointment(updateRequest, id); 
        }

        protected override void DeleteFromService(int id)
        {
            _service.DeleteAppointment(id);
        }

        [Fact]
        public void DeleteAppointment_NotifiesClient_WhenClientIdIsNotNull()
        {
            var appointment = mockAppointmentWithClient;
            var client = new User { Id = appointment.ClientId.Value, Email = "client@example.com", Name = "John Doe" };
            var shop = new Shop { Id = appointment.ShopId, Name = "Test Shop", Phone = "123-456-7890" };

            _repositoryMock.Setup(r => r.GetById(appointment.Id)).Returns(appointment);
            _userRepositoryMock.Setup(u => u.GetById(appointment.ClientId)).Returns(client);
            _shopRepositoryMock.Setup(s => s.GetById(appointment.ShopId)).Returns(shop);

            _service.DeleteAppointment(appointment.Id);

            _emailServiceMock.Verify(e => e.NotifyClientCancellation(client.Email, client.Name, shop.Name, shop.Phone), Times.Once);
        }

        [Fact]
        public void GetAvailableAppointmentsByEmployeeId_ReturnsAppointmentsDtoList_WhenEmployeeHaveAppointments()
        {
            int employeeId = 1;
            _repositoryMock.Setup(a => a.GetAvailableAppointmentsByEmployeeId(employeeId)).Returns(mockAppointments);

            var result = _service.GetAvailableAppointmentsByEmployeeId(employeeId);

            Assert.NotEmpty(result);
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.IsType<List<AppointmentDTO>>(result);
        }

        [Fact]
        public void GetAvailableAppointmentsByEmployeeId_ThrowsNotFoundException_WhenEmployeeHaveNotAppointments()
        {
            int employeeId = 1;
            _repositoryMock.Setup(a => a.GetAvailableAppointmentsByEmployeeId(employeeId)).Returns(new List<Appointment>());

            var exception = Assert.Throws<NotFoundException>(() => _service.GetAvailableAppointmentsByEmployeeId(employeeId));
            Assert.Equal($"No se encontró ningun {EntityName}", exception.Message);
        }

        [Fact]
        public void GetAvailableAppointmentsByClientId_ReturnsAppointmentsDtoList_WhenEmployeeHaveAppointments()
        {
            int clientId = 1;
            var appointments = mockAppointments;
            _repositoryMock.Setup(a => a.GetAvailableAppointmentsByClientId(clientId)).Returns(appointments);

            var result = _service.GetAvailableAppointmentsByClientId(clientId);

            Assert.NotEmpty(result);
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.IsType<List<AppointmentDTO>>(result);
        }

        [Fact]
        public void GetAvailableAppointmentsByClientId_ThrowsNotFoundException_WhenEmployeeDontHaveAppointments()
        {
            int clientId = 1;
            var appointments = new List<Appointment>();
            _repositoryMock.Setup(a => a.GetAvailableAppointmentsByClientId(clientId)).Returns(appointments);

            var exception = Assert.Throws<NotFoundException>(() => _service.GetAvailableAppointmentsByClientId(clientId));
            Assert.Equal($"No se encontró ningun {EntityName}", exception.Message);
        }

        [Fact]
        public void GetLastAppointmentByShopId_ReturnsAppointmentsDtoList_WhenShopHaveAppointments()
        {
            int shopId = 1;
            var owner = mockOwner;
            var appointment = mockAppointment;
            _repositoryMock.Setup(a => a.GetLastAppointmentByShopId(shopId)).Returns(appointment);
            _ownerRepositoryMock.Setup(o => o.GetById(owner.Id)).Returns(owner);

            var result = _service.GetLastAppointmentByShopId(shopId);

            Assert.NotNull(result);
            Assert.IsType<AppointmentDTO>(result);
        }

        [Fact]
        public void GetLastAppointmentByShopId_ThrowsNotFoundException_WhenOwnerDontExists()
        {
            int shopId = 1;
            int ownerId = 1;
            var appointment = mockAppointment;
            _repositoryMock.Setup(a => a.GetLastAppointmentByShopId(shopId)).Returns(appointment);
            _ownerRepositoryMock.Setup(o => o.GetById(ownerId)).Returns((Owner?)null);

            var exception = Assert.Throws<NotFoundException>(() => _service.GetLastAppointmentByShopId(shopId));
            Assert.Equal($"Entity Owner ({ownerId}) was not found.", exception.Message);
        }

        [Fact]
        public void GetAllApointmentsOfMyShop_ReturnsAppointmentsDtoList_WhenShopHaveAppointments()
        {
            int shopId = 1;
            _repositoryMock.Setup(a => a.GetAllAppointmentsByShopId(shopId)).Returns(mockAppointments);
            _ownerRepositoryMock.Setup(o => o.GetById(shopId)).Returns(mockOwner);

            var result = _service.GetAllApointmentsOfMyShop(shopId);

            Assert.NotEmpty(result);
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.IsType<List<AllApointmentsOfMyShopRequestDTO>>(result);
        }

        [Fact]
        public void GetAllApointmentsOfMyShop_ThrowsNotFoundException_WhenOwnerDontExists()
        {
            int shopId = 1;
            int ownerId = 1;
            _repositoryMock.Setup(a => a.GetAllAppointmentsByShopId(shopId)).Returns(mockAppointments);
            _ownerRepositoryMock.Setup(o => o.GetById(ownerId)).Returns((Owner?)null);

            var exception = Assert.Throws<NotFoundException>(() => _service.GetAllApointmentsOfMyShop(1));
            Assert.Equal($"Entity Owner ({ownerId}) was not found.", exception.Message);
        }

        [Fact]
        public void GetAllAppointmentsByProviderId_ReturnsAppointmentsDtoList_WhenProviderHaveAppointments()
        {
            int providerId = 1;
            _repositoryMock.Setup(a => a.GetAllAppointmentsByProviderId(providerId)).Returns(mockAppointments);

            var result = _service.GetAllAppointmentsByProviderId(providerId);

            Assert.NotEmpty(result);
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.IsType<List<AllApointmentsOfMyShopRequestDTO>>(result);
        }

        [Fact]
        public void GetAllAppointmentsByProviderId_ThrowsNotFoundException_WhenProviderDontHaveAppointments()
        {
            int providerId = 1;
            _repositoryMock.Setup(a => a.GetAllAppointmentsByProviderId(providerId)).Returns(new List<Appointment>());

            var exception = Assert.Throws<NotFoundException>(() => _service.GetAllAppointmentsByProviderId(1));
            Assert.Equal($"No se encontró ningun {EntityName}", exception.Message);
        }

        [Fact]
        public void CreateAppointment_CallsRepositoryAdd_WhenAllParametersHasValue()
        {
            var shopId = 1;
            var providerId = 2;
            var dateAndHour = new DateTime(2024, 10, 18, 15, 0, 0);
            int? serviceId = 3;
            int? clientId = 4;

            _service.CreateAppointment(shopId, providerId, dateAndHour, serviceId, clientId);

            _repositoryMock.Verify(a => a.Add(It.Is<Appointment>(app =>
                app.ShopId == shopId &&
                app.ProviderId == providerId &&
                app.DateAndHour == dateAndHour &&
                app.ServiceId == serviceId &&
                app.ClientId == clientId &&
                app.Duration == TimeSpan.Zero
            )), Times.Once);
        }

        [Fact]
        public void CreateAppointment_CallsRepositoryAdd_WhenServiceIdAndClientIdAreNull()
        {
            var shopId = 1;
            var providerId = 2;
            var dateAndHour = new DateTime(2024, 10, 18, 15, 0, 0);

            _service.CreateAppointment(shopId, providerId, dateAndHour);

            _repositoryMock.Verify(a => a.Add(It.Is<Appointment>(app =>
                app.ShopId == shopId &&
                app.ProviderId == providerId &&
                app.DateAndHour == dateAndHour &&
                app.ServiceId == null &&
                app.ClientId == null &&
                app.Duration == TimeSpan.Zero
            )), Times.Once);
        }

        [Fact]
        public void AssignClient_CallsRepositoryUpdate_WhenAppointmentExists()
        {
            var assignClientRequest = new AssignClientRequestDTO()
            {
                IdAppointment = 1,
                ServiceId = 1,
                ClientId = 1,
            };

            _repositoryMock.Setup(r => r.GetById(mockAppointment.Id)).Returns(mockAppointment);

            _service.AssignClient(assignClientRequest);

            _repositoryMock.Verify(r => r.Update(It.Is<Appointment>(app =>
                app.ServiceId == assignClientRequest.ServiceId &&
                app.ClientId == assignClientRequest.ClientId
            )), Times.Once);
        }

        public List<Appointment> mockAppointments = new List<Appointment>
        {
            new Appointment
            {
                Id = 1,
                Status = Status.Active,
                ServiceId = 1,
                ProviderId = 1,
                ClientId = 1,
                ShopId = 1,
                DateAndHour = new DateTime(2024, 10, 18, 18, 0, 0),
                Duration = TimeSpan.FromHours(1)
            },
            new Appointment
            {
                Id = 2,
                Status = Status.Active,
                ServiceId = 2,
                ProviderId = 2,
                ClientId = 2,
                ShopId = 2,
                DateAndHour = new DateTime(2024, 10, 19, 17, 0, 0),
                Duration = TimeSpan.FromHours(1.5)
            }
        };

        public Appointment mockAppointment = new Appointment
        {
            Id = 1,
            Status = Status.Active,
            ServiceId = 1,
            ProviderId = 1,
            ClientId = null,
            ShopId = 1,
            DateAndHour = new DateTime(2024, 10, 18, 18, 0, 0),
            Duration = TimeSpan.FromHours(1)
        };

        public Appointment mockAppointmentWithClient = new Appointment
        {
            Id = 1,
            Status = Status.Active,
            ServiceId = 1,
            ProviderId = 1,
            ClientId = 1,
            ShopId = 1,
            DateAndHour = new DateTime(2024, 10, 18, 18, 0, 0),
            Duration = TimeSpan.FromHours(1)
        };

        public AppointmentUpdateRequest mockAppointmentUpdateRequest = new AppointmentUpdateRequest
        {
            Status = Status.Inactive,
            ServiceId = 2,
            ProviderId = 2,
            ClientId = 2,
            DateAndHour = new DateTime(2024, 10, 18, 16, 0, 0)
        };

        public Owner mockOwner = new Owner
        {
            Id = 1,
            ShopId = 1
        };
    }
}