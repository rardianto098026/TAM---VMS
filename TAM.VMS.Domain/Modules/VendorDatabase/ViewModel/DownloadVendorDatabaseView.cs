using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TAM.VMS.Domain
{
    [Table("VW_DownloadVendorDB")]

    public class DownloadVendorDBView
    {
        public Guid ID { get; set; }
        public string FileName { get; set; }
        public DateTime? ReqDate { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public DateTime? FileDate { get; set; }
        public string DepartmentName { get; set; }
        public string Status { get; set; }
        public string StatusID { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string Level { get; set; }
        public string RoleName { get; set; }
        public string RoleDesc { get; set; }
    }

}
