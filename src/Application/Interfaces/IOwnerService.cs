using Application.Models;
using Application.Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IOwnerService
    {
        void AddNewAppointments(int ownerId, string dateStart, string dateEnd);
        //void AddNewAppointmentsByOne();
        OwnerDTO? GetOwnerById(int ownerId);
        List<OwnerDTO?> GetAllOwners();
        OwnerDTO CreateNewOwner(OwnerCreateRequest ownerCreateRequest);
        void ModifyOwnerData(int id, OwnerUpdateRequest ownerUpdateRequest);
        void PermanentDeletionOwner(int id);
        void LogicalDeletionOwner(int id);
    }
}