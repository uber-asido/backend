using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Uber.Core;
using Uber.Core.OData;
using Uber.Module.File.Abstraction.Service;

namespace Uber.Module.File.Api.OData
{
    public class FileController : UberODataController
    {
        private readonly IFileService fileService;

        public FileController(IFileService fileService)
        {
            this.fileService = fileService;
        }

        [HttpPost()]
        [ODataRoute("UploadFile")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            var data = file.OpenReadStream().ReadToEnd();
            var result = await fileService.ScheduleForProcessing(new Abstraction.Service.File(file.FileName, data));

            if (result.Succeeded)
                return Ok(result.Value);

            if (result.Errors.Any())
                ModelState.AddModelError(nameof(file), string.Join('\n', result.Errors));

            return ODataBadRequest();
        }
    }
}
