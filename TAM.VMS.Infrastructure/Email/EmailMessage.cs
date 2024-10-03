using System.IO;
using System.Net.Mail;
using System.Net.Mime;
using System.Collections.Generic;

namespace TAM.VMS.Infrastructure.Email
{
    public class EmailMessage
    {
        public readonly IList<string> CC = new List<string>();
        public readonly IList<string> Bcc = new List<string>();
        public readonly IList<string> Recipients = new List<string>();
        public readonly IList<Attachment> Attachments = new List<Attachment>();
        public readonly IList<AlternateView> AlternateViews = new List<AlternateView>();

        public string Subject { get; set; }
        public string From { get; set; }
        public string DisplayName { get; set; }
        public string Body { get; set; }
        public bool IsBodyHtml { get; set; }

        private void AttachAttachment(Attachment attachment, ContentDisposition contentDisposition = null)
        {
            if (contentDisposition != null)
            {
                attachment.ContentDisposition.Inline = contentDisposition.Inline;
                attachment.ContentDisposition.DispositionType = contentDisposition.DispositionType;
            }

            Attachments.Add(attachment);
        }

        public void AddAttachment(Stream stream, ContentType contentType, ContentDisposition contentDisposition = null)
        {
            var attachment = new Attachment(stream, contentType);
            AttachAttachment(attachment);
        }

        public void AddAttachment(Stream stream, string name, string mediaType, ContentDisposition contentDisposition = null)
        {
            var attachment = new Attachment(stream, name, mediaType);
            AttachAttachment(attachment);
        }

        public void AddAttachment(string filePath, string mediaType)
        {
            Attachments.Add(new Attachment(filePath, mediaType));
        }
    }
}
