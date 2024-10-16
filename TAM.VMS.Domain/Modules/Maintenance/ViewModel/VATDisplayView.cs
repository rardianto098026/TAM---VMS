using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace TAM.VMS.Domain
{
    [Table("VW_BusinessCategoryDisplay")]
    public class VATDisplayView
    {
        public Guid ClassificationID { get; set; }
        public string BusinessClassification { get; set; }
        public string BusinessCategory { get; set; }
        public string MappingTo { get; set; }
        public Guid DepartmentID { get; set; }
        public string RowStatus { get; set; }
        public string[] classificationsArray { get; set; }
        public string[] categoriesArray { get; set; }
        public string[] mappingsArray { get; set; }
    }
}
