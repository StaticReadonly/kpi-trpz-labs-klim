using BookingClinic.Domain.Entities;

namespace BookingClinic.Domain.Interfaces.Repositories
{
    public interface IUserRepository : IRepository<UserBase, Guid>
    {
        IEnumerable<UserBase> GetVisitorAdmins();
        IEnumerable<UserBase> GetVisitorDoctors();
        IEnumerable<UserBase> GetVisitorPatients();
        IEnumerable<Doctor> GetSearchDoctors();
        UserBase? GetUserByEmail(string email);
        Doctor? GetDoctorByIdWithAppClinicSpeciality(Guid id);
        Doctor? GetDoctorById(Guid id);
    }
}
