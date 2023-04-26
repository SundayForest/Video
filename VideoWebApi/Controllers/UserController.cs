using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VideoDomain.Entity;
using VideoDomain.Service;
using VideoDomain.Service.ServiceInterface;

namespace VideoWebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly IFileService fileService;

        public UserController(IUserService userService, IFileService fileService)
        {
            this.userService = userService;
            this.fileService = fileService;
        }
        [HttpGet]
        public async Task<ActionResult<List<AuthInfo>>> FindAttentions(string username) { 
            var res = await userService.FindAttentionsAsync(username);
            if (res == null) {
                return BadRequest("no User");
            }
            return Ok(res);
        }
        [HttpGet]
        public async Task<ActionResult<List<TheFile>>> FindAuthFile(long authId) { 
            var list =  await fileService.FindAuthWorksAsync(authId);
            if(list.Count == 0)
            {
                return BadRequest("no file");
            }
            return Ok(list);
        }
    }
}
