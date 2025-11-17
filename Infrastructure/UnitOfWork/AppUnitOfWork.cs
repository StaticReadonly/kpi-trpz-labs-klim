using BookingClinic.Application.Interfaces.Repositories;
using BookingClinic.Application.Interfaces.UnitOfWork;
using BookingClinic.Infrastructure.AppContext;

namespace BookingClinic.Infrastructure.UnitOfWork
{
    public class AppUnitOfWork : IUnitOfWork
    {
        private readonly ApplicationContext _context;

        public IUserRepository Users { get; init; }

        public IClinicRepository Clinics { get; init; }

        public ISpecialityRepository Specialities { get; init; }

        public IAppointmentRepository Appointments { get; init; }

        public IDoctorReviewRepository DoctorReviews { get; init; }

        public AppUnitOfWork(
            ApplicationContext context,
            IUserRepository userRepository,
            IClinicRepository clinicRepository,
            ISpecialityRepository specialityRepository,
            IAppointmentRepository appointmentRepository,
            IDoctorReviewRepository doctorReviewRepository
            )
        {
            this._context = context;
            this.Users = userRepository;
            this.Clinics = clinicRepository;
            this.Specialities = specialityRepository;
            this.Appointments = appointmentRepository;
            this.DoctorReviews = doctorReviewRepository;
        }

        public async Task<IDbTransaction> BeginTransactionAsync()
        {
            return new DbTransaction(await _context.Database.BeginTransactionAsync());
        }

        public Task<int> SaveChangesAsync() => _context.SaveChangesAsync();
    }
}
