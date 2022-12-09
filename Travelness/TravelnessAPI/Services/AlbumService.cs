using AutoMapper;
using DataAccessLayer.Entities;
using DataAccessLayer.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TravelnessAPI.Interfaces;
using TravelnessAPI.Models.Album;
using TravelnessAPI.Utils;

namespace TravelnessAPI.Services
{
    public class AlbumService : IAlbumService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public AlbumService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<Response<Tuple<Album, int>>> GetById(int id, string search, int page, int pageSize)
        {
            try
            {
                Func<Sightseeing, bool> searchFilter = x => true;
                if(search != null)
                {
                    searchFilter = x => x.Name.ToLower().Contains(search.ToLower());
                }
                var album = unitOfWork.Albums.Get(x => x.Id == id).Include(x => x.User).Include(x => x.Sightseeings).First();
                var count = album.Sightseeings.Where(searchFilter).Count();
                album.Sightseeings = album.Sightseeings.Where(searchFilter).Skip((page - 1) * pageSize).Take(pageSize).ToList();
                
                return new Response<Tuple<Album, int>>(new Tuple<Album, int>(album, count));
            }
            catch (Exception exception)
            {
                return new Response<Tuple<Album, int>>(exception.Message);
            }
        }

        public async Task<Response<IEnumerable<Album>>> GetUserAlbums(int userId, int sightseeingId)
        {
            try
            {
                var albums = await unitOfWork.Albums.Get(x => x.User.Id == userId).Include(x => x.Sightseeings.Where(x => x.Id == sightseeingId)).OrderByDescending(x => x.Id).ToListAsync();
                return new Response<IEnumerable<Album>>(albums);
            }
            catch (Exception exception)
            {
                return new Response<IEnumerable<Album>>(exception.Message);
            }
        }

        public async Task<Response<string>> AddSightseeing(int id, int sightseeingId, int userId)
        {
            try
            {
                var album = unitOfWork.Albums.Get(x => x.Id == id).Include(x => x.Sightseeings).First();
                if (album.UserId == userId)
                {
                    var sightseeing = unitOfWork.Sightseeings.GetById(sightseeingId);
                    if (!album.Sightseeings.Where(x => x.Id == sightseeingId).Any())
                    {
                        album.Sightseeings.Add(sightseeing);
                    }

                    unitOfWork.Albums.Update(album);
                    await unitOfWork.CommitAsync();

                    return new Response<string>(data: "Success");
                }
                else
                {
                    return new Response<string>("Invalid user id");

                }
            }
            catch (Exception exception)
            {
                return new Response<string>(exception.Message);
            }
        }

        public async Task<Response<string>> RemoveSightseeing(int id, int sightseeingId, int userId)
        {
            try
            {
                var album = unitOfWork.Albums.Get(x => x.Id == id).Include(x => x.Sightseeings).First();
                if (album.UserId == userId)
                {
                    var sightseeing = unitOfWork.Sightseeings.GetById(sightseeingId);
                    if (album.Sightseeings.Where(x => x.Id == sightseeingId).Any())
                    {
                        album.Sightseeings.Remove(sightseeing);
                    }

                    unitOfWork.Albums.Update(album);
                    await unitOfWork.CommitAsync();

                    return new Response<string>(data: "Success");
                }
                else
                {
                    return new Response<string>("Invalid user id");

                }
            }
            catch (Exception exception)
            {
                return new Response<string>(exception.Message);
            }
        }

        public async Task<Response<string>> Create(AlbumCreateViewModel model, int userId)
        {
            try
            {
                var album = mapper.Map<Album>(model);
                album.UserId = userId;
                unitOfWork.Albums.Insert(album);
                await unitOfWork.CommitAsync();

                return new Response<string>(data: "Succes");
            }
            catch (Exception exception)
            {
                return new Response<string>(exception.Message);
            }
        }

        public async Task<Response<Album>> Edit(AlbumEditViewModel model, int userId)
        {
            try
            {
                var album = unitOfWork.Albums.Get(x => x.Id == model.Id).First();
                if (album.UserId == userId)
                {
                    album.Name = model.Name;
                    unitOfWork.Albums.Update(album);
                    await unitOfWork.CommitAsync();
                    return new Response<Album>(album);
                }
                else
                {
                    return new Response<Album>("Invalid user id");

                }
            }
            catch (Exception exception)
            {
                return new Response<Album>(exception.Message);
            }
        }

        public async Task<Response<string>> Delete(int id, int userId)
        {
            try
            {
                var album = unitOfWork.Albums.Get(x => x.Id == id).First();
                if (album.UserId == userId)
                {
                    unitOfWork.Albums.Delete(album);
                    await unitOfWork.CommitAsync();
                    return new Response<string>(data: "Success");
                }
                else
                {
                    return new Response<string>("Invalid user id");

                }
            }
            catch (Exception exception)
            {
                return new Response<string>(exception.Message);
            }
        }
    }
}
