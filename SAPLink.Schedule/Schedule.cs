using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPLink.Schedule
{
    public class Schedule
    {
        [Key] 
        public int Id { get; set; }

        public List<TimeOnly> Times { get; set; } = new List<TimeOnly>();

        public string Hours { get; set; }

        public string GetHours()
        {
            var hours = "";

            foreach (var time in Times)
            {
                hours += $" - {time}";
            }

            return hours.TrimStart(' ').TrimStart('-');
        }
    }
}
