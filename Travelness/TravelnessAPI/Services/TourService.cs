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
using TravelnessAPI.Models.Sightseeing;
using TravelnessAPI.Models.Tour;
using TravelnessAPI.Utils;

namespace TravelnessAPI.Services
{
    public class TourService : ITourService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public TourService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<Response<IEnumerable<Tour>>> GetAll()
        {
            try
            {
                var tours = await unitOfWork.Tours.Get().OrderByDescending(x => x.Id).ToListAsync();

                return new Response<IEnumerable<Tour>>(tours);
            }
            catch (Exception exception)
            {
                return new Response<IEnumerable<Tour>>(exception.Message);
            }
        }

        public async Task<Response<TourResponseViewModel>> GetById(int id)
        {
            try
            {
                var entity = unitOfWork.Tours.GetByIdWithSightseeings(id);
                var tour = mapper.Map<TourResponseViewModel>(entity);

                return new Response<TourResponseViewModel>(tour);
            }
            catch (Exception exception)
            {
                return new Response<TourResponseViewModel>(exception.Message);
            }
        }

        public async Task<Response<string>> Create(int id, TourCreateViewModel model)
        {
            try
            {
                var user = unitOfWork.Users.GetById(id);
                var tour = mapper.Map<Tour>(model);
                tour.User = user;
                foreach(var item in model.SightseeingsIds)
                {
                    var sightseeing = unitOfWork.Sightseeings.GetById(item);
                    tour.Sightseeings.Add(sightseeing);
                }
                unitOfWork.Tours.Insert(tour);
                await unitOfWork.CommitAsync();

                return new Response<string>(true, string.Empty);
            }
            catch (Exception exception)
            {
                return new Response<string>(exception.Message);
            }
        }

        public async Task<Response<string>> Edit(int userId, TourEditViewModel model)
        {
            try
            {
                var tour = unitOfWork.Tours.GetByIdWithSightseeings(model.Id);
                if (tour.UserId == userId)
                {
                    tour.Name = model.Name;
                    tour.Description = model.Description;
                    tour.Price = model.Price;
                    tour.Sightseeings.Clear();
                    foreach (var item in model.SightseeingsIds)
                    {
                        var sightseeing = unitOfWork.Sightseeings.GetById(item);
                        tour.Sightseeings.Add(sightseeing);
                    }
                    unitOfWork.Tours.Update(tour);
                    await unitOfWork.CommitAsync();

                    return new Response<string>(true, string.Empty);
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
        public async Task<Response<string>> Delete(int userId, int tourId)
        {
            try
            {
                var tour = unitOfWork.Tours.Get(x => x.Id == tourId, null, "User").First();
                if(tour.UserId == userId)
                {
                    unitOfWork.Tours.Delete(tour);
                    await unitOfWork.CommitAsync();
                    return new Response<string>(true, string.Empty);
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
