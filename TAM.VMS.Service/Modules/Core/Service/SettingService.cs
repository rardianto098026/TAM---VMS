using TAM.VMS.Domain;
using System.Linq;
using Dapper;
using TAM.VMS.Infrastructure.Session;

namespace TAM.VMS.Service
{
    public class SettingService : DbService
    {
        public SettingService(IDbHelper db) : base(db)
        {
        }

        public SettingRequest GetSetting()
        {
            var settings = Db.ConfigRepository.FindAll();

            SettingRequest setting = new SettingRequest();

            SettingRequest.LDAPSetting ldap = new SettingRequest.LDAPSetting();
            ldap.LdapAsPrimaryAuthentication = bool.Parse(settings.FirstOrDefault(s => s.Module == "LDAP" && s.ConfigKey == "ldap-primary-authentication").ConfigValue);
            ldap.LdapAuthenticationOnly = bool.Parse(settings.FirstOrDefault(s => s.Module == "LDAP" && s.ConfigKey == "ldap-authentication-only").ConfigValue);
            ldap.LdapGroupAsRole = bool.Parse(settings.FirstOrDefault(s => s.Module == "LDAP" && s.ConfigKey == "ldap-group-as-role").ConfigValue);
            ldap.LdapDefaultRole = settings.FirstOrDefault(s => s.Module == "LDAP" && s.ConfigKey == "ldap-default-role").ConfigValue;
            ldap.LdapDomain = settings.FirstOrDefault(s => s.Module == "LDAP" && s.ConfigKey == "ldap-domain").ConfigValue;
            ldap.LdapPath = settings.FirstOrDefault(s => s.Module == "LDAP" && s.ConfigKey == "ldap-path").ConfigValue;
            setting.Ldap = ldap;

            SettingRequest.SecuritySetting security = new SettingRequest.SecuritySetting();
            security.PasswordUseDefaultSetting = bool.Parse(settings.FirstOrDefault(s => s.Module == "Security" && s.ConfigKey == "password-use-default-setting").ConfigValue);
            security.PasswordRequireDigit = bool.Parse(settings.FirstOrDefault(s => s.Module == "Security" && s.ConfigKey == "password-require-digit").ConfigValue);
            security.PasswordRequireUppercase = bool.Parse(settings.FirstOrDefault(s => s.Module == "Security" && s.ConfigKey == "password-require-uppercase").ConfigValue);
            security.PasswordRequireLowercase = bool.Parse(settings.FirstOrDefault(s => s.Module == "Security" && s.ConfigKey == "password-require-lowercase").ConfigValue);
            security.PasswordRequireNonAplhaNumeric = bool.Parse(settings.FirstOrDefault(s => s.Module == "Security" && s.ConfigKey == "password-require-nonalphanumeric").ConfigValue);
            security.PasswordMaxLength = int.Parse(settings.FirstOrDefault(s => s.Module == "Security" && s.ConfigKey == "password-max-length").ConfigValue);
            security.LockoutEnable = bool.Parse(settings.FirstOrDefault(s => s.Module == "Security" && s.ConfigKey == "lockout-enable").ConfigValue);
            security.LockoutMaxAttemp = int.Parse(settings.FirstOrDefault(s => s.Module == "Security" && s.ConfigKey == "lockout-max-attempt").ConfigValue);
            security.LockoutLockingDuration = int.Parse(settings.FirstOrDefault(s => s.Module == "Security" && s.ConfigKey == "lockout-locking-duration").ConfigValue);
            security.OtpEnable = bool.Parse(settings.FirstOrDefault(s => s.Module == "Security" && s.ConfigKey == "otp-enable").ConfigValue);
            security.OtpEmailVerification = bool.Parse(settings.FirstOrDefault(s => s.Module == "Security" && s.ConfigKey == "otp-email").ConfigValue);
            setting.Security = security;

            SettingRequest.EmailSetting email = new SettingRequest.EmailSetting();
            email.FromAddress = settings.FirstOrDefault(s => s.Module == "Email" && s.ConfigKey == "email-from-address").ConfigValue;
            email.FromDisplay = settings.FirstOrDefault(s => s.Module == "Email" && s.ConfigKey == "email-from-display").ConfigValue;
            email.SmtpHost = settings.FirstOrDefault(s => s.Module == "Email" && s.ConfigKey == "email-smtp-host").ConfigValue;
            email.SmtpPort = int.Parse(settings.FirstOrDefault(s => s.Module == "Email" && s.ConfigKey == "email-smtp-port").ConfigValue);
            email.Username = settings.FirstOrDefault(s => s.Module == "Email" && s.ConfigKey == "email-username").ConfigValue;
            email.Password = settings.FirstOrDefault(s => s.Module == "Email" && s.ConfigKey == "email-password").ConfigValue;
            email.EnableSSL = bool.Parse(settings.FirstOrDefault(s => s.Module == "Email" && s.ConfigKey == "email-enable-ssl").ConfigValue);
            email.UseDefaultCredential = bool.Parse(settings.FirstOrDefault(s => s.Module == "Email" && s.ConfigKey == "email-use-default-credential").ConfigValue);
            setting.Email = email;

            return setting;
        }

