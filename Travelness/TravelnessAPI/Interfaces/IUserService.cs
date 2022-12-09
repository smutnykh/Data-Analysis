using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TravelnessAPI.Models.User;
using TravelnessAPI.Utils;

namespace TravelnessAPI.Interfaces
{
    public interface IUserService
    {
        Task<Response<IEnumerable<User>>> GetAll();
        Task<Response<UserResponseViewModel>> GetById(int id);
        Task<Response<UserResponseViewModel>> Edit(int id, UserEditViewModel model);
    }
}
