using Microsoft.Extensions.Configuration;

namespace TAM.VMS.Infrastructure
{
    public static class AppConstants
    {
        public const string SessionKey = "AppUserSession";
        public const string TokenSessionKey = "AppTokenSession";
        public const string NoRecordsText = "No records yet";

        public static string HangfireConnectionString
        {
            get
            {
                return GetConnectionStringHangFire();
            }
        }
        public const string HangfireSchemaName = "HANGFIRE";
        private static string GetConnectionStringHangFire()
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var connectionString = config["ConnectionStrings:HangfireConnection"];
            return connectionString;
        }

        public class CoreRoles
        {
            public const string DefaultUser = "DEFAULT_USER";
            public const string Administrator = "ADMINISTRATOR";
        }

    }
    public static class MailStatus
    {
        public const string Pending = "pending";
        public const string Sent = "sent";
    }
}
