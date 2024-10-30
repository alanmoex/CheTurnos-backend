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
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class AppointmentService: IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IOwnerRepository _ownerRepository;
        private readonly IServiceRepository _serviceRepository;
        private readonly IRepositoryUser _repositoryUser;
        private readonly IClientRepository _clientRepository;
        private readonly IShopRepository _shopRepository;


        public AppointmentService (IAppointmentRepository appointmentRepository, IOwnerRepository ownerRepository, IServiceRepository serviceRepository, IRepositoryUser repositoryUser, IClientRepository clientRepository, IShopRepository shopRepository)
        {
            _appointmentRepository = appointmentRepository;
            _ownerRepository = ownerRepository;
            _serviceRepository = serviceRepository;
            _clientRepository = clientRepository;
            _repositoryUser = repositoryUser;
            _shopRepository = shopRepository;

        }

        public List<AppointmentDTO?> GetAllAppointment()
        {
            var listAppointment = _appointmentRepository.GetAll()
                ?? throw new NotFoundException("list not found");
            return AppointmentDTO.CreateList(listAppointment);
        }

        public AppointmentDTO? GetAppointmentById(int id)
        {
            var obj = _appointmentRepository.GetById(id)
                ?? throw new NotFoundException("Appointment not found");
            return AppointmentDTO.Create(obj);
        }

        public void DeleteAppointment (int id) 
        {
            var obj = _appointmentRepository.GetById(id)
                ?? throw new NotFoundException("Appointment not found");

            _appointmentRepository.Delete(obj);
        }

        public List<EmployeeAppointmentListDTO> GetAvailableAppointmentsByEmployeeId(int employeeId)
        {
            var EmployeeAppointments = _appointmentRepository.GetAvailableAppointmentsByEmployeeId(employeeId)
                ?? throw new NotFoundException("Appointment not found");

            List<EmployeeAppointmentListDTO> appointmentList = [];

            foreach (var a in EmployeeAppointments)
            {
                var shopName = _shopRepository.GetById(a.ShopId)?.Name ?? string.Empty;
                var serviceName = _serviceRepository.GetById(a.ServiceId)?.Name ?? string.Empty;
                var ClientName = _clientRepository.GetById(a.ClientId)?.Name ?? string.Empty;

                appointmentList.Add(EmployeeAppointmentListDTO.Create(a, serviceName, shopName, ClientName));
            }
            return appointmentList;
        }
        public List<ClientsAppointmentListDTO> GetAvailableAppointmentsByClienId(int clientId)
        {
            var clientAppointments = _appointmentRepository.GetAvailableAppointmentsByClientId(clientId)
                ?? throw new NotFoundException("Appointment not found");

            List<ClientsAppointmentListDTO> appointmentList = [];

            foreach (var a in clientAppointments)
            {
                var shopName = _shopRepository.GetById(a.ShopId)?.Name ?? string.Empty;
                var serviceName = _serviceRepository.GetById(a.ServiceId)?.Name ?? string.Empty;

                appointmentList.Add(ClientsAppointmentListDTO.Create(a, serviceName, shopName));
            }
                return appointmentList;
        }

        public void CreateAppointment (int shopId, int providerId, DateTime dateAndHour, int? serviceId = null, int? clientId = null)
        {
            var newObj = new Appointment();
            newObj.ServiceId= serviceId;
            newObj.ProviderId = providerId;
            newObj.ClientId = clientId;
            newObj.ShopId = shopId;
            newObj.DateAndHour = dateAndHour;
            newObj.Duration = new TimeSpan();

            _appointmentRepository.Add(newObj);
        }

        public AppointmentDTO UpdateAppointment (AppointmentUpdateRequest appointment, int id)
        {
            var obj = _appointmentRepository.GetById(id)
                ?? throw new NotFoundException("Appointment not found");

            obj.Status = appointment.Status;
            obj.ServiceId = appointment.ServiceId;
            obj.ProviderId= appointment.ProviderId;
            obj.ClientId= appointment.ClientId;
            obj.DateAndHour = appointment.DateAndHour;
            //obj.Duration = appointment.Duration;

            _appointmentRepository.Update(obj);
            return AppointmentDTO.Create(obj);
        }

        public void AssignClient(AssignClientRequestDTO request)
        {
            var obj = _appointmentRepository.GetById(request.IdAppointment)
                ?? throw new NotFoundException("Appointment not found");
            obj.ServiceId = request.ServiceId;
            obj.ClientId = request.ClientId;
            obj.Status = Status.Inactive;
            _appointmentRepository.Update(obj);

        }

        public AppointmentDTO GetLastAppointmentByShopId(int ownerId)
        {
            var owner = _ownerRepository.GetById(ownerId);

            var lastAppointment = _appointmentRepository.GetLastAppointmentByShopId(owner.ShopId);
            if (lastAppointment == null)
            {
                throw new NotFoundException("Appointment not found");
            }

            var lastAppDTO = AppointmentDTO.Create(lastAppointment);
            return lastAppDTO;
        }

        public List<AllApointmentsOfMyShopRequestDTO?> GetAllApointmentsOfMyShop(int ownerId) //Camiar DTO
        {
            var owner = _ownerRepository.GetById(ownerId);
            List<Appointment> myAppList = _appointmentRepository.GetAllAppointmentsByShopId(owner.ShopId);

            List<AllApointmentsOfMyShopRequestDTO?> listDto = [];

            foreach (var a in myAppList)
            {
                var provider = _repositoryUser.GetById(a.ProviderId)?.Name ?? string.Empty;
                var client = _clientRepository.GetById(a.ClientId)?.Name ?? string.Empty;
                var service = _serviceRepository.GetById(a.ServiceId)?.Name ?? string.Empty;

                listDto.Add(AllApointmentsOfMyShopRequestDTO.Create(a, provider,client, service));
            }
            return listDto;
        }

        public List<AllApointmentsOfMyShopRequestDTO> GetAllAppointmentsByProviderId(int providerId)
        {
            var appointmentList = _appointmentRepository.GetAllAppointmentsByProviderId(providerId)
                ?? throw new Exception("not found appointments");
            List<AllApointmentsOfMyShopRequestDTO> listDto = [];

            foreach (var a in appointmentList)
            {
                var provider = _repositoryUser.GetById(a.ProviderId)?.Name ?? string.Empty;
                var client = _clientRepository.GetById(a.ClientId)?.Name ?? string.Empty;
                var service = _serviceRepository.GetById(a.ServiceId)?.Name ?? string.Empty;

                listDto.Add(AllApointmentsOfMyShopRequestDTO.Create(a, provider, client, service));
            }
            return listDto;
        }
    }
}
