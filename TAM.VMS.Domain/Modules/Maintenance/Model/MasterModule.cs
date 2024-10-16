using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TAM.VMS.Domain
{
    [Table("TB_M_Module")]
    public partial class MasterModule
    {
        [Key]
        public Guid ID { get; set; }

        public string Module { get; set; }

        public string Desc { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }
    }
}