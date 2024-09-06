using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Schedule
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public User? Employee { get; set; }
    public Shop? Shop { get; set; }
    public DayOfWeek Day { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }

    public Schedule() { }

    public Schedule(DayOfWeek day, TimeSpan startTime, TimeSpan endTime, User? employee, Shop? shop)
    {
        Day = day;
        StartTime = startTime;
        EndTime = endTime;
        Employee = employee;
        Shop = shop;
    }
}