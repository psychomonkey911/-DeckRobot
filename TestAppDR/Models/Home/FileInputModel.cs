using Microsoft.AspNetCore.Http;

namespace TestAppDeckRobot.Models.Home
{
    public class FileInputModel
    {
        public IFormFile FileToUpload { get; set; }
    }
}
