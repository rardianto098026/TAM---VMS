using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TAM.VMS.Domain
{
    [Table("TB_M_Config")]
    public partial class Config
    {
        [Key]
        public Guid ID { get; set; }
        public string Module { get; set; }
        public string ConfigKey { get; set; }
        public string ConfigText { get; set; }
        public string ConfigValue { get; set; }
        public string ConfigDataType { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public bool RowStatus { get; set; }
    }
}
