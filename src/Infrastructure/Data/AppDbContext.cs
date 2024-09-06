using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

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
        }

        private object[] CreateOwnerDataSeed()
        {
            object[] result = new[]
            {
                new { Id = 1, Name = "John Doe", Email = "john@example.com", Password = "password123", Type = UserType.Owner, ShopId = 1, Status = Status.Active },
            };
                
            return result;
        }
        /*
        private object[] CreateEmployeeDataSeed()
        {
            object[] result = new[]
            {
                new { Id = 3, Name = "Jane Smith", Email = "jane@example.com", Password = "password123", Type = UserType.Employee, Status = Status.Active },
                new { Id = 4, Name = "Jake Smith", Email = "jake@example.com", Password = "password123", Type = UserType.Employee, Status = Status.Active }
            };

            return result;
        }

        private object[] CreateClientDataSeed()
        {
            object[] result = new[]
            {
                new { Id = 5, Name = "Bob Brown", Email = "bob@example.com", Password = "password123", Type = UserType.Client, Status = Status.Active },
            };

            return result;
        }

        private object[] CreateSysAdminDataSeed()
        {
            object[] result = new[]
            {
                new { Id = 6, Name = "Charlie Davis", Email = "charlie@example.com", Password = "password123", Type = UserType.SysAdmin, Status = Status.Active }
            };

            return result;
        }

        private Shop[] CreateShopDataSeed()
        {
            Shop[] result = new Shop[]
            {
                new Shop {Name = "HairSalon 1", Id = 1, Type = ShopType.HairSalon },
                new Shop {Name = "NailSalon 1", Id = 2, Type = ShopType.NailSalon }
            };
            return result;
        }

        private Service[] CreateServiceDataSeed()
        {
            Service[] result = new Service[]
            {
                new Service { Id = 1, Name = "Haircut", Description = "A basic haircut", Price = 20.00m, Duration = TimeSpan.FromMinutes(30), ServiceType = ServiceType.Haircut },
                new Service { Id = 2, Name = "Nail Polish", Description = "Nail polishing service", Price = 15.00m, Duration = TimeSpan.FromMinutes(45), ServiceType = ServiceType.Others }
            };
            return result;
        }

        private Schedule[] CreateScheduleDataSeed()
        {
            Schedule[] result = new Schedule[]
            {
                new Schedule { Id = 1, Day = DayOfWeek.Monday, StartTime = new TimeSpan(9, 0, 0), EndTime = new TimeSpan(17, 0, 0) },
                new Schedule { Id = 2, Day = DayOfWeek.Tuesday, StartTime = new TimeSpan(9, 0, 0), EndTime = new TimeSpan(17, 0, 0) },
                new Schedule { Id = 3, Day = DayOfWeek.Wednesday, StartTime = new TimeSpan(9, 0, 0), EndTime = new TimeSpan(17, 0, 0) },
                new Schedule { Id = 4, Day = DayOfWeek.Thursday, StartTime = new TimeSpan(9, 0, 0), EndTime = new TimeSpan(17, 0, 0) },
                new Schedule { Id = 5, Day = DayOfWeek.Friday, StartTime = new TimeSpan(9, 0, 0), EndTime = new TimeSpan(17, 0, 0) }
            };
            return result;
        }

        private object[] CreateAppointmentDataSeed()
        {
            object[] result = new[]
            {
                new { Id = 1, Status = Status.Active, ServiceId = 1, EmployeeId = 1, ClientId = 1, ShopId = 1, DateAndHour = new DateTime(2024, 9, 21, 10, 0, 0), Duration = new TimeSpan(1, 0, 0) },
                new { Id = 2, Status = Status.Active, ServiceId = 2, EmployeeId = 2, ClientId = 1, ShopId = 2, DateAndHour = new DateTime(2024, 9, 21, 10, 0, 0), Duration = new TimeSpan(1, 0, 0) },
                new { Id = 3, Status = Status.Active, ServiceId = 3, EmployeeId = 3, ClientId = 1, ShopId = 1, DateAndHour = new DateTime(2024, 9, 21, 10, 0, 0), Duration = new TimeSpan(1, 0, 0) }
            };
            return result;
        }*/

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
                ServiceType = ServiceType.Haircut,
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
                ServiceType = ServiceType.Others,
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
