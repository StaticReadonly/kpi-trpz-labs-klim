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

        public static ServiceError InvalidEmailOrPassword() =>
            new(ServiceErrorCodes.InvalidEmailOrPassword, "Invalid email or password.");

        // ======= Users Entity Errors =============

        public static ServiceError UserAlreadyExists() =>
            new(ServiceErrorCodes.UserAlreadyExists, "User with such email already exists!");

        public static ServiceError UserNotFound() =>
            new(ServiceErrorCodes.UserNotFound, "User not found!");

        public static ServiceError UserIdCantBeEmpty() =>
            new(ServiceErrorCodes.UserIdCantBeEmpty, "User Id can't be empty!");

        public static ServiceError UserMustProvidePassword() =>
            new(ServiceErrorCodes.UserMustProvidePassword, "User must provide a password!");

        public static ServiceError DoctorNotFound() =>
            new(ServiceErrorCodes.DoctorNotFound, "Doctor not found!");

        public static ServiceError MustProvideImage() =>
            new(ServiceErrorCodes.MustProvideImage, "Must provide an image file!");

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

        public static ServiceError ReviewNotFound() =>
            new(ServiceErrorCodes.ReviewNotFound, "Review not found!"); 

        // ======= Appointment Entity Errors =============

        public static ServiceError AppointmentAlreadyExists() =>
            new(ServiceErrorCodes.AppointmentAlreadyExists, "Appointment already exists!");

        public static ServiceError AppointmentNotFound() =>
            new(ServiceErrorCodes.AppointmentNotFound, "Appointment not found!");

        public override string ToString() => $"{Code}: {Message}";
    }
}
