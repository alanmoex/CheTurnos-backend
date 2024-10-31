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
        private readonly IEmailService _emailService;

        public AppointmentService (IAppointmentRepository appointmentRepository, IOwnerRepository ownerRepository, IServiceRepository serviceRepository, IRepositoryUser repositoryUser, IClientRepository clientRepository, IShopRepository shopRepository, IEmailService emailService)
        {
            _appointmentRepository = appointmentRepository;
            _ownerRepository = ownerRepository;
            _serviceRepository = serviceRepository;
            _clientRepository = clientRepository;
            _repositoryUser = repositoryUser;
            _shopRepository = shopRepository;
            _emailService = emailService;

        }

        public List<AppointmentDTO?> GetAllAppointment()
        {
            var listAppointment = _appointmentRepository.GetAll();

            if (listAppointment == null || !listAppointment.Any())
            {
                throw new NotFoundException($"No se encontro ningun {nameof(Appointment)}");
            }

            return AppointmentDTO.CreateList(listAppointment);
        }

        public AppointmentDTO? GetAppointmentById(int id)
        {
            var obj = GetAppointmentByIdOrThrow(id);
            return AppointmentDTO.Create(obj);
        }

        public void DeleteAppointment (int id) 
        {
            var obj = GetAppointmentByIdOrThrow(id);
            _appointmentRepository.Delete(obj);

            if(obj.ClientId != null && obj.DateAndHour > DateTime.Now) 
            {
                NotifyClientCancellation(obj.ClientId, obj.ShopId);
            }
        }

        public List<Appointment> GetAppointmentsBy(Func<int, IEnumerable<Appointment>> getAppointmentsFunc, int id)
        {
            var appointments = getAppointmentsFunc(id);

            if (appointments == null || !appointments.Any())
            {
                throw new NotFoundException($"No se encontró ningun {nameof(Appointment)}");
            }

            return appointments.ToList();
        }

        public List<EmployeeAppointmentListDTO> GetAvailableAppointmentsByEmployeeId(int employeeId)
        {
            var EmployeeAppointments = GetAppointmentsBy(_appointmentRepository.GetAvailableAppointmentsByEmployeeId, employeeId);
            List <EmployeeAppointmentListDTO> appointmentList = [];

            foreach (var a in EmployeeAppointments)
            {
                var shopName = _shopRepository.GetById(a.ShopId)?.Name ?? string.Empty;
                var serviceName = _serviceRepository.GetById(a.ServiceId)?.Name ?? string.Empty;
                var ClientName = _clientRepository.GetById(a.ClientId)?.Name ?? string.Empty;

                appointmentList.Add(EmployeeAppointmentListDTO.Create(a, serviceName, shopName, ClientName));
            }
            return appointmentList;
        }

        public List<ClientsAppointmentListDTO> GetAvailableAppointmentsByClientId(int clientId)
        {
            var clientAppointments = GetAppointmentsBy(_appointmentRepository.GetAvailableAppointmentsByClientId, clientId);
            List<ClientsAppointmentListDTO> appointmentList = [];

            foreach (var a in clientAppointments)
            {
                var shopName = _shopRepository.GetById(a.ShopId)?.Name ?? string.Empty;
                var serviceName = _serviceRepository.GetById(a.ServiceId)?.Name ?? string.Empty;

                appointmentList.Add(ClientsAppointmentListDTO.Create(a, serviceName, shopName));
            }
            return appointmentList;
        }

        public List<AllApointmentsOfMyShopRequestDTO?> GetAllApointmentsOfMyShop(int ownerId)
        {
            var owner = _ownerRepository.GetById(ownerId);
            if (owner == null)
            {
                throw new NotFoundException(nameof(Owner), ownerId);
            }

            List<AllApointmentsOfMyShopRequestDTO?> listDto = [];
            List<Appointment> myAppList = GetAppointmentsBy(_appointmentRepository.GetAllAppointmentsByShopId, owner.ShopId);

            foreach (var a in myAppList)
            {
                var provider = _repositoryUser.GetById(a.ProviderId)?.Name ?? string.Empty;
                var client = _clientRepository.GetById(a.ClientId)?.Name ?? string.Empty;
                var service = _serviceRepository.GetById(a.ServiceId)?.Name ?? string.Empty;

                listDto.Add(AllApointmentsOfMyShopRequestDTO.Create(a, provider, client, service));
            }

            return listDto;
        }

        public List<AllApointmentsOfMyShopRequestDTO?> GetAllAppointmentsByProviderId(int providerId)
        {
            List<AllApointmentsOfMyShopRequestDTO?> listDto = [];
            List<Appointment> myAppList = GetAppointmentsBy(_appointmentRepository.GetAllAppointmentsByProviderId, providerId);

            foreach (var a in myAppList)
            {
                var provider = _repositoryUser.GetById(a.ProviderId)?.Name ?? string.Empty;
                var client = _clientRepository.GetById(a.ClientId)?.Name ?? string.Empty;
                var service = _serviceRepository.GetById(a.ServiceId)?.Name ?? string.Empty;

                listDto.Add(AllApointmentsOfMyShopRequestDTO.Create(a, provider, client, service));
            }

            return listDto;
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
            var obj = GetAppointmentByIdOrThrow(id);

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
            var obj = GetAppointmentByIdOrThrow(request.IdAppointment);

            obj.ServiceId = request.ServiceId;
            obj.ClientId = request.ClientId;
            obj.Status = Status.Inactive;
            _appointmentRepository.Update(obj);

        }

        public AppointmentDTO GetLastAppointmentByShopId(int ownerId)
        {
            var owner = _ownerRepository.GetById(ownerId);
            if (owner == null)
            {
                throw new NotFoundException(nameof(Owner), ownerId);
            }

            var lastAppointment = _appointmentRepository.GetLastAppointmentByShopId(owner.ShopId);
            if (lastAppointment == null)
            {
                throw new NotFoundException("Appointment not found");
            }

            var lastAppDTO = AppointmentDTO.Create(lastAppointment);
            return lastAppDTO;
        }


        public Appointment GetAppointmentByIdOrThrow(int id)
        {
            var appointment = _appointmentRepository.GetById(id);
            if (appointment == null)
            {
                throw new NotFoundException(nameof(Appointment), id);
            }
            return appointment;
        }

        public void NotifyClientCancellation(int? clientId, int shopId)
        {
            var client = _repositoryUser.GetById(clientId);
            var shop = _shopRepository.GetById(shopId);
            _emailService.NotifyClientCancellation(client.Email, client.Name, shop.Name, shop.Phone);
        }
    }
}
