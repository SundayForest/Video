using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VideoDomain.Entity;
using VideoDomain.Service;
using VideoDomain.Service.ServiceInterface;
using VideoDomain.Value;

namespace VideoWebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class FileController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly IFileService fileService;

        public FileController(IUserService userService, IFileService fileService)
        {
            this.userService = userService;
            this.fileService = fileService;
        }

        [HttpPost]
        public async Task<ActionResult<TheFile?>> UpLoadFileAsync([FromForm] IFormFile[] files) {
            if (files.Length == 2)
            {
                var file1 = files[0];
                var file2 = files[1];
                string file1Name = file1.FileName;
                string file2Name = file2.FileName;
                using Stream s1 = file1.OpenReadStream();
                using Stream s2 = file2.OpenReadStream();
                string username = User.FindFirst(ClaimTypes.Name)!.Value;
                var auth = await userService.FindUserAsync(username);
                return Ok(await fileService.SaveFileAsync(s1, s2, auth!, file1Name, file2Name));
            }
            else {
                return BadRequest("require one img and one video");
            }
        }
        [HttpPost]
        public async Task<ActionResult<TheFile>> UpdateFileInfo(string? filename, string filehash, string? des, List<Tag> tags) {
            var user = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var file = await fileService.UpdateFileInfoAsync(filehash, tags, des, filename, user);
            if (file != null)
            {
                return Ok(file);
            }
            return BadRequest("you are not auth or not file");

        }
        [HttpGet]
        public async Task<ActionResult<List<Tag>>> FindAllTags() {
            return Ok(await fileService.FindAllTagAsync());
        }
        [HttpGet]
        public async Task<ActionResult<Page<List<TheFile>>>> PageFileWithTagOrder(int tagId,int index,int num,DayType dayType) {
            return Ok(await fileService.PageFileAsync(index,num,tagId,dayType));
        }
    }
}
