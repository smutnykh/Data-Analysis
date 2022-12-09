using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace TravelnessAPI.Managers
{
    public static class FileManager
    {
        public static void SaveImage(IFormFile picture, string filename, bool isProfileImage)
        {
            if (!(picture.ContentType == "image/png" || picture.ContentType == "image/jpeg" || picture.ContentType == "image/bmp" || picture.ContentType == "image/gif"))
                throw new ArgumentException("Error, given file is not a .jpeg, .png, .gif or .bmp image.");

            //string imageDirectory = "/ClientApp/src/assets/images/profile";
            string filePath = "default";
            //var domain = AppDomain.CurrentDomain.BaseDirectory;
            if (isProfileImage)
                filePath = "D:/Thesis/Travelness/TravelnessAPI/ClientApp/public/images/profile/" + filename;
            else
                filePath = "D:/Thesis/Travelness/TravelnessAPI/ClientApp/public/images/sightseeing/" + filename;
            using (var stream = File.Create(filePath))
            {
                picture.CopyTo(stream);
            }
        }

        public static string GetUniqueFilename(string filename)
        {
            var guid = Guid.NewGuid();
            filename = Path.GetFileNameWithoutExtension(filename) + "-" + guid.ToString() + Path.GetExtension(filename);
            return filename;
        }
    }
}
