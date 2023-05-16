using Moq;
using SystemUnderTest.Interface;

namespace SystemUnderTest.UnitTest
{
    public class UnitTests
    {
       private readonly StringWriter Output = new ();
        public UnitTests()
        {
            Console.SetOut(Output);
        }
        [Fact]
        public async Task File_Upload_Suceess()
        {
            var azBlobService = new Mock<IAzBlobService>();

            azBlobService
                .Setup(x => x.UploadFileToAzBlobAsync(It.IsAny<string>()))
                .ReturnsAsync(true);

            await Program.UploadFileToAzBlobAsync(azBlobService.Object);

            Assert.Contains("File uploaded successfully", Output.ToString());
        }
        [Fact]
        public async Task File_Upload_Failure ()
        {
            var azBlobService = new Mock<IAzBlobService>();
            azBlobService
                .Setup(x => x.UploadFileToAzBlobAsync(It.IsAny<string>()))
                .ReturnsAsync(false);

            await Program.UploadFileToAzBlobAsync(azBlobService.Object);
            Assert.Contains("File upload failed", Output.ToString());
        }
    }
}