        private void SaveSetting(string key, string value, string module)
        {

            var parameterHeaders = new DynamicParameters();
            parameterHeaders.Add("@Key", key);
            parameterHeaders.Add("@Value", value);
            parameterHeaders.Add("@Module", module);
            parameterHeaders.Add("@By", SessionManager.Current);
            using (DbHelper db = new DbHelper(true))
            {
                db.Connection.Execute("usp_Core_UpdateSetting", parameterHeaders, db.Transaction, commandType: System.Data.CommandType.StoredProcedure);
                db.Commit();
            }
        }

        public void SaveLdapSetting(SettingRequest.LDAPSetting ldap)
        {
            SaveSetting("ldap-primary-authentication", ldap.LdapAsPrimaryAuthentication.ToString(), "LDAP");
            SaveSetting("ldap-authentication-only", ldap.LdapAuthenticationOnly.ToString(), "LDAP");
            SaveSetting("ldap-group-as-role", ldap.LdapGroupAsRole.ToString(), "LDAP");
            SaveSetting("ldap-default-role", ldap.LdapDefaultRole.ToString(), "LDAP");
            SaveSetting("ldap-domain", ldap.LdapDomain.ToString(), "LDAP");
            SaveSetting("ldap-path", ldap.LdapPath.ToString(), "LDAP");
        }
        public void SaveFRCardSetting(bool FRCard)
        {
            SaveSetting("frcard-setting", FRCard == true ? "True" : "False", "FRCard");
        }

        public void SaveEmailSetting(SettingRequest.EmailSetting email)
        {
            SaveSetting("email-enable-ssl", email.EnableSSL.ToString(), "Email");
            SaveSetting("email-use-default-credential", email.UseDefaultCredential.ToString(), "Email");
            SaveSetting("email-from-address", email.FromAddress.ToString(), "Email");
            SaveSetting("email-from-display", email.FromDisplay.ToString(), "Email");
            SaveSetting("email-username", email.Username.ToString(), "Email");
            SaveSetting("email-password", email.Password.ToString(), "Email");
            SaveSetting("email-smtp-host", email.SmtpHost.ToString(), "Email");
            SaveSetting("email-smtp-port", email.SmtpPort.ToString(), "Email");
        }

        public void SaveSecuritySetting(SettingRequest.SecuritySetting security)
        {
            SaveSetting("password-max-length", security.PasswordMaxLength.ToString(), "Security");
            SaveSetting("password-require-digit", security.PasswordRequireDigit.ToString(), "Security");
            SaveSetting("password-require-lowercase", security.PasswordRequireLowercase.ToString(), "Security");
            SaveSetting("password-require-nonalphanumeric", security.PasswordRequireNonAplhaNumeric.ToString(), "Security");
            SaveSetting("password-require-uppercase", security.PasswordRequireUppercase.ToString(), "Security");
            SaveSetting("password-use-default-setting", security.PasswordUseDefaultSetting.ToString(), "Security");
            SaveSetting("lockout-enable", security.LockoutEnable.ToString(), "Security");
            SaveSetting("lockout-locking-duration", security.LockoutLockingDuration.ToString(), "Security");
            SaveSetting("lockout-max-attempt", security.LockoutMaxAttemp.ToString(), "Security");
            SaveSetting("otp-email", security.OtpEmailVerification.ToString(), "Security");
            SaveSetting("otp-enable", security.OtpEnable.ToString(), "Security");
        }
    }
}
