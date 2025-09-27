namespace BookingClinic.Services
{
    public class ServiceResult<T> where T : class
    {
        public T? Result { get; set; }
        public bool IsSuccess { get; set; }
        public ICollection<ServiceError> Errors { get; set; }

        protected ServiceResult(T? result, bool isSuccess, ICollection<ServiceError> erorrs)
        {
            Result = result;
            IsSuccess = isSuccess;
            Errors = erorrs;
        }

        public static ServiceResult<T> Success(T? result) => 
            new ServiceResult<T>(result, true, Array.Empty<ServiceError>());

        public static ServiceResult<T> Failure(ICollection<ServiceError> errors) =>
            new ServiceResult<T>(null, false, errors);
    }
}
