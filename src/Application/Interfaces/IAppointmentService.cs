using Application.Models;
using Application.Models.Requests;
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
        void DeleteAppointment(int id);
        void CreateAppointment(AppointmentCreateRequest appointmentReques);
        AppointmentDTO UpdateAppointment(AppointmentUpdateRequest appointment, int id);
        void AssignClient(AssignClientRequestDTO request);
    }
}
