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
    }
}
