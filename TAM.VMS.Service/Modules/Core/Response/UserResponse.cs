namespace TAM.VMS.Service
{
    public class UserResponse
    {
        public string USERNAME { get; set; }
        public string USER_ROLE { get; set; }
        public string USER_LOCATION { get; set; }
        public string ID { get; set; }
        public string REG_NO { get; set; }
        public DateTime ACCOUNT_VALIDITY_DATE { get; set; }
        public string FIRST_NAME { get; set; }
        public string LAST_NAME { get; set; }
        public string GENDER { get; set; }
        public DateTime BIRTH_DATE { get; set; }
        public string ADDRESS { get; set; }
        public string COMPANY { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime CREATED_DATE { get; set; }
        public string CHANGED_BY { get; set; }
        public DateTime? CHANGED_DATE { get; set; }
    }
}
