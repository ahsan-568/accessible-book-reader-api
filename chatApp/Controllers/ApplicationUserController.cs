using chatApp.Models;
using chatApp.Repository;
using DotNetOpenAuth.AspNet.Clients;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static chatApp.Models.MakingHttpRequest;
using chatApp;
using System.IO;
using System.Net.Http.Headers;
using static DotNetOpenAuth.OpenId.Extensions.AttributeExchange.WellKnownAttributes;
using Microsoft.AspNetCore.Authorization;
using System.Web.Http.Results;
using Microsoft.AspNetCore.Hosting;

namespace chatApp.Controllers
{

    public class ApplicationUserController : Controller
    {
        private readonly IGenericRepository<ApplicationUser> repository = null;
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;
        private List<AuthenticationScheme> ExternalLogins;
        private IConfiguration _config;
        private readonly IserviceTTS _TTSservice;
        private readonly IGenericRepository<Messages> repository_message = null;
        private readonly IWebHostEnvironment _env;



        public ApplicationUserController(IGenericRepository<ApplicationUser> repository,
            UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signManger, IserviceTTS TTSservcie,
            IConfiguration _config, IGenericRepository<Messages> repository_message, IWebHostEnvironment env)
        {
            this.repository = repository;         //initializing repository to a specific table
            this._userManager = userManager;// initializinh usermanager by dependency injection
            _signInManager = signManger;
            this._TTSservice = TTSservcie;
            this._config = _config;
            this.repository_message = repository_message;
            _env = env;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        

        public IActionResult GetAll()
        {
            
            List<ApplicationUser> users = (List<ApplicationUser>)this.repository.GellAll();  // to list has its own loop
            return Ok(users); //API Generation...
        }


        [HttpPost]
        // 
        public async Task<IActionResult> Add([FromBody] ApplicationUser user)
        {

           
            user.Id = Guid.NewGuid().ToString(); // generating iD in runtime by backend
            ApplicationUser appUser = new ApplicationUser { Id = user.Id, UserName = user.FirstName, Email = user.Email };

            IdentityResult result = await _userManager.CreateAsync(appUser, user.PasswordHash);

            if (result.Succeeded)
            {
                return Ok(user);
            }

            else
            {
                foreach (IdentityError error in result.Errors)
                    ModelState.AddModelError("", error.Description);
            }

            //user.EnrollmentDate = DateTime.Now;
            //repository.Add(user);

            //repository.Save();

            return Ok(user); //Api Generation
        }

      

        [HttpGet]
        public async Task<IActionResult> AuthAsync([FromQuery] loginSession userLogin)
        {
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            var user = new ApplicationUser();
           
            user = await _userManager.FindByEmailAsync(userLogin.Email);
            PasswordVerificationResult result = _userManager.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, userLogin.PasswordHash);

            if (result == PasswordVerificationResult.Success)
            {
                
                
                    JWT jwt = new JWT(this._config);
                    var token = jwt.GenerateJSONWebToken(user); // this user is traced throuh an email and will pass only if user name and its password is valid..
                    userLogin.SenderId = user.Id;
                    userLogin.FirstName = user.UserName;
                    userLogin.success = true;
                    return Ok(token);
             }

                  // var token= GenerateJSONWebToken(userLogin);

                 //  userLogin.SenderId = user.Id;

                 // userLogin.FirstName = user.UserName;

                 // above three parameters will be shifted to token return if user exist with same password and this user is worked as a sender

                // return Ok("Error");
           

            else {
                userLogin.success = false;
                return Ok(userLogin);
            }


        }

        [HttpGet]
        public string Get()
        {


            string name = "کیسے ہیں احسن صاحب";
            String accessToken = "ffff96ec-0ab9-446e-a636-cf9d086ac212";
            String inputText = name;

            String JSON_MSG = "{ \"text\" : \"" + inputText + "\" , \"token\" : \"" + accessToken + "\", \"voice\" : \"CLE_Naghma1\", \"rate\" : \"" + 2 + "\" , \"volume\" : \"" + 100 + "\"}";

              using (var client = new HttpClient())
              {
                  var endpoint = new Uri("https://api.cle.org.pk/v1/synth");
                  var payload = new StringContent(JSON_MSG, Encoding.UTF8, "application/json");
                  var response = client.PostAsync(endpoint, payload).Result.Content.ReadAsStringAsync().Result;
                  return client.PostAsync(endpoint, payload).Result.Content.ReadAsStringAsync().Result;
              }


           /* using (var client = new HttpClient())
            {
                var endpoint = new Uri("https://pagesegmentation.pythonanywhere.com//upload");
                var payload = new StringContent("", Encoding.UTF8, "application/json");
                var response = client.PostAsync(endpoint, payload).Result.Content.ReadAsStringAsync().Result;
                var result = client.PostAsync(endpoint, payload).Result.Content.ReadAsStringAsync().Result;
            }*/



            // return await _TTSservice.Get(JSON_MSG);
        }


        [Authorize]
        [HttpPost, DisableRequestSizeLimit]
        public async Task<IActionResult> postFile()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            string SenderId = identity.FindFirst("UserID").Value;
            var nameClaim = identity.FindFirst(ClaimTypes.Name);
                    
                var userName = nameClaim.Value;
                // use the name variable as needed
           
            try
            {
                var formCollection = await Request.ReadFormAsync();
                var file = Request.Form.Files[0];
                // Generate a unique file name based on the sender's ID and the current date/time
                var fileName = userName + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + file.FileName;
                string folderName = "Gallery/" + userName + "_" + SenderId + "/Books";


                var uploadFolder = Path.Combine(_env.ContentRootPath, folderName);


                // Generate the file path based on the sender's ID
                // var filePath = "/Gallery/to/Books/storage/" + SenderId + "/" + fileName;
                //  var directoryPath = Path.GetDirectoryName(filePath);
                
                // Create the upload folder if it doesn't exist
                if (!Directory.Exists(uploadFolder))
                {
                    Directory.CreateDirectory(uploadFolder);
                }
                var filePath = Path.Combine(uploadFolder, fileName);

                // Save the uploaded file to the server's local storage
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Generate the link for the uploaded file
                //var link = "http://Book.com/files/" + file.FileName;
                var bookName = Path.GetFileNameWithoutExtension(file.FileName);

                // Create a new instance of the Messages class and set its properties
                var message = new Messages
                {
                    EnrollmentDate = DateTime.Now,
                    Message = bookName,
                    SenderId = SenderId,
                    RecieverId = SenderId,    
                    FileLink = filePath // Set the FileLink property to store the link
                };

                // Save the new message to the database
                this.repository_message.Add(message);
                this.repository_message.Save();
                var successMessage = $"The file '{file.FileName}' has been successfully uploaded and stored.";


                return Ok(new { Message = successMessage });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
   
        }

        private static async Task<byte[]> ReadAsByteArrayAsync(Stream stream)
        {
            using var memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }

        public void gett()
        {
            //  var userClient = new TwitterClient("CONSUMER_KEY", "CONSUMER_SECRET", "ACCESS_TOKEN", "ACCESS_TOKEN_SECRET");
        }

       

       
    }

    public class loginSession
    {
        public string Email { get; set; }
        public string PasswordHash { get; set; }

        public string SenderId { get; set; }

        public string FirstName { get; set; }

        public bool success { get; set; }
    }
}
