using BookingClinic.Domain.Interfaces.Repositories;

namespace BookingClinic.Application.Interfaces.UnitOfWork
{
    public interface IUnitOfWork
    {
        IUserRepository Users { get; }
        IClinicRepository Clinics { get; }
        ISpecialityRepository Specialities { get; }
        IAppointmentRepository Appointments { get; }
        IDoctorReviewRepository DoctorReviews { get; }

        Task<int> SaveChangesAsync();
        Task<IDbTransaction> BeginTransactionAsync();
    }
}
