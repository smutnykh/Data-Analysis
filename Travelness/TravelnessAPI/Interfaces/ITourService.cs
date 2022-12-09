using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TravelnessAPI.Models.Tour;
using TravelnessAPI.Models.User;
using TravelnessAPI.Utils;

namespace TravelnessAPI.Interfaces
{
    public interface ITourService
    {
        Task<Response<IEnumerable<Tour>>> GetAll();
        Task<Response<TourResponseViewModel>> GetById(int id);
        Task<Response<string>> Create(int id, TourCreateViewModel model);
        Task<Response<string>> Edit(int userId, TourEditViewModel model);
        Task<Response<string>> Delete(int userId, int tourId);
    }
}
