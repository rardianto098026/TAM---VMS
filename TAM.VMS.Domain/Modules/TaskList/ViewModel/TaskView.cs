using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TAM.VMS.Domain
{
    [Table("VW_TaskView")]

    public class TaskView
    {
        public Guid ID { get; set; }

        public string ModuleName { get; set; }

        public string VendorName { get; set; }

        public string Complience { get; set; }

        public string TransactionCategory { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public string Status { get; set; }
    }
}
