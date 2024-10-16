using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TAM.VMS.Domain
{
    [Table("VW_TaskView")]

    public class TaskView
    {
        public Guid ID { get; set; }
        public Guid IdModule { get; set; }

        public string Module { get; set; }

        public string VendorName { get; set; }

        public string Complience { get; set; }

        public string TransactionCategory { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public string Status { get; set; }
        public string StatusID { get; set; }
        public string Role { get; set; }
        public string Level { get; set; }
        public Guid IdModuleProcess { get; set; }
        public Guid IdDataByModule { get; set; }
    }
}
