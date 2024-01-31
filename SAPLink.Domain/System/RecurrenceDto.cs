namespace SAPLink.Domain.System;

public class RecurrenceHourlyDto
{
    public Documents Document { get; set; }
    public int Interval { get; set; }
}
public class RecurrenceDailyDto
{
    public Documents Document { get; set; }
    public DayOfWeek DayOfWeek { get; set; }

}

public class RecurrenceDto
{
    public Documents Document { get; set; }
    public Repeats Recurring { get; set; }
    public int Interval { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
}