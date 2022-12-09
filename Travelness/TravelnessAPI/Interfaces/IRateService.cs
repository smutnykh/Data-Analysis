using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TravelnessAPI.Utils;

namespace TravelnessAPI.Interfaces
{
    public interface IRateService
    {

        Task<Response<double>> GetSightseeingRate(int sightseeingId);
        Task<Response<byte>> GetUserRating(int sightseeingId, int userId);
        Task<Response<string>> Add(int sightseeingId, int userId, byte rating);
        Task<Response<string>> Remove(int sightseeingId, int userId);

    }
}
