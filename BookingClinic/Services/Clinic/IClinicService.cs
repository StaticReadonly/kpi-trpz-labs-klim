namespace BookingClinic.Services.Clinic
{
    public interface IClinicService
    {
        ServiceResult<IEnumerable<string>> GetClinicNames(); 
    }
}
