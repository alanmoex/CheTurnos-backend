using Application.Models;
using Application.Models.Requests;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IAppointmentService
    {
        List<AppointmentDTO?> GetAllAppointment();
        AppointmentDTO? GetAppointmentById(int id);
        List<AppointmentDTO?> GetAvailableAppointmentsByEmployeeId(int employeeId);
        List<AppointmentDTO> GetAvailableAppointmentsByClienId(int clientId);
        void DeleteAppointment(int id);
        void CreateAppointment(int shopId, int providerId, DateTime dateAndHour, int? serviceId = null, int? clientId = null);
        AppointmentDTO UpdateAppointment(AppointmentUpdateRequest appointment, int id);
        AppointmentDTO GetLastAppointmentByShopId(int ownerId);
        void AssignClient(AssignClientRequestDTO request);
        List<AllApointmentsOfMyShopRequestDTO?> GetAllApointmentsOfMyShop(int ownerId);

        List<AllApointmentsOfMyShopRequestDTO> GetAllAppointmentsByProviderId(int providerId);
    }
}
