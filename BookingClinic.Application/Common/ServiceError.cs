namespace BookingClinic.Application.Common
{
    public sealed record ServiceError(ServiceErrorCodes Code, string Message)
    {
        public static ServiceError Create(ServiceErrorCodes code, string message) => new(code, message);

        // ======= Common Errors =============

        public static ServiceError UnexpectedError() => 
            new(ServiceErrorCodes.UnexpectedError, "Unexpected error occured!");

        // ======= Authorization Errors =============

        public static ServiceError Unauthorized() => 
            new(ServiceErrorCodes.Unauthorized, "Action forbidden.");

        // ======= Users Entity Errors =============

        public static ServiceError UserAlreadyExists() =>
            new(ServiceErrorCodes.UserAlreadyExists, "User with such email already exists!");

        public static ServiceError UserNotFound() =>
            new(ServiceErrorCodes.UserNotFound, "User not found!");

        public static ServiceError UserIdCantBeEmpty() =>
            new(ServiceErrorCodes.UserIdCantBeEmpty, "User Id can't be empty!");

        public static ServiceError UserMustProvidePassword() =>
            new(ServiceErrorCodes.UserMustProvidePassword, "User must provide a password!");

        // ======= Clinic Entity Errors =============

        public static ServiceError ClinicNotFound() =>
            new(ServiceErrorCodes.ClinicNotFound, "Clinic not found!");

        public static ServiceError ClinicIdCantBeEmpty() =>
            new(ServiceErrorCodes.ClinicIdCantBeEmpty, "Clinic Id can't be empty!");

        // ======= Speciality Entity Errors =============

        public static ServiceError SpecialityNotFound() =>
            new(ServiceErrorCodes.SpecialityNotFound, "Speciality not found!");

        public static ServiceError SpecialityIdCantBeEmpty() =>
            new(ServiceErrorCodes.SpecialityIdCantBeEmpty, "Speciality Id can't be empty!");

        // ======= Review Entity Errors =============



        // ======= Appointment Entity Errors =============



        public override string ToString() => $"{Code}: {Message}";
    }
}
