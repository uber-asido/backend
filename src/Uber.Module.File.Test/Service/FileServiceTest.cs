using FluentAssertions;
using System;
using System.Threading.Tasks;
using Uber.Module.File.Abstraction.Model;
using Xunit;

namespace Uber.Module.File.Test.Service
{
    [Collection(FileTestCollection.Name)]
    public class FileServiceTest : FileTestBase
    {
        private static readonly byte[] csvData = EmbeddedResource.GetBytes("Film_Locations_in_San_Francisco.csv");

        public FileServiceTest(FileFixture fixture) : base(fixture) { }

        [Fact]
        public void CanQuery()
        {
            FileService.QueryHistory().Should().NotBeNull();
        }

        [Fact]
        public async Task CanQuerySingle()
        {
            FileService.QueryHistorySingle(Guid.NewGuid()).Should().BeEmpty();

            var result = await FileService.ScheduleForProcessing(new Abstraction.Service.File("file.csv", csvData));
            result.Succeeded.Should().BeTrue();
            FileService.QueryHistorySingle(result.Value.Key).Should().HaveCount(1);
        }

        [Fact]
        public async Task ScheduleForProcessingFailsOnUnsupportedFile()
        {
            var result = await FileService.ScheduleForProcessing(new Abstraction.Service.File("file.blah", csvData));
            result.Succeeded.Should().BeFalse();
            result.Errors.Should().HaveCount(1);
        }

        [Fact]
        public async Task CanScheduleAndFinishProcessing()
        {
            var file = new Abstraction.Service.File("file.csv", csvData);
            var result = await FileService.ScheduleForProcessing(file);
            result.Succeeded.Should().BeTrue();

            var history = result.Value;
            history.Key.Should().NotBe(default(Guid));
            history.Status.Should().Be(UploadStatus.Pending);
            history.Filename.Should().Be(file.Filename);
            history.Errors.Should().BeEmpty();
            history.Timestamp.Should().BeCloseTo(DateTimeOffset.UtcNow, 1000);

            await WaitForProcessingFinish(history.Key);

            var found = await FileService.FindHistory(history.Key);
            found.Key.Should().Be(history.Key);
            found.Status.Should().Be(UploadStatus.Done);
            found.Filename.Should().Be(history.Filename);
            found.Errors.Should().BeEmpty();
            history.Timestamp.Should().Be(history.Timestamp);
        }

        private Task WaitForProcessingFinish(Guid historyKey) => WaitFor(async () =>
            {
                var h = await FileService.FindHistory(historyKey);
                return h.Status == UploadStatus.Done;
            });

        private async Task WaitFor(Func<Task<bool>> action)
        {
            var start = DateTime.UtcNow;

            while (!await action())
            {
                await Task.Delay(50);
                (DateTime.UtcNow - start).Should().BeLessOrEqualTo(TimeSpan.FromSeconds(10), because: "Wait timeout");
            }
        }
    }
}
