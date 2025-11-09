using BookingClinic.Domain.Entities;
using BookingClinic.Domain.Interfaces;

namespace BookingClinic.Domain.Services
{
    public class AppointmentDomainService : IAppointmentDomainService
    {
        public List<Tuple<string, IEnumerable<string>>> GetAppointments(Doctor doctor)
        {
            var res = new List<Tuple<string, IEnumerable<string>>>();
            var dateTimeNow = DateTime.UtcNow;

            for (int i = 1; i <= 10; i++)
            {
                var day = dateTimeNow.AddDays(i);

                if (day.DayOfWeek == DayOfWeek.Sunday || day.DayOfWeek == DayOfWeek.Saturday)
                {
                    continue;
                }

                var apps = doctor.DoctorAppointments.Where(a => a.DateTime.Date == day.Date);

                string dayString = $"{day.DayOfWeek}, {day:dd.MM.yyyy}";
                List<string> strings = new();

                int from = 9;
                int to = 16;

                for(int j = from; j <= to; j++)
                {
                    if (apps.FirstOrDefault(a => a.DateTime.Hour == j && a.DateTime.Minute == 0) == null)
                    {
                        strings.Add($"{j}:00-{j}:30");
                    }

                    if (apps.FirstOrDefault(a => a.DateTime.Hour == j && a.DateTime.Minute == 30) == null)
                    {
                        strings.Add($"{j}:30-{j + 1}:00");
                    }
                }

                res.Add(new(dayString, strings));
            }

            return res;
        }
    }
}
