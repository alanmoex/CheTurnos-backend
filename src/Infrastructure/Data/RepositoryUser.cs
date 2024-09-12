using Domain.Entities;
using Domain.Exceptions;
using Domain.Interface;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class RepositoryUser : RepositoryBase<User>, IRepositoryUser
    {
        private readonly AppDbContext _context;
        public RepositoryUser(AppDbContext context): base(context)
        {
            _context = context;
        }   
        
        public User GetByEmail(string email)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email)
                ?? throw new NotFoundException("User not found");
            return user;
        }

    }
}
