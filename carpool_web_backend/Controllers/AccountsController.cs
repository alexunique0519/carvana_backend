using carpool_web_backend.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace carpool_web_backend.Controllers
{
    [RoutePrefix("api/accounts")]
    public class AccountsController : BaseApiController
    {
        [Authorize(Roles = "Admin")]
        [Route("users")]
        public HttpResponseMessage GetUsers()
        {
            return Request.CreateResponse(HttpStatusCode.OK, this.AppUserManager.Users.ToList().Select(u => this.TheModelFactory.Create(u)));

        }

        //[Authorize]
        [Route("UserId/{name}")]
        public HttpResponseMessage GetUserId(string name)
        {
            ApplicationUser user = null;
            if (!string.IsNullOrEmpty(name))
            {
                user = this.AppUserManager.FindByName(name);
                if(user != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, user.Id);
                }
            }

            return Request.CreateResponse(HttpStatusCode.BadRequest, "The user not exsits");

        }

        [Route("driverInfo/{id:guid}")]
        public HttpResponseMessage GetDriverInfo(string id)
        {
            ApplicationUser user = null;


            if (!string.IsNullOrEmpty(id))
            {
                user = this.AppUserManager.Users.FirstOrDefault(u => u.Id == id);
               ReturnDriverModel model = new ReturnDriverModel();
                model.Email = user.Email;
                model.phoneNumber = user.PhoneNumber;
                model.FullName = user.FirstName + " " + user.LastName;
               return Request.CreateResponse(HttpStatusCode.OK, model);

            }

            return Request.CreateResponse(HttpStatusCode.BadRequest, "Your request userId is invalid");    
         
        }

        //[Authorize(Roles = "Admin")]
        [Route("users/{id:guid}")]
        public async Task<HttpResponseMessage> GetUser(string id)
        {
            var user = await this.AppUserManager.FindByIdAsync(id);

            if(user != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK ,this.TheModelFactory.Create(user));
            }

            return Request.CreateResponse(HttpStatusCode.NotFound);
        }

        [AllowAnonymous]
        [Route("create")]
        public async Task<HttpResponseMessage> CreateUser(CreateUserModel userModel)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "The user information is not correct.");
            }

            var user = new ApplicationUser()
            {
                FirstName = userModel.FirstName,
                LastName = userModel.LastName,
                UserName = userModel.UserName,
                UserType = userModel.UserType,
                JoinDate = DateTime.Now.Date,
                Email = userModel.Email
            };

            IdentityResult result = IdentityResult.Success;
            try
            {
                result = await this.AppUserManager.CreateAsync(user, userModel.Password);
            }
            catch (Exception ex)
            {
                string a = ex.InnerException.Message;
            }

           

            if (!result.Succeeded)
            {
                //need to add later
                //return GetErrorResult(result);
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }

            return Request.CreateResponse(HttpStatusCode.OK);

        }

        [Authorize]
        [Route("ChangePassword")]
        public async Task<HttpResponseMessage> ChangePassword(ChangePasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                //consider add additional text message
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            IdentityResult result = await this.AppUserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);

            if (!result.Succeeded)
            {
                //return GetErrorResult(result);
                return null;
            }

            return Request.CreateResponse(HttpStatusCode.OK, "The new password has been set successfully.");
        }

        [Authorize(Roles = "Admin")]
        [Route("user/{id:guid}")]
        public async Task<HttpResponseMessage> DeleteUser(string id)
        {
            //Only Admin can delete users (Later when implement roles)
            var appUser = await this.AppUserManager.FindByIdAsync(id);

            if (appUser != null)
            {
                IdentityResult result = await this.AppUserManager.DeleteAsync(appUser);

                if (!result.Succeeded)
                {
                    //return GetErrorResult(result);
                    return null;
                }

                return Request.CreateResponse(HttpStatusCode.OK, "The user has been deleted.");

            }

            return Request.CreateResponse(HttpStatusCode.NotFound, "The user doesn't exist");

        }

        [Authorize(Roles = "Admin")]
        [Route("user/{id:guid}/roles")]
        [HttpPut]
        public async Task<IHttpActionResult> AssignRolesToUser([FromUri] string id, [FromBody] string[] rolesToAssign)
        {
            var appUser = await this.AppUserManager.FindByIdAsync(id);

            if (appUser == null)
            {
                return NotFound();
            }

            var currentRoles = await this.AppUserManager.GetRolesAsync(appUser.Id);

            var rolesNotExists = rolesToAssign.Except(this.AppRoleManager.Roles.Select(x => x.Name)).ToArray();

            if (rolesNotExists.Count() > 0)
            {
                ModelState.AddModelError("", string.Format("Roles '{0}' does not exixts in the system", string.Join(",", rolesNotExists)));
                return BadRequest(ModelState);
            }

            IdentityResult removeResult = await this.AppUserManager.RemoveFromRolesAsync(appUser.Id, currentRoles.ToArray());

            if (!removeResult.Succeeded)
            {
                ModelState.AddModelError("", "Failed to remove user roles");
                return BadRequest(ModelState);
            }

            IdentityResult addResult = await this.AppUserManager.AddToRolesAsync(appUser.Id, rolesToAssign);

            if (!addResult.Succeeded)
            {
                ModelState.AddModelError("", "Failed to add user roles");
                return BadRequest(ModelState);
            }

            return Ok();
        }

    }

    

}
