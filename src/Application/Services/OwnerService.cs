using Application.Interfaces;
using Application.Models;
using Application.Models.Requests;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Interface;
using Domain.Interfaces;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Application.Services
{
    public class OwnerService : IOwnerService
    {
        private readonly IShopService _shopService;
        private readonly IEmployeeService _employeeService;
        private readonly IAppointmentService _appointmentService;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IOwnerRepository _ownerRepository;
        private readonly IRepositoryUser _userRepository;
        private readonly IEmailService _emailService;

        public OwnerService(IShopService shopService, IEmployeeService employeeService, IAppointmentService appointmentService, IAppointmentRepository appointmentRepository, IOwnerRepository ownerRepository, IRepositoryUser userRepository, IEmailService emailService)
        {
            _shopService = shopService;
            _employeeService = employeeService;
            _appointmentService = appointmentService;
            _appointmentRepository = appointmentRepository;
            _ownerRepository = ownerRepository;
            _userRepository = userRepository;
            _emailService = emailService;
        }

        public void AddNewAppointments(int ownerId, string dateStart, string dateEnd)
        {
            //validamos que los string suministrados se puedan convertir a fecha
            if (!ValidateStringsToDate(dateStart, dateEnd))
            {
                throw new Exception("El formato de fecha ingresado es incorrecto");
            }

            //nos traemos el owner en cuestión para saber de qué negocio es dueño mediante su atributo shopId
            var currentOwner = _ownerRepository.GetById(ownerId);

            //ahora nos traemos el shop para poder utilizar sus datos
            var myShop = _shopService.GetById(currentOwner.ShopId);

            //nos traemos el último turno existente que existe (si lo hay) para este negocio (el del mayor id y fecha)
            var lastAppointment = _appointmentRepository.GetLastAppointmentByShopId(myShop.Id);

            //validamos que la fecha de inicio indicada sea correcta y las fechas de los turnos no se pisen
            if (lastAppointment != null)
            {
                if (!ValidateDateStart(DateTime.Parse(dateStart), DateTime.Parse(dateEnd), lastAppointment.DateAndHour))
                {
                    throw new Exception("La fecha de inicio indicada no es correcta");
                }
            }

            //armamos una lista de num enteros a la que le agregamos el id del dueño. Nos servirá luego
            List<int> shopWorkersId = new List<int>() { currentOwner.Id };

            //traemos los empleados del negocio en cuestión (si los hay)
            var employeesList = _employeeService.GetAllByShopId(myShop.Id);

            //validamos que el negocio cuento con empleados registrados
            if (employeesList.Count > 0)
            {
                //si existen empleados agregamos sus id a la lista de trabajadores
                foreach (var emp in employeesList)
                {
                    shopWorkersId.Add(emp.Id);
                }
            }

            //creamos una lista con todos los horarios del día que deberían tener los turnos a crear, de acuerdo con el horario
            //de trabajo del negocio
            List<TimeOnly> newTimeSchema = CreateTimeSchema(myShop.TimeStart, myShop.TimeEnd, myShop.AppoimentFrecuence);

            //creamos una lista con todas las fechas comprendidas en el rango de fechas que especificó el usuario
            List<DateOnly> newDateSchema = CreateDateSchema(DateTime.Parse(dateStart), DateTime.Parse(dateEnd));

            //creamos una lista de strings con los nombres de los días de la semana en los que trabaja el negocio
            List<string> workDayNames = DaysEnumToString(myShop.WorkDays);

            //ideamos la siguiente lógica para generar la plantilla de turnos según el período especificado por el usuario
            foreach (var id in shopWorkersId)
            {
                foreach (var day in newDateSchema)
                {
                    if (workDayNames.Contains(day.DayOfWeek.ToString()))
                    {
                        foreach (var time in newTimeSchema)
                        {
                            _appointmentService.CreateAppointment(myShop.Id, id, new DateTime(day, time));
                        }
                    }
                }
            }
        }

        public List<OwnerDTO?> GetAllOwners()
        {
            var ownersList = _ownerRepository.GetAll();

            return OwnerDTO.CreateList(ownersList);
        }

        public OwnerDTO? GetOwnerById(int id)
        {
            var owner = _ownerRepository.GetById(id);

            if (owner == null)
                throw new NotFoundException(nameof(Owner), id);

            return OwnerDTO.Create(owner);
        }

        public OwnerDTO CreateNewOwner(OwnerCreateRequest ownerCreateRequest)
        {
            bool validationFlag = ValidatePassword(ownerCreateRequest.Password);
            if (validationFlag)
            {
                validationFlag = ValidateName(ownerCreateRequest.Name);
                if (validationFlag)
                {
                    validationFlag = ValidateEmail(ownerCreateRequest.Email);
                    if (validationFlag)
                    {
                        var existentUser = _userRepository.GetByEmail(ownerCreateRequest.Email);

                        if (existentUser != null)
                        {
                            throw new Exception("El email que intenta utilizar ya existe");
                        }
                    }
                }
            }

            if (validationFlag)
            {
                var newOwner = new Owner();

                newOwner.Name = ownerCreateRequest.Name;
                newOwner.Email = ownerCreateRequest.Email;
                newOwner.Password = ownerCreateRequest.Password;
                newOwner.Type = UserType.Owner;
                newOwner.ShopId = ownerCreateRequest.ShopId;
                //Nuevos atributos de Usuario.
                newOwner.ImgUrl = "";
                newOwner.PasswordResetCode = Guid.NewGuid().ToString().Substring(0, 6);
                _ownerRepository.Add(newOwner);
                _emailService.AccountCreationConfirmationEmail(newOwner.Email, newOwner.Name);
                return OwnerDTO.Create(newOwner);
            }
            else
            {
                throw new ValidationException("Los datos ingresados no son válidos");
            }
        }

        public void ModifyOwnerData(int id, OwnerUpdateRequest request)
        {
            var owner = _ownerRepository.GetById(id) ?? throw new NotFoundException("User not Found");

            if (owner.Password != request.ConfirmationPassword) throw new Exception("Passwords do not match");

            if (!string.IsNullOrEmpty(request.Name.Trim())) owner.Name = request.Name;

            if (string.IsNullOrEmpty(request.NewPassword.Trim())) throw new Exception("Empty NewPassword.");
            if (!ValidatePassword(request.NewPassword)) throw new Exception("NewPassword is not validate");
            owner.Password = request.NewPassword;

            _ownerRepository.Update(owner);
        }

        public void PermanentDeletionOwner(int id)
        {
            var owner = _ownerRepository.GetById(id);

            if (owner == null)
                throw new NotFoundException(nameof(Owner), id);

            _ownerRepository.Delete(owner);
        }

        public void LogicalDeletionOwner(int id)
        {
            var owner = _ownerRepository.GetById(id);

            if (owner == null)
                throw new NotFoundException(nameof(Owner), id);

            if (owner.Status == Status.Inactive)
            {
                throw new Exception("El dueño especificado ya se encuentra inactivo");
            }

            owner.Status = Status.Inactive;
            _ownerRepository.Update(owner);
        }

        //------------------
        private int CalculateAppointmentAmount(TimeOnly timeStart, TimeOnly timeEnd, int appointmentFrecuence)
        {
            TimeSpan timeDiference = timeEnd - timeStart;
            double appointmentAmount = timeDiference.TotalMinutes / appointmentFrecuence;
            return (int)appointmentAmount;
        }
        private List<TimeOnly> CreateTimeSchema(TimeOnly timeStart, TimeOnly timeEnd, int appointmentFrecuence)
        {
            //calculamos la cantidad de turnos que entran en una jornada de trabajo de un día
            int appointmentAmount = CalculateAppointmentAmount(timeStart, timeEnd, appointmentFrecuence);

            List<TimeOnly> newSchedule = new List<TimeOnly>();

            TimeSpan scheduleItem = timeStart.ToTimeSpan();
            TimeSpan timeInterval = new TimeSpan(0, appointmentFrecuence, 0);

            for (int i = 0; i < appointmentAmount; i++)
            {
                TimeOnly auxTime = new TimeOnly(scheduleItem.Hours, scheduleItem.Minutes);
                newSchedule.Add(auxTime);
                scheduleItem = scheduleItem + timeInterval;
            }

            return newSchedule;
        }
        private bool ValidateDateStart(DateTime dateStart, DateTime dateEnd, DateTime lastAppointmentDate)
        {
            if (dateStart < DateTime.Today || dateStart <= lastAppointmentDate || dateStart > dateEnd)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        private List<DateOnly> CreateDateSchema(DateTime dateStart, DateTime dateEnd)
        {
            List<DateOnly> dateOnlyList = new List<DateOnly>();

            while (dateStart <= dateEnd)
            {
                dateOnlyList.Add(new DateOnly(dateStart.Year, dateStart.Month, dateStart.Day));
                dateStart = dateStart.AddDays(1);
            }

            return dateOnlyList;
        }
        private List<string> DaysEnumToString(List<Days> workDays)
        {
            List<string> dayNames = new List<string>();
            workDays.ForEach(d =>
            {
                string name = d.ToString();
                dayNames.Add(name);
            });
            return dayNames;
        }
        private bool ValidateStringsToDate(string dateStart, string dateEnd)
        {
            try
            {
                DateTime.Parse(dateStart);
                DateTime.Parse(dateEnd);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        //------------------
        private bool ValidatePassword(string password)
        {
            //comprobamos si la contraseña es nula o tiene menos de 8 caracteres
            if (string.IsNullOrEmpty(password) || password.Length < 8)
            {
                return false;
            }

            /*con esta expresión regular verificaremos que la contraseña contenga al menos una letra y un número*/
            string pattern = @"^(?=.*[a-zA-Z])(?=.*\d).+$";
            //la siguiente función devolverá true si hay match, y false en caso contrario
            return Regex.IsMatch(password, pattern);
        }
        private bool ValidateName(string name)
        {
            if (string.IsNullOrEmpty(name.Trim()))
            {
                return false;
            }
            return true;
        }
        private bool ValidateEmail(string email)
        {
            if (string.IsNullOrEmpty(email.Trim()))
            {
                return false;
            }
            else
            {
                try
                {
                    MailAddress mail = new MailAddress(email);
                    return true;
                }
                catch (FormatException)
                {
                    return false;
                }
            }
        }
    }
}


