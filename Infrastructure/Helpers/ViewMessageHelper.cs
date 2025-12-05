using BookingClinic.Application.Common;
using BookingClinic.Application.Interfaces.Helpers;
using System.Text.Json;

namespace BookingClinic.Infrastructure.Helpers
{
    public class ViewMessageHelper : IViewMessageHelper
    {
        public const string ErrorsKey = "Errors";
        public const string SuccessKey = "Success";

        public List<ServiceError> GetErrors(IDictionary<string, object?> dict)
        {
            List<ServiceError> errors = new List<ServiceError>();

            if (dict[ErrorsKey] != null)
            {
                errors = JsonSerializer.Deserialize<List<ServiceError>>(dict[ErrorsKey]!.ToString()!) ?? new List<ServiceError>();
            }

            return errors;
        }

        public string? GetSuccess(IDictionary<string, object?> dict)
        {
            return dict[SuccessKey]?.ToString();
        }

        public void SetErrors(IEnumerable<ServiceError> errors, IDictionary<string, object?> dict)
        {
            if (dict[ErrorsKey] == null)
            {
                dict[ErrorsKey] = JsonSerializer.Serialize(errors);
            }
            else
            {
                var errs = JsonSerializer.Deserialize<List<ServiceError>>(dict[ErrorsKey]!.ToString()!);
                errs?.AddRange(errors);
                dict[ErrorsKey] = JsonSerializer.Serialize(errors);
            }
        }

        public void SetSuccess(string message, IDictionary<string, object?> dict)
        {
            dict[SuccessKey] = message;
        }
    }
}
