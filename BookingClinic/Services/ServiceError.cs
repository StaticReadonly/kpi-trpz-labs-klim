namespace BookingClinic.Services
{
    public class ServiceError
    {
        public string ErrorName { get; set; }
        public string ErrorMessage { get; set; }

        public ServiceError(string errorName, string errorMessage)
        {
            ErrorName = errorName;
            ErrorMessage = errorMessage;
        }

        public static ServiceError UnexpectedError() =>
            new("Unexpected error", "Some unexpected error happened");

        public static ServiceError UserAlreadyExists() => 
            new("User already exists", "User already exists");

        public static ServiceError InvalidCredentials() =>
            new("Invalid credentials", "Email or password incorrect");

        public static ServiceError InvalidPassword() =>
            new("Invalid password", "Password is invalid");

        public static ServiceError Unauthorized() =>
            new("User unauthorized", "User is unauthorized");

        public static ServiceError DoctorNotFound() =>
            new("Doctor not found", "Doctor not found");

        public static ServiceError AppointmentAlreadyExists() =>
            new("Appointment exists", "Appointment already exists");
    }
}
