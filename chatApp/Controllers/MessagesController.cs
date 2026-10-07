using chatApp.Models;
using chatApp.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http.Results;

namespace chatApp.Controllers
{
    public class MessagesController : Controller
    {
        private readonly IHubContext<NotifyHub> hubContext;
        private readonly IGenericRepository<Messages> repository = null;

        public MessagesController(IGenericRepository<Messages> repository,IHubContext<NotifyHub> _hubContext)
        {
            this.hubContext = _hubContext;
            this.repository = repository;
        }
        //[Route("SendMessageByUser/{Id}")]
        
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> SendMessageByUserAsync([FromBody] Messages msg) //Add a message to a specific id
        {

            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                //IEnumerable<Claim> claims = identity.Claims;
                // or
              
                string SenderId=identity.FindFirst("UserID").Value;

                msg.SenderId = SenderId;  // custom Authorization for protecting Sender ID and getting from token....

            }

            msg.EnrollmentDate= DateTime.Now;
            //  msg.SenderId = id;
            this.repository.Add(msg);
            CustomResponse response = this.repository.Save();
            if (response.Success)
            {
                //message sent l
            }
            else
            {
                //error
                var error = response.Exception;
            }
            await this.hubContext.Clients.User(msg.RecieverId).SendAsync("messageReceivedFromApi", msg);
            //await this.hubContext.Clients.All.SendAsync("messageReceivedFromApi",msg);
            return Ok(msg);
        }

        // [Route("GetMessagesByUser/{Id}")]

        [HttpGet]
       //[Route("GetMessages/{SenderId}/receivedBy/{ReceiverID}")]
        
        public IActionResult GetMessages([FromQuery] ConnectionUser connectionId)
        {

            //User.Identity.

            //SELECT Message AS Message, EnrollmentDate AS SenTDate FROM Messages WHERE SenderId = userId
            IQueryable<Messages> queryable = repository.GetQueryable(p => p.SenderId == connectionId.SenderId && p.RecieverId==connectionId.RecieverId || p.SenderId==connectionId.RecieverId && p.RecieverId==connectionId.SenderId);
            List<UserMessage> onlyMessages = queryable.Select(p => new UserMessage()
            {
                Message = p.Message,
                SentDate = p.EnrollmentDate,
                SenderId=p.SenderId,
            }).ToList()
            .Select(p=> new UserMessage()
            {
               Message = p.Message,
                SentDate = p.SentDate,
                SenderId = p.SenderId,
            }).ToList();

            //SELECT * FROM Messages WHERE SenderId = userId
            /* List<Messages> filteredMessages = repository.GetByExpression(p => p.SenderId == userId);*/

            //SELECT * FROM Messages
            /*List<Messages> messages = (List<Messages>)this.repository.GellAll().Where(model => model.SenderId == userId);*/

            
            return Ok(onlyMessages);
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetBooksPath()
        {

            //User.Identity.

            //SELECT Message AS Message, EnrollmentDate AS SenTDate FROM Messages WHERE SenderId = userId
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            string SenderId = identity.FindFirst("UserID").Value;
            IQueryable<Messages> queryable = repository.GetQueryable(p => p.SenderId == SenderId && p.FileLink!=null);
            List<UserMessage> onlyMessages = queryable.Select(p => new UserMessage()
            {
                Message = p.Message,
                SentDate = p.EnrollmentDate,
                SenderId = p.SenderId,
                BookPath =p.FileLink
            }).ToList()
            .Select(p => new UserMessage()
            {
                Message = p.Message,
                SentDate = p.SentDate,
                SenderId = p.SenderId,
                BookPath = p.BookPath
            }).ToList();

            List<FileContentResult> pdfFiles = new List<FileContentResult>();

            // Loop through each message and read the PDF file from the file system
            /*  foreach (Messages message in queryable)
               {
                   byte[] fileBytes = System.IO.File.ReadAllBytes(message.FileLink);
                   pdfFiles.Add(new FileContentResult(fileBytes, "application/pdf") { FileDownloadName = message.Message });
               }*/


            // we loop through each message in the queryable result and read the corresponding PDF file from the file system.For each message, we create a new UserMessageWithPdf object and set its properties including the PDF bytes, content type, and file name. We also set the MessageId property to the ID of the message object, so that we can later associate the PDF with the correct message.Finally, we add the UserMessageWithPdf object to the userMessagesWithPdf list.After looping through all the messages, we return the list of UserMessageWithPdf objects as the response.
            /*   List<UserMessageWithPdf> userMessagesWithPdf = new List<UserMessageWithPdf>();

              foreach (Messages message in queryable)
               {
                   byte[] fileBytes = System.IO.File.ReadAllBytes(message.FileLink);
                   string fileName = Path.GetFileName(message.FileLink);

                   userMessagesWithPdf.Add(new UserMessageWithPdf
                   {
                       MessageId = message.Id,
                       Message = message.Message,
                       SentDate = message.EnrollmentDate,
                       SenderId = message.SenderId,
                       BookPath = new FileContentResult(fileBytes, "application/pdf") { FileDownloadName = fileName }
                   });
               }

               return Ok(userMessagesWithPdf);*/


            List<UserMessageWithPdf> userMessagesWithPdf = new List<UserMessageWithPdf>();
            foreach (Messages message in queryable)
            {
                byte[] fileBytes = System.IO.File.ReadAllBytes(message.FileLink);
                UserMessageWithPdf userMessageWithPdf = new UserMessageWithPdf
                {
                    MessageId = message.Id,
                    Message = message.Message,
                    SentDate = message.EnrollmentDate,
                    SenderId = message.SenderId,
                    PdfBytes = fileBytes,
                    ContentType = "application/pdf",
                    FileName = message.Message + ".pdf"
                };
                userMessagesWithPdf.Add(userMessageWithPdf);
            }

            // Return the list of UserMessageWithPdf objects as the response
            return Ok(userMessagesWithPdf);



           // return Ok(pdfFiles);
        }
        public IActionResult Index()
        {
            return View();
        }

        
    }

    public class UserMessage
    {
        public string Message { get; set; }
        public DateTime? SentDate { get; set; }

        public string SenderId { get; set; }

        public string BookPath { get; set; }
    }

    public class UserstringId
        {
         public   string contactId { get; set; }
        }
    public class ConnectionUser
    {
        public string SenderId { get; set; }
        public string RecieverId { get; set; }
    }

    public class BookConnection
    {
        public string SenderId;
    }
}
