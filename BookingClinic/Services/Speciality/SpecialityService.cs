using BookingClinic.Data.Repositories.SpecialityRepository;

namespace BookingClinic.Services.Speciality
{
    public class SpecialityService : ISpecialityService
    {
        private readonly ISpecialityRepository _specialityRepository;

        public SpecialityService(ISpecialityRepository specialityRepository)
        {
            _specialityRepository = specialityRepository;
        }
        public ServiceResult<IEnumerable<string>> GetSpecialityNames()
        {
            try
            {
                var res = _specialityRepository.GetAll().Select(c => c.Name).ToList();

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
