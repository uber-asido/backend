using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Uber.Core;
using Uber.Module.File.Abstraction.Model;
using Uber.Module.File.Abstraction.Service;
using Uber.Module.File.Abstraction.Store;
using Uber.Module.File.BackgroundJob;
using Uber.Module.File.FileProcessor;
using Uber.Module.Geocoding.Abstraction.Service;
using Uber.Module.Movie.Abstraction.Service;

namespace Uber.Module.File.Service
{
    public class FileService : IFileService
    {
        private readonly IGeocodingService geocodingService;
        private readonly IMovieService movieService;
        private readonly IUploadHistoryStore historyStore;
        private readonly IBackgroundJobClient backgroundJobClient;

        public FileService(IGeocodingService geocodingService, IMovieService movieService, IUploadHistoryStore historyStore, IBackgroundJobClient backgroundJobClient)
        {
            this.geocodingService = geocodingService;
            this.movieService = movieService;
            this.historyStore = historyStore;
            this.backgroundJobClient = backgroundJobClient;
        }

        public IQueryable<UploadHistory> QueryHistory() => historyStore.Query();
        public IQueryable<UploadHistory> QueryHistorySingle(Guid key) => historyStore.QuerySingle(key);

        public Task<UploadHistory> FindHistory(Guid key) => historyStore.Find(key);

        public async Task<OperationResult<UploadHistory>> ScheduleForProcessing(Abstraction.Service.File file)
        {
            var processor = ProcessorResolver.Resolve(file.Filename);
            if (processor == null)
                return OperationResult<UploadHistory>.Failed("The file format is not supported.");

            var history = new UploadHistory
            {
                Key = Guid.NewGuid(),
                Filename = file.Filename,
                Status = UploadStatus.Pending,
                Timestamp = DateTimeOffset.UtcNow,
                Errors = new string[0]
            };
            await historyStore.Create(history, file.Data);

            backgroundJobClient.Enqueue<FileJob>(job => job.ProcessFile(history.Key));

            return OperationResult<UploadHistory>.Success(history);
        }

        internal async Task ProcessFile(Guid uploadHistoryKey)
        {
            UploadHistory history = null;
            try
            {
                history = await historyStore.Find(uploadHistoryKey);

                var processor = ProcessorResolver.Resolve(history.Filename);
                if (processor == null)
                    throw new Exception("The file format is not supported.");

                var data = await historyStore.FindFileData(uploadHistoryKey);
                var parseResult = processor.Parse(data);
                var movies = parseResult.Movies.Where(e => e.FilmingLocations.Any());
                var mergeTasks = new List<Task>();

                foreach (var movie in movies)
                {
                    var geocodeTasks = new List<Task>();
                    foreach (var location in movie.FilmingLocations)
                    {
                        async Task doGeocode()
                        {
                            var geocode = await geocodingService.Geocode(location.FormattedAddress);
                            location.AddressKey = geocode.Key;
                            location.FormattedAddress = geocode.FormattedAddress;
                            location.Latitude = geocode.Latitude;
                            location.Longitude = geocode.Longitude;
                        }
                        geocodeTasks.Add(doGeocode());
                    }
                    await Task.WhenAll(geocodeTasks);

                    mergeTasks.Add(movieService.Merge(movie));
                }

                await Task.WhenAll(mergeTasks);

                history.Status = UploadStatus.Done;
                history.Errors = parseResult.Errors.ToArray();
                await historyStore.Update(history);
            }
            catch (Exception ex)
            {
                if (history != null)
                {
                    history.Status = UploadStatus.Done;
                    history.Errors = new[] { ex.Message };
                    await historyStore.Update(history);
                }
            }
        }
    }
}
