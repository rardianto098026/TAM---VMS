using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TAM.VMS.Domain
{
    [Table("TB_M_CategoryDepartmentMapping")]
    public partial class MasterCategoryDepartmentMapping
    {
        [Key]
        public Guid ID { get; set; }
        public Guid CategoryID { get; set; }
        public Guid DepartmentID { get; set; }
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public short RowStatus { get; set; } 
    }
}
