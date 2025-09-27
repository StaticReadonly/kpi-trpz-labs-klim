using BookingClinic.Data.Repositories.ClinicRepository;

namespace BookingClinic.Services.Clinic
{
    public class ClinicService : IClinicService
    {
        private readonly IClinicRepository _clinicRepository;

        public ClinicService(IClinicRepository clinicRepository)
        {
            _clinicRepository = clinicRepository;
        }

        public ServiceResult<IEnumerable<string>> GetClinicNames()
        {
            try
            {
                var res = _clinicRepository.GetAll().Select(c => c.Name).ToList();

                return ServiceResult<IEnumerable<string>>.Success(res);
            }
            catch (Exception)
            {
                return ServiceResult<IEnumerable<string>>.Failure(
                    new List<ServiceError>() { ServiceError.UnexpectedError() });
            }
        }
    }
}
