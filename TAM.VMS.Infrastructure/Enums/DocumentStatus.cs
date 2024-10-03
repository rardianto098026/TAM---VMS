using System.IO;
using System.Net.Mail;
using System.ComponentModel;

namespace TAM.VMS.Infrastructure.Enums
{
    public enum DocumentStatus
    {
        [Description("Draft")]
        Draft,
        [Description("Submit")]
        PCR
    }
    public enum ExtendVisitType
    {
        [Description("Day")]
        Day,
        [Description("Hour")]
        Hour
    }


}
