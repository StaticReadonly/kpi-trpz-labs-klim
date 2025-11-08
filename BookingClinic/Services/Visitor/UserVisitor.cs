using BookingClinic.Data.Entities;
using QuestPDF.Fluent;
using QuestPDF.Helpers;

namespace BookingClinic.Services.Visitor
{
    public class UserVisitor : IUserVisitor
    {
        private readonly string _fileName;
        private readonly List<string> _lines = new();
        private bool _disposed;

        public UserVisitor(string fileName)
        {
            _fileName = fileName;
        }

        public void VisitPatient(Patient patient)
        {
            if (patient == null) return;

            _lines.Add($"Patient: {patient.Name} {patient.Surname} | Email: {patient.Email} | Phone: {patient.Phone}");
            
            if (patient.ClientAppointments?.Any() == true)
            {
                _lines.Add($"  Total Appointments: {patient.ClientAppointments.Count}");
                _lines.Add($"  Finished: {patient.ClientAppointments.Count(a => a.IsFinished)}");
                _lines.Add($"  Canceled: {patient.ClientAppointments.Count(a => a.IsCanceled)}");
            }
        }

        public void VisitAdmin(Admin admin)
        {
            if (admin == null) return;
            _lines.Add($"Admin: {admin.Name} {admin.Surname}");
        }

        public void VisitDoctor(BookingClinic.Data.Entities.Doctor doctor)
        {
            if (doctor == null) return;

            var speciality = doctor.Speciality?.Name ?? "—";
            var clinic = doctor.Clinic?.Name ?? "—";


            _lines.Add($"Doctor: {doctor.Name} {doctor.Surname} | Speciality: {speciality} | Clinic: {clinic} | Email: {doctor.Email}");

            if (doctor.DoctorAppointments?.Any() == true)
            {
                _lines.Add($"  Total Appointments: {doctor.DoctorAppointments.Count}");
                _lines.Add($"  Finished: {doctor.DoctorAppointments.Count(a => a.IsFinished)}");
                _lines.Add($"  Canceled: {doctor.DoctorAppointments.Count(a => a.IsCanceled)}");
            }
        }

        private void CreatePdf()
        {
            var directory = Path.GetDirectoryName(_fileName);
            if (!string.IsNullOrWhiteSpace(directory) && !Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            if (!_lines.Any())
            {
                File.WriteAllText(_fileName, "Export produced no entries.");
                return;
            }

            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(25);
                    page.DefaultTextStyle(x => x.FontSize(12));
                    page.Content().Column(column =>
                    {
                        column.Item().Text($"Users export — {DateTime.UtcNow:yyyy-MM-dd HH:mm} UTC").FontSize(14).Bold();
                        column.Item().LineHorizontal(1).LineColor(Colors.Grey.Lighten2);

                        foreach (var line in _lines)
                        {
                            column.Item().PaddingTop(6).Text(line);
                        }
                    });
                });
            })
            .GeneratePdf(_fileName);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                CreatePdf();
            }

            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~UserVisitor()
        {
            Dispose(false);
        }
    }
}
