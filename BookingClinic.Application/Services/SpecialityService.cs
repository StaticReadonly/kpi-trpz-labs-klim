using BookingClinic.Application.Common;
using BookingClinic.Application.Interfaces.Services;
using BookingClinic.Application.Interfaces.UnitOfWork;

namespace BookingClinic.Application.Services
{
    public class SpecialityService : ISpecialityService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SpecialityService(
            IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        public ServiceResult<IEnumerable<string>> GetSpecialityNames()
        {
            try
            {
                var res = _unitOfWork.Specialities.GetAll().Select(c => c.Name).ToList();

                return ServiceResult<IEnumerable<string>>.Success(res);
            }
            catch (Exception)
            {
                return ServiceResult<IEnumerable<string>>.Failure(ServiceError.UnexpectedError());
            }
        }
    }
}
