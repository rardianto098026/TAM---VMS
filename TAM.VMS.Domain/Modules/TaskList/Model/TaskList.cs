using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TAM.VMS.Domain
{
    [Table("TB_M_TaskList")]
    public partial class TaskList
    {
        [Key]
        //public Guid ID { get; set; }
        //public string TaskID { get; set; }
        public string ModuleName { get; set; }
    }
}