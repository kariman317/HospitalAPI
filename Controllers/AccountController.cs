using Hospital.Management.System.DTO;
using Hospital.Management.System.Models;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Management.System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<Patient> userManager;
        private readonly IConfiguration configuration;
        private readonly SignInManager<Patient> signInManager;

        public AccountController(UserManager<Patient> userManager,
            IConfiguration configuration,
            SignInManager<Patient> signInManager )
        {
            this.userManager = userManager;
            this.configuration = configuration;
            this.signInManager = signInManager;
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDTO register)
        {
            var existUser = await userManager.FindByEmailAsync(register.Email);
            if (existUser != null)
            {
                return BadRequest("User Already Exists!");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Patient userModel = new Patient()
            {
                UserName = register.UserName,
                Email = register.Email,
                Age = register.Age,
                PatientHistory = register.PatientHistory,
            };
            IdentityResult result = await userManager.CreateAsync(userModel,register.Password);
            if (result.Succeeded)
            {
                return Ok("User Created Successfully");
            }
            else
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
                return BadRequest(ModelState);
            }
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDTO login)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Patient userModel = await userManager.FindByEmailAsync(login.Email);
            if (userModel != null)
            {
                if (await userManager.CheckPasswordAsync(userModel, login.Password))
                {
                    var claims = new List<Claim>();
                    claims.Add(new Claim(ClaimTypes.Email, userModel.Email));
                    claims.Add(new Claim(ClaimTypes.NameIdentifier, userModel.Id));
                    var roles = await userManager.GetRolesAsync(userModel);
                    foreach (var item in roles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, item));
                    }
                    claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:SecretKey"]));
                  
                    var token = new JwtSecurityToken(
                        audience: configuration["JWT:ValidAudience"],
                        issuer: configuration["JWT:ValidIssuer"],
                        expires: DateTime.Now.AddDays(3),
                        claims: claims,
                        signingCredentials:
                        new SigningCredentials
                        (key, SecurityAlgorithms.HmacSha256)
                        );
                    return Ok(new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(token),
                        expiration = token.ValidTo
                    });

                }
                return Unauthorized();
            }
            return BadRequest("User Name or Password Incorrect");
        }
        

        [HttpPost("RegisterAdmin")]
        [Authorize(Roles = "admin")]

        public async Task<IActionResult> RegisterAdmin(RegisterDTO register)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Patient userModel = new Patient()
            {
                UserName = register.UserName,
                Email = register.Email,
            };
            IdentityResult result = await userManager.CreateAsync(userModel, register.Password);
            if (result.Succeeded)
            {
                //add to role manager
                IdentityResult resultRole = await userManager.AddToRoleAsync(userModel, "admin");
                if (resultRole.Succeeded)
                {
                    //await signInManager.SignInAsync(userModel, false) ;
                    return Ok(userModel);
                }
                else
                {
                    return BadRequest("error");
                }

            }
            else
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
                return BadRequest(ModelState);
            }
        }







        ///forget password
        [HttpPost("ResetPasswordToken")]
        public async Task<IActionResult> ResetPasswordToken(ResetPasswordTokenDTO passwordToken)
        {
            var user = await userManager.FindByEmailAsync(passwordToken.Email);
            if (user == null)
            {
                return BadRequest("User does not Exists");
            }
            var token = await userManager.GeneratePasswordResetTokenAsync(user);

            var message = new MimeMessage();
            message.From.Add( MailboxAddress.Parse("kareman.17295774@compit.aun.edu.eg"));
            message.To.Add(MailboxAddress.Parse(user.Email));
            message.Subject="Token from API";
            message.Body = new TextPart()
            {
                Text = token
            };
            using(var client = new SmtpClient())
            {
                try
                {
                    client.Connect("smtp-mail.outlook.com", 587 , MailKit.Security
                        .SecureSocketOptions.StartTls);
                    client.Authenticate("kareman.17295774@compit.aun.edu.eg","124578@asd");

                    client.Send(message);
                    client.Disconnect(true);
                }
                catch(Exception ex)
                {

                    return BadRequest(ex.Message);
                }
                
            }
            return Ok("Token send Successfully");
        }


        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDTO resetPassword)
        {
            var user = await userManager.FindByEmailAsync(resetPassword.Email);
            if (user == null)
            {
                return BadRequest("User does not Exists");
            }
            if (string.Compare(resetPassword.NewPassword , resetPassword.ConfirmNewPassword) !=0)
            {
                return BadRequest("The New Password and Confirm Password does not Match !");
            }
            if (string.IsNullOrEmpty(resetPassword.Token))
            {
                return BadRequest("Invaild Token!");
            }
            var result = await userManager.ResetPasswordAsync(user, resetPassword.Token, resetPassword.NewPassword);
            if (result.Succeeded)
            {
                return Ok("Password Reseted Successfully");
            }
            else
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(",", item.Description);

                }
                return BadRequest(ModelState);
            }

        }





    }
}
