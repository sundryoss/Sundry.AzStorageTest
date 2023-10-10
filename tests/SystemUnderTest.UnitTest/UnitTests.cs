using FakeItEasy;
using SystemUnderTest.Interface;

namespace SystemUnderTest.UnitTest
{
    public class UnitTests
    {
        private readonly StringWriter Output = new();
        public UnitTests()
        {
            Console.SetOut(Output);
        }
        [Fact]
        public async Task File_Upload_Suceess()
        {
            var azBlobService = A.Fake<IAzBlobService>();
            A.CallTo(() => azBlobService.UploadFileToAzBlobAsync(A<string>._)).Returns(true);
            await Program.UploadFileToAzBlobAsync(azBlobService);
            Assert.Contains("File uploaded successfully", Output.ToString());
        }
        [Fact]
        public async Task File_Upload_Failure()
        {
            var azBlobService = A.Fake<IAzBlobService>();
            A.CallTo(() => azBlobService.UploadFileToAzBlobAsync(A<string>._)).Returns(false);
            await Program.UploadFileToAzBlobAsync(azBlobService);
            Assert.Contains("File upload failed", Output.ToString());
        }
    }
}