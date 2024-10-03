using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TAM.VMS.Domain
{
    [Table("VW_UserRole")]

    public class UserRoleView
    {
        public Guid ID { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string NIP { get; set; }
        public string Role { get; set; }
        public string RoleID { get; set; }
        public string Area { get; set; }
        public string AreaID { get; set; }
        public string DivisionID { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
        public bool RowStatus { get; set; }
        public bool IsUserPassport { get; set; }

    }
}
