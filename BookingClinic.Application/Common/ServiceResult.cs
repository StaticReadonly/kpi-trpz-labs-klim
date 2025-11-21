namespace BookingClinic.Application.Common
{
    public class ServiceResult
    {
        public IReadOnlyList<ServiceError> Errors { get; }
        public bool IsSuccess => Errors.Count == 0;

        protected ServiceResult(IReadOnlyList<ServiceError> errors) => Errors = errors ?? Array.Empty<ServiceError>();

        public static ServiceResult Success() => new(Array.Empty<ServiceError>());
        public static ServiceResult Failure(IEnumerable<ServiceError> errors) => new((errors ?? Array.Empty<ServiceError>()).ToArray());
        public static ServiceResult Failure(params ServiceError[] errors) => new((errors ?? Array.Empty<ServiceError>()).ToArray());
    }

    public sealed class ServiceResult<T> : ServiceResult where T : class
    {
        public T? Result { get; }

        private ServiceResult(T? result, IReadOnlyList<ServiceError> errors)
            : base(errors)
        {
            Result = result;
        }

        public static ServiceResult<T> Success(T? result) => new(result, Array.Empty<ServiceError>());
        public new static ServiceResult<T> Failure(IEnumerable<ServiceError> errors) => new(null, (errors ?? Array.Empty<ServiceError>()).ToArray());
        public new static ServiceResult<T> Failure(params ServiceError[] errors) => new(null, (errors ?? Array.Empty<ServiceError>()).ToArray());
    }
}
