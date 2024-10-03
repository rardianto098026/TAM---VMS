using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TAM.VMS.Domain
{
    [Table("TB_M_RolePermission")]
    public partial class RolePermission
    {
        [Key]
        public Guid ID { get; set; }
        public Guid RoleID { get; set; }
        public Guid PermissionID { get; set; }
    }
}