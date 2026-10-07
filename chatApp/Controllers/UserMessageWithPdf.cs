using Microsoft.AspNetCore.Mvc;

namespace chatApp.Controllers
{
    internal class UserMessageWithPdf
    {
        public UserMessageWithPdf()
        {
        }

        public object MessageId { get; set; }
        public object Message { get; set; }
        public object SentDate { get; set; }
        public object SenderId { get; set; }
        public object PdfBytes { get; set; }
        public string PdfContentType { get; set; }
        public object PdfFileName { get; set; }
        public FileContentResult BookPath { get; internal set; }
        public string ContentType { get; internal set; }
        public string FileName { get; internal set; }
    }
}