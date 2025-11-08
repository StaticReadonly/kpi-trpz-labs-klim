using Microsoft.AspNetCore.Authorization;

namespace BookingClinic
{
    public static class AuthorizationPolicies
    {
        /// <summary>
        /// Allows access only for admin.
        /// </summary>
        public const string AdminOnlyPolicy = "Admin";

        /// <summary>
        /// Configure admin only policy.
        /// </summary>
        /// <param name="opts"><see cref="AuthorizationOptions"/> object.</param>
        public static void BuildAdminOnlyPolicy(AuthorizationOptions opts)
        {
            opts.AddPolicy(AdminOnlyPolicy, cfg =>
            {
                cfg.RequireAuthenticatedUser()
                        .RequireRole("Admin");
            });
        }

        /// <summary>
        /// Allows access only for authorized users.
        /// </summary>
        public const string AuthorizedUserOnlyPolicy = "AuthUser";

        /// <summary>
        /// Configure authorized user policy.
        /// </summary>
        /// <param name="opts"><see cref="AuthorizationOptions"/> object.</param>
        public static void BuildUserOnlyPolicy(AuthorizationOptions opts)
        {
            opts.AddPolicy(AuthorizedUserOnlyPolicy, cfg =>
            {
                cfg.RequireAuthenticatedUser();
            });
        }

        /// <summary>
        /// Allows access only for doctors.
        /// </summary>
        public const string DoctorOnlyPolicy = "Doctor";

        /// <summary>
        /// Configure doctor only policy.
        /// </summary>
        /// <param name="opts"><see cref="AuthorizationOptions"/> object.</param>
        public static void BuildDoctorOnlyPolicy(AuthorizationOptions opts)
        {
            opts.AddPolicy(DoctorOnlyPolicy, cfg =>
            {
                cfg.RequireAuthenticatedUser()
                        .RequireRole("Doctor");
            });
        }

        /// <summary>
        /// Allows access only for patients and admins.
        /// </summary>
        public const string PatientOnlyPolicy = "Patient";

        /// <summary>
        /// Configure patient only policy.
        /// </summary>
        /// <param name="opts"><see cref="AuthorizationOptions"/> object.</param>
        public static void BuildPatientOnlyPolicy(AuthorizationOptions opts)
        {
            opts.AddPolicy(PatientOnlyPolicy, cfg =>
            {
                cfg.RequireAuthenticatedUser()
                        .RequireRole("Patient", "Admin");
            });
        }
    }
}
