using FluentAssertions;
using System;
using System.Text;
using System.Threading.Tasks;
using Uber.Module.File.Abstraction.Model;
using Xunit;

namespace Uber.Module.File.Test.Service
{
    [Collection(FileTestCollection.Name)]
    public class FileServiceTest : FileTestBase
    {
        private static readonly byte[] csvData = Encoding.UTF8.GetBytes(@"
Title,Release Year,Locations,Fun Facts,Production Company,Distributor,Director,Writer,Actor 1,Actor 2,Actor 3
180,2011,Epic Roasthouse (399 Embarcadero),,SPI Cinemas,,Jayendra,""Umarji Anuradha, Jayendra, Aarthi Sriram, &Suba "",Siddarth,Nithya Menon,Priya Anand
180, 2011, Mason & California Streets(Nob Hill),, SPI Cinemas,, Jayendra, ""Umarji Anuradha, Jayendra, Aarthi Sriram, & Suba "", Siddarth, Nithya Menon, Priya Anand
");

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

            var result = await FileService.ScheduleForProcessing(new Abstraction.Service.File("test.csv", csvData));
            result.Succeeded.Should().BeTrue();
            FileService.QueryHistorySingle(result.Result.Key).Should().HaveCount(1);
        }

        [Fact]
        public async Task CanScheduleAndFinishProcessing()
        {
            var result = await FileService.ScheduleForProcessing(new Abstraction.Service.File("test.csv", csvData));
            result.Succeeded.Should().BeTrue();

            var history = result.Result;
            history.Key.Should().NotBe(default(Guid));
            history.Status.Should().Be(UploadStatus.Ongoing);
            history.Filename.Should().Be("test.csv");
            history.Error.Should().BeNull();
            history.Timestamp.Should().BeCloseTo(DateTimeOffset.UtcNow, 1000);

            await WaitForProcessingFinish(history.Key);

            var found = await FileService.FindHistory(history.Key);
            found.Key.Should().Be(history.Key);
            found.Status.Should().Be(UploadStatus.Success);
            found.Filename.Should().Be(history.Filename);
            found.Error.Should().BeNull();
            history.Timestamp.Should().Be(history.Timestamp);
        }

        private Task WaitForProcessingFinish(Guid historyKey) => WaitFor(async () =>
            {
                var h = await FileService.FindHistory(historyKey);
                return h.Status != UploadStatus.Ongoing;
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
