using chatApp.Controllers;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System;
using Microsoft.Extensions.Configuration;

namespace chatApp.Models
{
    public  class JWT
    {

        private IConfiguration _config;

        public JWT(IConfiguration config)
        {
            _config = config;
        }

        public Object GenerateJSONWebToken(ApplicationUser user)
        { 
             // keeps in your mind this token will only be generated if user is authenticated by its password and usermails
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            // for encryption...

            // var userRoles = await _userManager.GetRolesAsync(user);
            try
            {
                var authClaims = new List<Claim>

               {
               new Claim(ClaimTypes.Name, user.NormalizedUserName?? ""),
               new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
               new Claim("UserID", user.Id), // fetching from user that we traced from finduserByEmail
                };

                /*    foreach (var userRole in userRoles)
                 {
                  authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                  }*/

                var token = new JwtSecurityToken(
                _config["jwt:Issuer"],
                _config["jwt:Audience"],
                authClaims,
                expires: DateTime.Now.AddMinutes(680),
                signingCredentials: credentials);
                var jwt_token = new JwtSecurityTokenHandler().WriteToken(token);
                return new { token = jwt_token };
                // return Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
            }

            catch(Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }

        }
    }
}
