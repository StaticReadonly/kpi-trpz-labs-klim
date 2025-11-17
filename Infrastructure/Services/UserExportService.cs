using BookingClinic.Application.Data.Visitor;
using BookingClinic.Application.Interfaces.UnitOfWork;
using BookingClinic.Application.Interfaces.Visitor;
using Mapster;

namespace BookingClinic.Infrastructure.Services
{
    public class UserExportService : IUserExportService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IVisitorFactory _visitorFactory;
        

        public UserExportService(
            IUnitOfWork unitOfWork,
            IVisitorFactory visitorFactory)
        {
            this._unitOfWork = unitOfWork;
            this._visitorFactory = visitorFactory;
        }

        public void ExportUsersToPdf()
        {
            string path = Path.Combine(Environment.GetFolderPath(
                Environment.SpecialFolder.MyDocuments), 
                "BookingClinicReports", 
                $"rep{DateTime.UtcNow:dd-MM-yyyy_HH-mm-ss}.pdf");


            using IUserVisitor visitor = _visitorFactory.CreatePDFVisitor(path);
            var patients = _unitOfWork.Users.GetVisitorPatients().Adapt<IEnumerable<VisitablePatientModel>>();
            var doctors = _unitOfWork.Users.GetVisitorDoctors().Adapt<IEnumerable<VisitableDoctorModel>>();
            var admins = _unitOfWork.Users.GetVisitorAdmins().Adapt<IEnumerable<VisitableAdminModel>>();
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
