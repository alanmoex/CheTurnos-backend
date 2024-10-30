using Domain.Entities;
using Domain.Exceptions;
using Domain.Interface;
using Domain.Interfaces;
using Moq;

namespace Application.UnitTests
{
    public abstract class CrudTestsBase<TEntity, TDto, TUpdateRequest, TService, TRepository> 
        where TEntity : class, IEntity
        where TDto: class
        where TUpdateRequest : class
        where TService : class
        where TRepository : class, IRepositoryBase<TEntity>
    {
        protected Mock<TRepository> _repositoryMock;
        protected TService _service;

        protected CrudTestsBase(Mock<TRepository> repositoryMock)
        {
            _repositoryMock = repositoryMock ?? throw new ArgumentNullException(nameof(repositoryMock));
        }

        // Métodos abstractos que se implementan en las clases hijas
        protected abstract TService CreateService(TRepository repository);
        protected abstract TEntity CreateEntity();
        protected abstract List<TEntity> CreateEntities();
        protected abstract List<TDto?> GetAllFromService();
        protected abstract TEntity GetByIdOrThrow(int id);
        protected abstract TDto UpdateFromService(TUpdateRequest updateRequest, int id);
        protected abstract void DeleteFromService(int id);
        protected abstract TDto? GetByIdFromService(int id);
        protected abstract TUpdateRequest CreateUpdateRequest();
        protected virtual string EntityName => nameof(TEntity);
        //

        private void InitializeService()
        {
            _service = CreateService(_repositoryMock.Object); // Inicializa el servicio
        }

        [Fact]
        public void GetAllEntities_ReturnsEntityDTOList_WhenExists()
        {
            InitializeService();

            var entities = CreateEntities();

            _repositoryMock.Setup(r => r.GetAll()).Returns(entities);

            var result = GetAllFromService();

            Assert.NotNull(result);
            Assert.Equal(entities.Count, result.Count);
            Assert.IsType<List<TDto>>(result);
        }

        [Fact]
        public void GetAllEntities_ThrowsNotFoundException_WhenNone()
        {
            InitializeService();

            _repositoryMock.Setup(r => r.GetAll()).Returns(new List<TEntity>());

            var exception = Assert.Throws<NotFoundException>(() => GetAllFromService());
            Assert.Equal($"No se encontro ningun {EntityName}", exception.Message);
        }

        [Fact]
        public void GetByIdEntity_ReturnsEntityDto_WhenExists()
        {
            InitializeService();

            var entity = CreateEntity();

            _repositoryMock.Setup(r => r.GetById(entity.Id)).Returns(entity);

            var result = GetByIdFromService(entity.Id);

            Assert.NotNull(result);
            Assert.IsType<TDto>(result);
        }

        [Fact]
        public void GetByIdOrThrow_ThrowsNotFoundException_WhenAppointmentDoesNotExist()
        {
            var nonExistentId = 999;
            _repositoryMock.Setup(r => r.GetById(nonExistentId)).Returns((TEntity?)null);

            var exception = Assert.Throws<NotFoundException>(() => GetByIdOrThrow(nonExistentId));
            Assert.Equal($"Entity {EntityName} ({nonExistentId}) was not found.", exception.Message);
        }

        [Fact]
        public void DeleteEntity_CallsRepositoryDelete_WhenEntityExists()
        {
            InitializeService();

            var entity = CreateEntity();

            _repositoryMock.Setup(a => a.GetById(1)).Returns(entity);

            DeleteFromService(entity.Id);

            _repositoryMock.Verify(a => a.Delete(entity), Times.Once);
        }

        [Fact]
        public void UpdateEntity_ReturnEntityDto_WhenExists()
        {
            InitializeService();

            var updateRequest = CreateUpdateRequest();
            var existingEntity = CreateEntity();

            _repositoryMock.Setup(r => r.GetById(1)).Returns(existingEntity);

            var result = UpdateFromService(updateRequest, 1);

            Assert.NotNull(result);
            Assert.IsType<TDto>(result);
            _repositoryMock.Verify(r => r.Update(It.IsAny<TEntity>()), Times.Once);
        }
    }
}
