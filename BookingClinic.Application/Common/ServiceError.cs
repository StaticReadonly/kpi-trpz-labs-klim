namespace BookingClinic.Application.Common
{
    public sealed record ServiceError(string Code, string Message)
    {
        public static ServiceError Create(string code, string message) => new(code, message);

        public static ServiceError UnexpectedError() => new("UnexpectedError", "Произошла непредвиденная ошибка.");
        public static ServiceError InvalidReviewData() => new("InvalidReviewData", "Неверные данные отзыва.");
        public static ServiceError UserAlreadyExists() => new("UserAlreadyExists", "Пользователь с такими данными уже существует.");
        public static ServiceError InvalidCredentials() => new("InvalidCredentials", "Неверные учётные данные.");
        public static ServiceError InvalidPassword() => new("InvalidPassword", "Неверный пароль.");
        public static ServiceError Unauthorized() => new("Unauthorized", "Доступ запрещён.");
        public static ServiceError DoctorNotFound() => new("DoctorNotFound", "Врач не найден.");
        public static ServiceError AppointmentAlreadyExists() => new("AppointmentAlreadyExists", "Запись уже существует.");
        public static ServiceError AppointmentNotFound() => new("AppointmentNotFound", "Запись не найдена.");

        public override string ToString() => $"{Code}: {Message}";
    }
}
