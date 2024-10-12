using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TAM.VMS.Domain
{
    [Table("TB_M_TaskList")]
    public partial class TaskList
    {
        [Key]
        public Guid ID { get; set; }

        public string ModuleName { get; set; }

        public string VendorName { get; set; }

        public string Complience { get; set; }

        public string TransactionCategory { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }
        public Guid IdModule { get; set; }
        public Guid IdDataByModule { get; set; }
    }
}