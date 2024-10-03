using System.IO;
using System.Net.Mail;
using System.ComponentModel;

namespace TAM.VMS.Infrastructure.Enums
{
    public enum Module
    {
        [Description("Visitor")]
        Visitor,
        [Description("On The Spot")]
        OnTheSpot,
        [Description("Internship")]
        Internship,
        [Description("SHD Invitation")]
        SHDInvitation,
        [Description("Antigen")]
        Antigen,
        [Description("Check In")]
        CheckIn,
        [Description("Extend Visit")]
        ExtendVisit
    }
    public enum ApprovalModule
    {
        [Description("Visitor")]
        Visitor,
        [Description("Visitor Regular")]
        VisitorRegular,
        [Description("Visitor VIP")]
        VisitorVIP,
        [Description("On The Spot")]
        OnTheSpot,
        [Description("Internship")]
        Internship,
        [Description("Periodic")]
        Periodic,
        [Description("SHD Form")]
        SHDForm,
        [Description("Antigen")]
        Antigen,
        [Description("Check In")]
        CheckIn,
        [Description("Extend Visit")]
        ExtendVisit
    }

}
