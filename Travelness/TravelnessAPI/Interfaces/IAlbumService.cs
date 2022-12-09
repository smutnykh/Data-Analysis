using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TravelnessAPI.Models.Album;
using TravelnessAPI.Utils;

namespace TravelnessAPI.Interfaces
{
    public interface IAlbumService
    {
        Task<Response<Tuple<Album, int>>> GetById(int id, string search, int page, int pageSize);
        Task<Response<IEnumerable<Album>>> GetUserAlbums(int userId, int sightseeingId);
        Task<Response<string>> AddSightseeing(int id, int sightseeingId, int userId);
        Task<Response<string>> RemoveSightseeing(int id, int sightseeingId, int userId);
        Task<Response<string>> Create(AlbumCreateViewModel model, int userId);
        Task<Response<Album>> Edit(AlbumEditViewModel model, int userId);
        Task<Response<string>> Delete(int id, int userId);
    }
}
