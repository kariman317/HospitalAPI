using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Hospital.Management.System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    // [Authorize(Roles="Admin")]
    public class RoleController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> roleManager;

        public RoleController(RoleManager<IdentityRole> roleManager)
        {
            this.roleManager = roleManager;
        }
        [HttpPost]
        public async Task<IActionResult> Add(string RoleName)
        {
            if (RoleName != null)
            {
                IdentityRole identityRole = new IdentityRole();
                identityRole.Name = RoleName;
                IdentityResult result = await roleManager.CreateAsync(identityRole);
                if (result.Succeeded)
                {
                    return Ok("Role created Successfully");
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
            return BadRequest("Error in RoleName");
        }
    }
}
