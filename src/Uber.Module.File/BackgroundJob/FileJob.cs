using System;
using System.Threading.Tasks;
using Uber.Module.File.Service;

namespace Uber.Module.File.BackgroundJob
{
    public class FileJob
    {
        private readonly FileService fileService;

        public FileJob(FileService fileService)
        {
            this.fileService = fileService;
        }

        public Task ProcessFile(Guid uploadHistoryKey) => fileService.ProcessFile(uploadHistoryKey);
    }
}
