using System.IO;
using System.Net.Mail;
using System.ComponentModel;

namespace TAM.VMS.Infrastructure.Enums
{
    public enum DocumentType
    {
        [Description("Face Recognition")]
        FaceRecognition,
        [Description("PCR")]
        PCR,
        [Description("IDCard")]
        IDCard
    }

}
