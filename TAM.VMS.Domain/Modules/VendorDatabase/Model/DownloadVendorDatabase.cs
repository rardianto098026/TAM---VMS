using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TAM.VMS.Domain
{
    [Table("TB_R_DownloadVendorDB")]
    public partial class DownloadVendorDatabase
    {
        [Key]
        public Guid ID { get; set; }

        public string FileName { get; set; }
        public DateTime? ReqDate { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public DateTime? FileDate { get; set; }
        public string DepartmentId { get; set; }
        public string StatusId { get; set; }

        public DateTime? CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
    }

}
