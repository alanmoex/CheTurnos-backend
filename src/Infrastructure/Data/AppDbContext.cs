using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class AppDbContext: DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Shop> Shops { get; set; }
        public DbSet<Service> Services {  get; set; } 
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<ShopsReview> ShopsReviews { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Realciones
            modelBuilder.Entity<Shop>()
                .HasOne(s => s.Owner)
                .WithMany()
                .HasForeignKey(s => s.OwnerId);

            //Semilla de datos??? 
            //modelBuilder.Entity<User>().HasData(CreateUserSeedData());
        }


        //private User[] CreateUserSeedData()
        //{
        //    return new User[]
        //    {
        //    new User { Id = 1, Name = "Valen", Password = "Pass1", Email = "Va@example.com", Type = UserType.Owner },
        //    new User { Id = 2, Name = "Manu", Password = "Pass2", Email = "Manu@example.com", Type = UserType.Staff },
        //    new User { Id = 3, Name = "Alan", Password = "Pass3", Email = "Alan.Mo@example.com", Type = UserType.Client},
        //    new User { Id = 4, Name = "Fabri", Password = "Pass4", Email = "Fab@example.com", Type = UserType.SisAdmin},
        //    };
        }

    }
}
