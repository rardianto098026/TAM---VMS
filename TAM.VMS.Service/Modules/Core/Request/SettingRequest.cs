namespace TAM.VMS.Service
{
    public class SettingRequest
    {
        public LDAPSetting Ldap { get; set; }
        public EmailSetting Email { get; set; }
        public SecuritySetting Security { get; set; }

        public class EmailSetting
        {
            public string FromAddress { get; set; }
            public string FromDisplay { get; set; }
            public string SmtpHost { get; set; }
            public int SmtpPort { get; set; }
            public bool EnableSSL { get; set; }
            public bool UseDefaultCredential { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }
        }
        public class LDAPSetting
        {
            public bool LdapAsPrimaryAuthentication { get; set; }
            public bool LdapAuthenticationOnly { get; set; }
            public bool LdapGroupAsRole { get; set; }
            public string LdapDefaultRole { get; set; }
            public string LdapDomain { get; set; }
            public string LdapPath { get; set; }
        }

        public class SecuritySetting
        {
            public bool PasswordUseDefaultSetting { get; set; }
            public bool PasswordRequireDigit { get; set; }
            public bool PasswordRequireUppercase { get; set; }
            public bool PasswordRequireLowercase { get; set; }
            public bool PasswordRequireNonAplhaNumeric { get; set; }
            public int PasswordMaxLength { get; set; }
            public bool LockoutEnable { get; set; }
            public int LockoutMaxAttemp { get; set; }
            public int LockoutLockingDuration { get; set; }
            public bool OtpEnable { get; set; }
            public bool OtpEmailVerification { get; set; }
        }
    }

}
