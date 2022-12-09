using AutoMapper;
using DataAccessLayer.Entities;
using DataAccessLayer.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TravelnessAPI.Interfaces;
using TravelnessAPI.Managers;
using TravelnessAPI.Models.Album;
using TravelnessAPI.Models.User;
using TravelnessAPI.Utils;

namespace TravelnessAPI.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<Response<IEnumerable<User>>> GetAll()
        {
            try
            {
                var users = await unitOfWork.Users.Get().ToListAsync();
                return new Response<IEnumerable<User>>(users);
            }
            catch (Exception exception)
            {
                return new Response<IEnumerable<User>>(exception.Message);
            }
        }

        public async Task<Response<UserResponseViewModel>> GetById(int id)
        {
            try
            {
                var userQuery = await unitOfWork.Users.Get(x => x.Id == id).Include(x => x.Tours.OrderByDescending(x => x.Id)).ThenInclude(x => x.Sightseeings).FirstAsync();
                if (userQuery.Role == Role.Admin)
                    throw new Exception("Admin page");

                var user = mapper.Map<UserResponseViewModel>(userQuery);
                var albums = unitOfWork.Albums.Get(x => x.User.Id == id).OrderByDescending(x => x.Id).Include(x => x.Sightseeings).ToList();
                foreach(var album in albums)
                {
                    user.Albums.Add(new AlbumForUserViewModel { 
                        Id = album.Id,
                        Name = album.Name,
                        Image = album.Sightseeings.FirstOrDefault() == null ? null: album.Sightseeings.FirstOrDefault().Image
                    });

                }

                return new Response<UserResponseViewModel>(user);
            }
            catch (Exception exception)
            {
                return new Response<UserResponseViewModel>(exception.Message);
            }
        }

        public async Task<Response<UserResponseViewModel>> Edit(int id, UserEditViewModel model)
        {
            try
            {
                var user = unitOfWork.Users.GetById(id);
                user.Username = model.Username;
                user.ProfileInfo = model.ProfileInfo;
                if (model.ProfileImage != null)
                {
                    ImageManager.DeleteUserImage(user);
                }
                ImageManager.SaveUserImage(user, model.ProfileImage);
                unitOfWork.Users.Update(user);
                await unitOfWork.CommitAsync();

                return new Response<UserResponseViewModel>(mapper.Map<UserResponseViewModel>(user));
            }
            catch (Exception exception)
            {
                return new Response<UserResponseViewModel>(exception.Message);
            }
        }
    }
}
