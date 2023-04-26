using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VideoDomain.Entity;
using VideoDomain.Service;
using VideoDomain.Service.ServiceInterface;
using VideoInfrastructure;

namespace VideoWebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly VideoDbContext videoDbContext;

        public LoginController(IUserService userService, VideoDbContext videoDbContext)
        {
            this.userService = userService;
            this.videoDbContext = videoDbContext;
        }
        [HttpGet]

        public async Task<ActionResult<string>> Login(string username,string pwd) { 
            var res =  await userService.LoginAsync(username, pwd);
            if (res == null) {
                return BadRequest("登录失败");
            }
            return Ok(res);
        }
        [HttpPost]
        public async Task<ActionResult> Register(string username,string pwd)
        {
            var res = await userService.RegisterAsync(username,pwd);
            if(res == null)
            {
                return BadRequest("该用户名已经被注册");
            }
            return Ok();
        }
    }
}
