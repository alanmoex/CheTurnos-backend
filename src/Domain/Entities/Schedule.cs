using Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Schedule
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public int ShopId { get; set; }
    public DayOfWeek Day { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }

    public Status Status { get; set; } = Status.Active;

    public Schedule() { }

    public Schedule(DayOfWeek day, TimeSpan startTime, TimeSpan endTime, int employee, int shop)
    {
        Day = day;
        StartTime = startTime;
        EndTime = endTime;
        EmployeeId = employee;
        ShopId = shop;
    }
}