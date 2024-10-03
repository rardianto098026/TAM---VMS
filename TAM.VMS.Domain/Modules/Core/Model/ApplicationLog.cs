using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TAM.VMS.Domain
{
    [Table("TB_R_APPLICATION_LOG")]
    public partial class ApplicationLog
    {
        [Key]
        public Guid ID { get; set; }
        public string Username { get; set; }
        public string IP { get; set; }
        public string Browser { get; set; }
        public string MessageType { get; set; }
        public string MessageLocation { get; set; }
        public string MessageDescription { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
    }
}
