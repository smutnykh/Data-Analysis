using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace TravelnessAPI.Managers
{
    public static class ImageManager
    {
        public static void SaveUserImage(User user, IFormFile picture)
        {
            if (picture != null)
            {
                string pictureFilename = FileManager.GetUniqueFilename(picture.FileName);
                user.ProfileImage = pictureFilename;
                FileManager.SaveImage(picture, pictureFilename, true);
            }
        }

        public static void SaveSightseeingImage(Sightseeing sightseeing, IFormFile picture)
        {
            if (picture != null)
            {
                string pictureFilename = FileManager.GetUniqueFilename(picture.FileName);
                sightseeing.Image = pictureFilename;
                FileManager.SaveImage(picture, pictureFilename, false);
            }
        }

        public static void DeleteUserImage(User user)
        {
            if (File.Exists("D:/Thesis/Travelness/TravelnessAPI/ClientApp/public/images/profile/" + user.ProfileImage))
                File.Delete("D:/Thesis/Travelness/TravelnessAPI/ClientApp/public/images/profile/" + user.ProfileImage);
        }

        public static void DeleteSightseeingImage(Sightseeing sightseeing)
        {
            if (File.Exists("D:/Thesis/Travelness/TravelnessAPI/ClientApp/public/images/sightseeing/" + sightseeing.Image))
                File.Delete("D:/Thesis/Travelness/TravelnessAPI/ClientApp/public/images/sightseeing/" + sightseeing.Image);
        }


    }
}
