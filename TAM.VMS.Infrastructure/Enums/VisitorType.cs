using System.IO;
using System.Net.Mail;
using System.ComponentModel;

namespace TAM.VMS.Infrastructure.Enums
{
    public enum VisitorType
    {
        [Description("Reguler")]
        Reguler,
        [Description("VIP")]
        VIP,
        [Description("On The Spot")]
        OnTheSpot
    }

    public enum VisitorCategory
    {
        [Description("Single")]
        Single,
        [Description("Multiple")]
        Multiple
    }

}
