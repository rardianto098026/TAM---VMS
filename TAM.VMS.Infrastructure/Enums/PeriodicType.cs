using System.IO;
using System.Net.Mail;
using System.ComponentModel;

namespace TAM.VMS.Infrastructure.Enums
{
    public enum PeriodicType
    {
        [Description("Internship")]
        Internship,
        [Description("Periodic")]
        Periodic
    }

    public enum PeriodicCategory
    {
        [Description("Single")]
        Single,
        [Description("Multiple")]
        Multiple
    }

}
