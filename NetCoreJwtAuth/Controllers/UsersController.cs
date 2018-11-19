using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetCoreJwtAuth.Entities;
using NetCoreJwtAuth.Services;

namespace NetCoreJwtAuth.Controllers {
  [Authorize]
  [Route("api/[controller]")]
  [ApiController]
  public class UsersController : ControllerBase {
    readonly IUserService userService;

    public UsersController(IUserService userService) {
      this.userService = userService;
    }

    [AllowAnonymous]
    [HttpPost("authenticate")]
    public IActionResult Authenticate([FromBody]AppUser user) {
      var foundUser = userService.Authenticate(user.UserName, user.Password);
      if (foundUser is null) {
        return BadRequest(new { message = "ユーザー名もしくはパスワードは不正です" });
      }
      return Ok(foundUser);
    }

    [HttpGet]
    public IActionResult GetAll() {
      var users = userService.GetAll();
      return Ok(users);
    }
  }
}
