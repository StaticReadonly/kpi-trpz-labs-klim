using BookingClinic.Data.Entities;

namespace BookingClinic.Services.Visitor
{
    public interface IUserVisitor : IDisposable
    {
        void VisitPatient(Patient patient);
        void VisitAdmin(Admin admin);
        void VisitDoctor(BookingClinic.Data.Entities.Doctor doctor);
    }
}
