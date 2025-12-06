namespace BookingClinic.Application.Common
{
    public enum ServiceErrorCodes
    {
        //Common
        UnexpectedError,

        //Authorization
        Unauthorized,
        InvalidEmailOrPassword,

        //Users
        UserAlreadyExists,
        UserNotFound,
        UserIdCantBeEmpty,
        UserMustProvidePassword,
        DoctorNotFound,
        MustProvideImage,

        //Clinic
        ClinicNotFound,
        ClinicIdCantBeEmpty,

        //Speciality
        SpecialityNotFound,
        SpecialityIdCantBeEmpty,

        //Review
        ReviewNotFound,

        //Appointment
        AppointmentNotFound,
        AppointmentAlreadyExists
    }
}
