using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetCoreJwtAuth.Entities;
using NetCoreJwtAuth.Services;

namespace NetCoreJwtAuth.Controllers {
  [Authorize]
  [Route("api/[controller]")]
  [ApiController]
  public class UsersController : ControllerBase {
    readonly IAuthService authService;
    private readonly IUserService userService;

    public UsersController(IAuthService authService, IUserService userService) {
      this.authService = authService;
      this.userService = userService;
    }

    [AllowAnonymous]
    [HttpPost("authenticate")]
    public IActionResult Authenticate([FromBody]AppUser user) {
      var foundUser = authService.Authenticate(user.UserName, user.Password);
      if (foundUser is null) {
        return BadRequest(new { message = "ユーザー名もしくはパスワードは不正です" });
      }
      return Ok(foundUser);
    }

    [HttpGet("{id}")]
    public IActionResult GetAll(int id) {
      var user = userService.GetById(id);
      return Ok(user);
    }
  }
}
