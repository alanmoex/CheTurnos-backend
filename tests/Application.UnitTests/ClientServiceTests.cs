using Application.Interfaces;
using Application.Models.Requests;
using Application.Models;
using Application.Services;
using Domain.Entities;
using Domain.Interfaces;
using Moq;
using Domain.Enums;

namespace Application.UnitTests
{
    public class ClientServiceTests : CrudTestsBase<Client, ClientDto, ClientUpdateRequest, ClientService, IClientRepository>
    {
        private readonly Mock<IRepositoryUser> _userRepositoryMock;
        private readonly Mock<IEmailService> _emailServiceMock;

        public ClientServiceTests()
            : base(new Mock<IClientRepository>())
        {
            _userRepositoryMock = new Mock<IRepositoryUser>();
            _emailServiceMock = new Mock<IEmailService>();

            _service = CreateService(_repositoryMock.Object);
        }

        // Implementación del método abstracto para crear el servicio con los repositorios simulados (mocks)
        protected override ClientService CreateService(IClientRepository repository)
        {
            return new ClientService(repository, _userRepositoryMock.Object, _emailServiceMock.Object);
        }

        protected override string EntityName => nameof(Client);
        protected override Client CreateEntity()
        {
            return mockClient;
        }

        protected override List<Client> CreateEntities()
        {
            return mockClients;
        }
        protected override ClientUpdateRequest CreateUpdateRequest()
        {
            return mockClientUpdateRequest;
        }

        protected override List<ClientDto?> GetAllFromService()
        {
            return _service.GetAllClients();
        }

        protected override ClientDto? GetByIdFromService(int id)
        {
            return _service.GetClientById(id);
        }

        protected override Client GetByIdOrThrow(int id)
        {
            return _service.GetClientByIdOrThrow(id);
        }

        protected override ClientDto UpdateFromService(ClientUpdateRequest updateRequest, int id)
        {
            _service.ModifyClientData(id, updateRequest);
            return _service.GetClientById(id);
        }

        protected override void DeleteFromService(int id)
        {
            _service.PermanentDeletionClient(id);
        }

        public List<Client> mockClients = new List<Client>
        {
            new Client
            {
                Id = 1,
                Name = "John Doe",
                Email = "john.doe@example.com",
                Password = "Password123!",
                Type = UserType.Client,
                Status = Status.Active,
                PasswordResetCode = "ABC123",
                ResetCodeExpiration = DateTime.UtcNow.AddHours(1),
                ImgUrl = "http://example.com/profile.jpg"
            },
            new Client
            {
                Id = 2,
                Name = "Adam Smith",
                Email = "adam.smith@example.com",
                Password = "Password123!",
                Type = UserType.Client,
                Status = Status.Active,
                PasswordResetCode = "ABC123",
                ResetCodeExpiration = DateTime.UtcNow.AddHours(1),
                ImgUrl = "http://example.com/profile.jpg"
            }
        };

        public Client mockClient = new Client
        {
            Id = 1,
            Name = "John Doe",
            Email = "john.doe@example.com",
            Password = "Password123!",
            Type = UserType.Client,
            Status = Status.Active,
            PasswordResetCode = "ABC123",
            ResetCodeExpiration = DateTime.UtcNow.AddHours(1),
            ImgUrl = "http://example.com/profile.jpg"
        };


        public ClientUpdateRequest mockClientUpdateRequest = new ClientUpdateRequest
        {
            Name = "George Washington",
            Password = "Password456!"
        };

    }
}