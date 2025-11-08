using BookingClinic.Data.Repositories.UserRepository;

namespace BookingClinic.Services.Visitor
{
    public class UserExportService
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
            var patients = _userRepository.GetVisitorPatients();
            var doctors = _userRepository.GetVisitorDoctors();
            var admins = _userRepository.GetVisitorAdmins();
            var users = patients.Concat(doctors).Concat(admins);

            foreach (var item in users)
            {
                item.AcceptVisitor(visitor);
            }
        }
    }
}
