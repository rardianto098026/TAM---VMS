using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TAM.VMS.Domain
{
    [Table("TB_M_Department")]
    public partial class MasterDepartment
    {
        [Key]
        public Guid ID { get; set; }

        public string DepartmentName { get; set; }
        public string DepartmentDesc { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public short RowStatus { get; set; } 
    }
}
