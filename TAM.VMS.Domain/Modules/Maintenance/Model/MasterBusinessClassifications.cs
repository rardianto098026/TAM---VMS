using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TAM.VMS.Domain
{
    [Table("TB_M_BusinessClassifications")]
    public partial class MasterBusinessClassifications
    {
        [Key]
        public Guid ID { get; set; }

        public string Name { get; set; }  

        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public short RowStatus { get; set; } 
    }
}
