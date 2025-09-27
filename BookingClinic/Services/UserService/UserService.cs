using BookingClinic.Data.Repositories.ClinicRepository;
using BookingClinic.Data.Repositories.SpecialityRepository;
using BookingClinic.Data.Repositories.UserRepository;
using BookingClinic.Services.Data.Doctor;
using Mapster;

namespace BookingClinic.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ISpecialityRepository _specialityRepository;
        private readonly IClinicRepository _clinicRepository;

        public UserService(
            IUserRepository userRepository,
            ISpecialityRepository specialityRepository,
            IClinicRepository clinicRepository)
        {
            _userRepository = userRepository;
            _specialityRepository = specialityRepository;
            _clinicRepository = clinicRepository;
        }

        public ServiceResult<IEnumerable<SearchDoctorResDto>> SearchDoctors(SearchDoctorDto dto)
        {
            try
            {
                var doctors = _userRepository.GetSearchDoctors();

                if (!string.IsNullOrEmpty(dto.Speciality))
                {
                    var speciality = _specialityRepository.GetSpecialityByName(dto.Speciality);

                    doctors = doctors.Where(d => d.SpecialityId == speciality.Id);
                }

                if (!string.IsNullOrEmpty(dto.Clinic))
                {
                    var clinic = _clinicRepository.GetClinicByName(dto.Clinic);

                    doctors = doctors.Where(d => d.ClinicId == clinic.Id);
                }

                if (!string.IsNullOrEmpty(dto.Query))
                {
                    var nameSurname = dto.Query.Trim().Split(' ').Take(2).Select(s => s.ToLower()).ToArray();

                    if (nameSurname.Count() == 2)
                    {
                        doctors = doctors.Where(d =>
                        {
                            var name = d.Name.ToLower();
                            var surname = d.Surname.ToLower();

                            return name.Contains(nameSurname[0]) || name.Contains(nameSurname[1]) ||
                            surname.Contains(nameSurname[0]) || surname.Contains(nameSurname[1]);
                        });
                    }
                    else
                    {
                        doctors = doctors.Where(d =>
                        {
                            var name = d.Name.ToLower();
                            var surname = d.Surname.ToLower();

                            return name.Contains(nameSurname[0]) || surname.Contains(nameSurname[0]);
                        });
                    }
                }

                var res = doctors.ToList().Adapt<IEnumerable<SearchDoctorResDto>>();

                return ServiceResult<IEnumerable<SearchDoctorResDto>>.Success(res);
            }
            catch(Exception)
            {
                return ServiceResult<IEnumerable<SearchDoctorResDto>>.Failure(
                    new List<ServiceError>() { ServiceError.UnexpectedError() });
            }
        }
    }
}
