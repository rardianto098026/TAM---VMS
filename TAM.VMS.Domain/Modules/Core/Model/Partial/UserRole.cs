using System;
using System.ComponentModel.DataAnnotations;

namespace TAM.VMS.Domain
{
    public partial class UserRole 
    {
        [Key]
        public string Username { get; set; }
        public string RoleName { get; set; }

    }
}
