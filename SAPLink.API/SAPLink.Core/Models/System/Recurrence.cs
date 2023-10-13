namespace SAPLink.Core.Models.System;

public class Recurrence
{
    public int Id { get; set; }
    public Documents Document { get; set; }
    public Repeats Recurring { get; set; }
    public int Interval { get; set; }

    public DayOfWeek DayOfWeek { get; set; }
}