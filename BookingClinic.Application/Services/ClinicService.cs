using BookingClinic.Application.Common;
using BookingClinic.Application.Interfaces.Services;
using BookingClinic.Application.Interfaces.UnitOfWork;

namespace BookingClinic.Application.Services
{
    public class ClinicService : IClinicService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ClinicService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public ServiceResult<IEnumerable<string>> GetClinicNames()
        {
            try
            {
                var res = _unitOfWork.Clinics.GetAll().Select(c => c.Name).ToList();

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
