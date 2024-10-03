using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TAM.VMS.Domain
{
    [Table("TB_M_EmailTemplate")]
    public partial class EmailTemplate
    {
        [Key]
        public Guid ID { get; set; }
        public string Module { get; set; }
        public string MailKey { get; set; }
        public string MailFrom { get; set; }
        public string DisplayName { get; set; }
        public string Title { get; set; }
        public string Subject { get; set; }
        public string MailContent { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public bool RowStatus { get; set; }
    }
}
