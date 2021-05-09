using DataLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Auth_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _securityManager;
        private readonly SignInManager<ApplicationUser> _loginManager;
        public AccountController(UserManager<ApplicationUser> secMgr, SignInManager<ApplicationUser> loginManager)
        {
            _securityManager = secMgr;
            _loginManager = loginManager;

        }
        [HttpPost("Register")]
        public async Task<ActionResult<Register>> Register(Register model)
        {
            using (var context = new AppDbContext())
            {
                if (ModelState.IsValid)
                {
                    var user = new ApplicationUser
                    {
                        UserName = model.Name,
                        Email = model.Email,
                        Address = model.Address,
                        PhoneNumber = model.Phone
                    };
                    var result = await _securityManager.CreateAsync(user, model.Password);

                   
                    var identity_result = result.Errors.ToList();

                    if (identity_result.Count > 0)
                    {
                        return BadRequest(identity_result);
                    }

                    if (result.Succeeded)
                    {
                        await _securityManager.AddToRoleAsync(user, "User");
                        
                        
                        return Ok(user);

                    }


                }
            }

            return BadRequest();
        }
        [HttpPost("Login")]
        public async Task<ActionResult<Login>> Login(Login model)
        {
            
            
            if (ModelState.IsValid)
            {
                var result = await _loginManager.PasswordSignInAsync(model.UserName, model.Password, false, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    // var user = await this._securityManager.GetUserAsync(User);
                    var appuser = await UserObj(model.UserName);
                    var role = await _securityManager.GetRolesAsync(appuser);
                    var mainrole = role[0];
                    var token = "";
                    if(role[0]=="ADMIN" || role[0]=="Admin")
                    {
                      token = new TokenService().Generate(true, "Admin");
                    }
                  else
                        token = new TokenService().Generate(true, "User");
                    return Ok(new { token,mainrole });
                }
                if (!result.Succeeded)
                {
                    
                    return BadRequest("Invalid Credentials");
                }


            }
            return BadRequest();
        }
        [HttpGet("IsLogin/{username}")]
        [Authorize]
        public async Task<ActionResult> isLogin(string username)
        {
            /*  var flag = _loginManager.IsSignedIn(User);
              if(flag)
              {
                  var user = await _securityManager.GetUserAsync(User);

                  return Ok(user);
              }
              return Ok();*/
            if (username == null)
                return BadRequest();
            var user = await UserObj(username);
            return Ok(new { user.Address, user.PhoneNumber });
        }
        [HttpGet("Logout")]
        public async Task<IActionResult> Logout()
        {
            await _loginManager.SignOutAsync();
            return Ok();
        }
        public async Task<ApplicationUser> UserObj(string username)
        {
            var user = await _securityManager.FindByNameAsync(username);
            return user;
        }
    }
}
