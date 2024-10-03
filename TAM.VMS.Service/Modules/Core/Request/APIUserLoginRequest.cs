using System.Text.Json.Serialization;

namespace TAM.VMS.Service
{
    public class APIUserLoginRequest
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
        //public string? RoleStr { get; set; }
        //public string? Token { get; set; }
        [JsonIgnore]
        public string? NoReg { get; set; }
    }
}
