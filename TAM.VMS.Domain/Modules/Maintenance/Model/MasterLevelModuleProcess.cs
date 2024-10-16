using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TAM.VMS.Domain
{
    [Table("TB_M_LevelModuleProcess")]
    public partial class MasterLevelModuleProcess
    {
        [Key]
        public Guid ID { get; set; }

        public Guid IDModule { get; set; }

        public int Level { get; set; }
        public Guid IdRole { get; set; }
        public string Desc { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}