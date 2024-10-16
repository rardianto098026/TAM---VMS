using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TAM.VMS.Domain
{
    [Table("VW_DetailRequestView")]

    public class DetailRequestView
    {
        public Guid ID { get; set; }
        public string VendorName { get; set; }
        public string VendorCode { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string RequestedBy { get; set; }
        public string TransactionCategoryRequest { get; set; }
        public string VMSStatus { get; set; }
        public string SAPTrade { get; set; }
        public string SAPNonTrade { get; set; }
        public string Reason { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
