using AutoMapper;
using DataAccessLayer.Entities;
using DataAccessLayer.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TravelnessAPI.Interfaces;
using TravelnessAPI.Utils;

namespace TravelnessAPI.Services
{
    public class RateService : IRateService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public RateService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<Response<double>> GetSightseeingRate(int sightseeingId)
        {
            try
            {
                var query = unitOfWork.Rates.Get(x => x.Sightseeing.Id == sightseeingId);
                if(query.Count() > 0)
                    return new Response<double>(Math.Round(unitOfWork.Rates.Get(x => x.Sightseeing.Id == sightseeingId).Average(x => x.Rating), 1));
                else
                    return new Response<double>(0);
            }
            catch (Exception exception)
            {
                return new Response<double>(exception.Message);
            }
        }
        public async Task<Response<byte>> GetUserRating(int sightseeingId, int userId)
        {
            try
            {
                var query = unitOfWork.Rates.Get(x => x.UserId == userId && x.SightseeingId == sightseeingId);
                if (query.Any())
                    return new Response<byte>(query.First().Rating);
                else
                    return new Response<byte>(0);
            }
            catch (Exception exception)
            {
                return new Response<byte>(exception.Message);
            }
        }
        public async Task<Response<string>> Add(int sightseeingId, int userId, byte rating)
        {
            try
            {
                var query = unitOfWork.Rates.Get(x => x.UserId == userId && x.SightseeingId == sightseeingId);
                if(!query.Any())
                {
                    var user = unitOfWork.Users.GetById(userId);
                    var sightseeing = unitOfWork.Sightseeings.GetById(sightseeingId);
                    var rate = new Rate()
                    {
                        Rating = rating,
                        User = user,
                        Sightseeing = sightseeing
                    };
                    unitOfWork.Rates.Insert(rate);
                }
                else if(rating > 0)
                {
                    query.First().Rating = rating;
                }
                await unitOfWork.CommitAsync();

                return new Response<string>(data: "Success");
            }
            catch (Exception exception)
            {
                return new Response<string>(exception.Message);
            }
        }

        public async Task<Response<string>> Remove(int sightseeingId, int userId)
        {
            try
            {
                var rateQuery = unitOfWork.Rates.Get(x => x.UserId == userId && x.SightseeingId == sightseeingId);
                if (rateQuery.Any())
                {
                    unitOfWork.Rates.Delete(rateQuery.First());
                    await unitOfWork.CommitAsync();
                }

                return new Response<string>(data: "Success");
            }
            catch (Exception exception)
            {
                return new Response<string>(exception.Message);
            }
        }
    }
}
