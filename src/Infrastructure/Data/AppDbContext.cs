using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Shop> Shops { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Schedule> Schedules { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(CreateOwnerDataSeed());
            modelBuilder.Entity<User>().HasData(CreateUserDataSeed());
            modelBuilder.Entity<Shop>().HasData(CreateShopDataSeed());
            modelBuilder.Entity<Service>().HasData(CreateServiceDataSeed());
            modelBuilder.Entity<Schedule>().HasData(CreateScheduleDataSeed());
            modelBuilder.Entity<Schedule>().HasData(CreateScheduleDataSeed2());
            modelBuilder.Entity<Appointment>().HasData(CreateAppointmentDataSeed());

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Appointment>()
                .Property(u => u.Status)
                .HasConversion(new EnumToStringConverter<Status>());

            modelBuilder.Entity<Service>()
                .Property(s => s.Type)
                .HasConversion(new EnumToStringConverter<ServiceType>());

            modelBuilder.Entity<Service>()
                .Property(s => s.Status)
                .HasConversion(new EnumToStringConverter<Status>());

            modelBuilder.Entity<Shop>()
                .Property(s => s.Type)
                .HasConversion(new EnumToStringConverter<ShopType>());

            modelBuilder.Entity<Shop>()
                .Property(s => s.Status)
                .HasConversion(new EnumToStringConverter<Status>());

            modelBuilder.Entity<User>()
                .Property(u => u.Type)
                .HasConversion(new EnumToStringConverter<UserType>());

            modelBuilder.Entity<User>()
                .Property(s => s.Status)
                .HasConversion(new EnumToStringConverter<Status>());
        }

        private object[] CreateOwnerDataSeed()
        {
            object[] result = new[]
            {
                new { Id = 1, Name = "John Doe", Email = "john@example.com", Password = "password123", Type = UserType.Owner, ShopId = 1, Status = Status.Active },
            };
                
            return result;
        }

        private static User[] CreateUserDataSeed()
        {
            return new User[]
            {          
            new User
            {
                Id = 2,
                Name = "Employee User",
                Email = "employee@example.com",
                Password = "password123",
                Type = UserType.Employee,
                Status = Status.Active
            },
            new User
            {
                Id = 3,
                Name = "Client User",
                Email = "client@example.com",
                Password = "password123",
                Type = UserType.Client,
                Status = Status.Active
            },
            new User
            {
                Id = 4,
                Name = "SysAdmin User",
                Email = "sysadmin@example.com",
                Password = "password123",
                Type = UserType.SysAdmin,
                Status = Status.Active
            }
            };
    }

        private static Shop[] CreateShopDataSeed()
        {
            return new Shop[]
            {
            new Shop
            {
                Id = 1,
                Name = "Beauty Salon",
                Type = ShopType.BeautyShop,
                Status = Status.Active
            }
            };
        }

        private static object[] CreateServiceDataSeed()
        {
            return new []
            {
            new
            {
                Id = 1,
                Name = "Haircut",
                Description = "A standard haircut service.",
                Price = 20.00m,
                Duration = new TimeSpan(0, 30, 0),
                Type = ServiceType.Haircut,
                Status = Status.Active,
                UserId = 2
            },
            new 
            {
                Id = 2,
                Name = "Hair Color",
                Description = "Full hair coloring service.",
                Price = 60.00m,
                Duration = new TimeSpan(1, 0, 0),
                Type = ServiceType.Others,
                Status = Status.Active,
                UserId = 2
            }
            };
        }

        private static object[] CreateAppointmentDataSeed()
        {
            return new []
            {
            new 
            {
                Id = 1,
                Status = Status.Active,
                ServiceId = 1,
                EmployeeId = 2,
                ClientId = 3,
                ShopId = 1,
                DateAndHour = DateTime.Now.AddDays(1),
                Duration = new TimeSpan(0, 30, 0)
            }
            };
        }

        private static object[] CreateScheduleDataSeed()
        {
            return new[]
            {
                new
                {
                    Id = 1,
                    Day = DayOfWeek.Monday,
                    StartTime = new TimeSpan(9, 0, 0),
                    EndTime = new TimeSpan(17, 0, 0),                 
                    ShopId = 1
                },
                new
                {
                    Id = 2,
                    Day = DayOfWeek.Tuesday,
                    StartTime = new TimeSpan(9, 0, 0),
                    EndTime = new TimeSpan(17, 0, 0),
                    ShopId = 1
                },
                new
                {
                    Id = 3,
                    Day = DayOfWeek.Wednesday,
                    StartTime = new TimeSpan(9, 0, 0),
                    EndTime = new TimeSpan(17, 0, 0),
                    ShopId = 1
                },
                new
                {
                    Id = 4,
                    Day = DayOfWeek.Thursday,
                    StartTime = new TimeSpan(9, 0, 0),
                    EndTime = new TimeSpan(17, 0, 0),
                    ShopId = 1
                },
                new
                {
                    Id = 5,
                    Day = DayOfWeek.Friday,
                    StartTime = new TimeSpan(9, 0, 0),
                    EndTime = new TimeSpan(17, 0, 0),
                    ShopId = 1
                }
            };
        }

        private static object[] CreateScheduleDataSeed2()
        {
            return new[]
            {
                new
                {
                    Id = 6,
                    Day = DayOfWeek.Monday,
                    StartTime = new TimeSpan(9, 0, 0),
                    EndTime = new TimeSpan(17, 0, 0),
                    EmployeeId = 2,
                },
                new
                {
                    Id = 7,
                    Day = DayOfWeek.Tuesday,
                    StartTime = new TimeSpan(9, 0, 0),
                    EndTime = new TimeSpan(17, 0, 0),
                    EmployeeId = 2
                },
                new
                {
                    Id = 8,
                    Day = DayOfWeek.Wednesday,
                    StartTime = new TimeSpan(9, 0, 0),
                    EndTime = new TimeSpan(17, 0, 0),
                    EmployeeId = 2
                },
                new
                {
                    Id = 9,
                    Day = DayOfWeek.Thursday,
                    StartTime = new TimeSpan(9, 0, 0),
                    EndTime = new TimeSpan(17, 0, 0),
                    EmployeeId = 2
                },
                new
                {
                    Id = 10,
                    Day = DayOfWeek.Friday,
                    StartTime = new TimeSpan(9, 0, 0),
                    EndTime = new TimeSpan(17, 0, 0),
                    EmployeeId = 2
                }
            };
        }
    }
}
