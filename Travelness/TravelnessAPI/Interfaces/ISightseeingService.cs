using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TravelnessAPI.Models.Sightseeing;
using TravelnessAPI.Utils;

namespace TravelnessAPI.Interfaces
{
    public interface ISightseeingService
    {
        Task<Response<int>> Count();
        Task<Response<IEnumerable<Sightseeing>>> GetAll();
        Task<Response<Tuple<IEnumerable<Sightseeing>, int>>> GetByPage(string[] countries, string[] areas, string search, int page, int perPage);
        Task<Response<Sightseeing>> GetById(int id);
        Task<Response<Tuple<SightseeingShowViewModel, object>>> GetByIdWithTours(int id, string[] companies, double minPrice, double maxPrice, string order, int page, int pageSize);
        Task<Response<string>> Create(SightseeingCreateEditViewModel model);
        Task<Response<string>> Edit(SightseeingCreateEditViewModel model);
        Task<Response<string>> Delete(int id);
    }
}
