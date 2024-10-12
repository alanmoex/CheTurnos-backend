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
        public AppointmentService (IAppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
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

        public List<AppointmentDTO> GetAvailableAppointmentsByEmployeeId(int employeeId)
        {
            var employeeAppointments = _appointmentRepository.GetAvailableAppointmentsByEmployeeId(employeeId)
                ?? throw new NotFoundException("Appointment not found"); 
            List<AppointmentDTO> appointmentList = new List<AppointmentDTO>();

            if(employeeAppointments != null)
            {
                foreach (var appointment in employeeAppointments)
                {
                    var appointmentToAdd = AppointmentDTO.Create(appointment);
                    appointmentList.Add(appointmentToAdd);
                }
                return appointmentList;
            }
            return [];
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

        public AppointmentDTO? GetLastAppointmentByShopId(int shopId)
        {
            var lastAppointment = _appointmentRepository.GetLastAppointmentByShopId(shopId);
            if (lastAppointment == null)
            {
                throw new NotFoundException("No existen turnos almacenados");
            }
            return AppointmentDTO.Create(lastAppointment);
        }
    }
}
