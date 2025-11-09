using BookingClinic.Application.Data.Visitor;
using BookingClinic.Application.Interfaces.Repositories;
using BookingClinic.Application.Interfaces.Visitor;
using Mapster;

namespace BookingClinic.Infrastructure.Services
{
    public class UserExportService : IUserExportService
    {
        private readonly IUserRepository _userRepository;
        private readonly IVisitorFactory _visitorFactory;
        

        public UserExportService(
            IUserRepository userRepository,
            IVisitorFactory visitorFactory)
        {
            this._userRepository = userRepository;
            this._visitorFactory = visitorFactory;
        }

        public void ExportUsersToPdf()
        {
            string path = Path.Combine(Environment.GetFolderPath(
                Environment.SpecialFolder.MyDocuments), 
                "BookingClinicReports", 
                $"rep{DateTime.UtcNow:dd-MM-yyyy_HH-mm-ss}.pdf");


            using IUserVisitor visitor = _visitorFactory.CreatePDFVisitor(path);
            var patients = _userRepository.GetVisitorPatients().Adapt<IEnumerable<VisitablePatientModel>>();
            var doctors = _userRepository.GetVisitorDoctors().Adapt<IEnumerable<VisitableDoctorModel>>();
            var admins = _userRepository.GetVisitorAdmins().Adapt<IEnumerable<VisitableAdminModel>>();
            var users = new List<VisitableModelBase>();

            users.AddRange(admins);
            users.AddRange(doctors);
            users.AddRange(patients);

            foreach (var item in users)
            {
                item.AcceptVisitor(visitor);
            }
        }
    }
}
