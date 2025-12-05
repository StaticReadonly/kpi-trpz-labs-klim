using BookingClinic.Application.Common;

namespace BookingClinic.Application.Interfaces.Helpers
{
    public interface IViewMessageHelper
    {
        void SetErrors(IEnumerable<ServiceError> errors, IDictionary<string, object?> dict);

        List<ServiceError> GetErrors(IDictionary<string, object?> dict);

        void SetSuccess(string message, IDictionary<string, object?> dict);

        string? GetSuccess(IDictionary<string, object?> dict);
    }
}
