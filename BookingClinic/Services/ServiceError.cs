namespace BookingClinic.Services
{
    public class ServiceError
    {
        public string ErrorName { get; set; }
        public string ErrorMessage { get; set; }

        protected ServiceError(string errorName, string errorMessage)
        {
            ErrorName = errorName;
            ErrorMessage = errorMessage;
        }

        public static ServiceError UnexpectedError() =>
            new("Unexpected error", "Some unexpected error happened");

        public static ServiceError UserDoesNotExist() => 
            new("User does not exist", "Specified user was not found");

        public static ServiceError InvalidCredentials() =>
            new("Invalid credentials", "Email or password incorrect");
    }
}
