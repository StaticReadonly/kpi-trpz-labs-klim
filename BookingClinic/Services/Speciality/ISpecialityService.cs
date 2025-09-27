namespace BookingClinic.Services.Speciality
{
    public interface ISpecialityService
    {
        ServiceResult<IEnumerable<string>> GetSpecialityNames();
    }
}
