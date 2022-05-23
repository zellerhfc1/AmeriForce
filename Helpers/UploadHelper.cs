using AmeriForce.Data;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace AmeriForce.Helpers
{
    public class UploadHelper
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string webRootPath;

        public UploadHelper(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            webRootPath = _webHostEnvironment.WebRootPath;
        }


        /// <summary>
        /// Create a directory
        /// If it exists, it will not be created
        /// </summary>
        /// <param name="id"></param>
        public void CreatedocumentDirectory(string id, string folderName)
        {
            string virtualPath = $"Documents/{folderName}/{id}";
            var path = Path.Combine(webRootPath, virtualPath);
            if (Directory.Exists(path))
            {
                return;     //Content("Directory exists");
            }
            else
            {
                DirectoryInfo di = Directory.CreateDirectory(path);
                return;     //Content(String.Format("The directory was created successfully at {0}.", Directory.GetCreationTime(path).ToString()));
            }
        }


        /// <summary>
        /// Create a directory
        /// If it exists, it will not be created
        /// </summary>
        /// <param name="id"></param>
        public void CreatedocumentDirectory(string folderName)
        {
            string virtualPath = $"{folderName}";
            var path = Path.Combine(webRootPath, virtualPath);
            if (Directory.Exists(path))
            {
                return;     //Content("Directory exists");
            }
            else
            {
                DirectoryInfo di = Directory.CreateDirectory(path);
                return;     //Content(String.Format("The directory was created successfully at {0}.", Directory.GetCreationTime(path).ToString()));
            }
        }

        public bool FileExists(string fileNameWithPathAndExtension)
        {
            var wwwroot = _webHostEnvironment.WebRootPath;
            var systemFilePath = Path.Combine(wwwroot, fileNameWithPathAndExtension);

            return System.IO.File.Exists(systemFilePath);
        }


    }
}
