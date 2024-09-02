using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interface
{
    public interface IRepositoryBase <T> where T : class
    {
        List<T> GetAll();
        T? GetById<TId>(TId id);
        T Add(T entity);
        void Delete(T entity);
        void Update(T entity);
    }
}
