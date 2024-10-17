﻿using Application.Models;
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
        void DeleteAppointment(int id);
        void CreateAppointment(int shopId, int providerId, DateTime dateAndHour, int? serviceId = null, int? clientId = null);
        AppointmentDTO UpdateAppointment(AppointmentUpdateRequest appointment, int id);
        List<Appointment?> GetLastAppointmentByShopId(int ownerId);
        List<AppointmentDTO?> GetAllApointmentsOfMyShop(int ownerId);
        void AssignClient(AssignClientRequestDTO request);
    }
}
