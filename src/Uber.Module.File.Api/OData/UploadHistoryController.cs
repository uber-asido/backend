using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using Uber.Core.OData;
using Uber.Module.File.Abstraction.Model;
using Uber.Module.File.Abstraction.Service;

namespace Uber.Module.File.Api.OData
{
    public class UploadHistoryController : UberODataController
    {
        private readonly IFileService fileService;

        public UploadHistoryController(IFileService fileService)
        {
            this.fileService = fileService;
        }

        [EnableQuery]
        [HttpGet]
        public IQueryable<UploadHistory> Get() => fileService.QueryHistory();

        [EnableQuery]
        [HttpGet]
        public IQueryable<UploadHistory> Get([FromODataUri] Guid key) => fileService.QueryHistorySingle(key);
    }
}
