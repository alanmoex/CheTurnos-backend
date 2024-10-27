using Application.Models;
using Application.Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ISysAdminService
    {
        List<SysAdminDTO?> GetAll();
        SysAdminDTO? GetById(int id);
        void Create(SysAdminCreateRequestDTO request);
        void Update(int id, SysAdminUpdateDTO request);
        void Delete(int id);
        void LogicalDelete(int id);
    }
}
