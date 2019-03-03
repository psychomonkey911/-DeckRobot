using System.Collections.Generic;

namespace TestAppDeckRobot.Models.Home
{
    public class FileDetails
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string FontName { get; set; }
        public string FontSize { get; set; }

    }

    public class FilesViewModel
    {
        public List<FileDetails> Files { get; set; } 
            = new List<FileDetails>();
    }
}
