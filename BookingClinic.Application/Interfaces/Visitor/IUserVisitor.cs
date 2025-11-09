using BookingClinic.Application.Data.Visitor;

namespace BookingClinic.Application.Interfaces.Visitor
{
    public interface IUserVisitor : IDisposable
    {
        void VisitPatient(VisitablePatientModel patient);
        void VisitAdmin(VisitableAdminModel admin);
        void VisitDoctor(VisitableDoctorModel doctor);
    }
}
