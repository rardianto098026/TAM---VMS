using System.Collections.Generic;

namespace TAM.VMS.Domain
{
    public partial class User
    {
        public string Role { get; set; }
        public string RoleID { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }

    public partial class ResetPassword
    {
        public System.Guid IdResetPW { get; set; }
        public string ResetNewPW { get; set; }
        public string ResetConfirmNewPW { get; set; }
    }

    public partial class ChangePassword
    {
        public System.Guid Id { get; set; }
        public string CurrentPW { get; set; }
        public string NewPW { get; set; }
        public string ConfirmNewPW { get; set; }
    }
}
