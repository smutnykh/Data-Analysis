using AutoMapper;
using DataAccessLayer.Entities;
using DataAccessLayer.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TravelnessAPI.Interfaces;
using TravelnessAPI.Managers;
using TravelnessAPI.Models.Sightseeing;
using TravelnessAPI.Utils;

namespace TravelnessAPI.Services
{
    public class SightseeingService : ISightseeingService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public SightseeingService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<Response<int>> Count()
        {
            try
            {
                var count = unitOfWork.Sightseeings.Count();
                return new Response<int>(count);
            }
            catch (Exception exception)
            {
                return new Response<int>(exception.Message);
            }
        }

        public async Task<Response<IEnumerable<Sightseeing>>> GetAll()
        {
            try
            {
                var sightseeings = await unitOfWork.Sightseeings.Get().OrderByDescending(x => x.Id).ToListAsync();
                return new Response<IEnumerable<Sightseeing>>(sightseeings);
            }
            catch (Exception exception)
            {
                return new Response<IEnumerable<Sightseeing>>(exception.Message);
            }
        }

        public async Task<Response<Tuple<IEnumerable<Sightseeing>, int>>> GetByPage(string[] countries, string[] areas, string search, int page, int pageSize)
        {
            try
            {
                Func<Sightseeing, bool> searchFilter = x => true;
                Func<Sightseeing, bool> countriesFilter = x => true;
                Func<Sightseeing, bool> areasFilter = x => true;

                if (search != null)
                    searchFilter = x => x.Name.ToLower().Contains(search.ToLower());
                if (countries.Length > 0)
                    countriesFilter = x => countries.Contains(x.Country);
                if (areas.Length > 0)
                    areasFilter = x => areas.Contains(x.Area);

                Func<Sightseeing, bool> filter = x => searchFilter(x) && countriesFilter(x) && areasFilter(x);

                var query = unitOfWork.Sightseeings.Get().Where(filter).OrderByDescending(x => x.Id);
                var count = query.Count();
                var sightseeings = query.Skip((page - 1) * pageSize).Take(pageSize);

                return new Response<Tuple<IEnumerable<Sightseeing>, int>>(new Tuple<IEnumerable<Sightseeing>, int>(sightseeings, count));
            }
            catch (Exception exception)
            {
                return new Response<Tuple<IEnumerable<Sightseeing>, int>>(exception.Message);
            }
        }

        public async Task<Response<Sightseeing>> GetById(int id)
        {
            try
            {
                var sightseeing = unitOfWork.Sightseeings.GetById(id);
                if (sightseeing == null)
                    throw new Exception("Invalid id");

                return new Response<Sightseeing> (sightseeing);
            }
            catch (Exception exception)
            {
                return new Response<Sightseeing>(exception.Message);
            }
        }

        public async Task<Response<Tuple<SightseeingShowViewModel, object>>> GetByIdWithTours(int id, string[] companies, double minPrice, double maxPrice, string order, int page, int pageSize)
        {
            try
            {
                Func<Tour, bool> authorsFilter = x => true;
                Func<Tour, bool> priceFilter = x => x.Price >= minPrice && x.Price <= maxPrice;
                if (companies.Length > 0)
                    authorsFilter = x => companies.Contains(x.User.Username);

                Func<Tour, bool> filter = x => authorsFilter(x) && priceFilter(x);

                var sightseeingQuery = unitOfWork.Sightseeings.GetByIdWithTours(id);

                var tourQuery = sightseeingQuery.Tours.Where(filter);
                var count = tourQuery.Count();

                List<string> allCompanies = new List<string>();
                foreach (var tour in sightseeingQuery.Tours)
                {
                    allCompanies.Add(tour.User.Username);
                }

                double min = 0, max = 0;

                if (count > 0)
                {
                    min = tourQuery.Min(x => x.Price);
                    max = tourQuery.Max(x => x.Price);
                }

                if (order == "none")
                    sightseeingQuery.Tours = tourQuery.OrderByDescending(x => x.Id).Skip((page - 1) * pageSize).Take(pageSize).ToList();
                else if(order == "ascending")
                    sightseeingQuery.Tours = tourQuery.OrderBy(x => x.Price).Skip((page - 1) * pageSize).Take(pageSize).ToList();
                else if (order == "descending")
                    sightseeingQuery.Tours = tourQuery.OrderByDescending(x => x.Price).Skip((page - 1) * pageSize).Take(pageSize).ToList();

                var sightseeing = mapper.Map<SightseeingShowViewModel>(sightseeingQuery);

                return new Response<Tuple<SightseeingShowViewModel, object>>(new Tuple<SightseeingShowViewModel, object>(sightseeing, 
                    new { count, min, max, allCompanies }));
            }
            catch (Exception exception)
            {
                return new Response<Tuple<SightseeingShowViewModel, object>>(exception.Message);
            }
        }

        public async Task<Response<string>> Create(SightseeingCreateEditViewModel model)
        {
            try
            {
                var sightseeing = mapper.Map<Sightseeing>(model);
                ImageManager.SaveSightseeingImage(sightseeing, model.Image);
                unitOfWork.Sightseeings.Insert(sightseeing);
                await unitOfWork.CommitAsync();
                return new Response<string>(true, string.Empty);
            }
            catch (Exception exception)
            {
                return new Response<string>(exception.Message);
            }
        }

        public async Task<Response<string>> Edit(SightseeingCreateEditViewModel model)
        {
            try
            {
                var sightseeing = unitOfWork.Sightseeings.GetById(model.Id);
                sightseeing.Name = model.Name;
                sightseeing.Description = model.Description;
                sightseeing.Country = model.Country;
                sightseeing.Area = model.Area;
                if (model.Image != null)
                {
                    ImageManager.DeleteSightseeingImage(sightseeing);
                }
                ImageManager.SaveSightseeingImage(sightseeing, model.Image);
                unitOfWork.Sightseeings.Update(sightseeing);
                await unitOfWork.CommitAsync();
                return new Response<string>(true, string.Empty);
            }
            catch (Exception exception)
            {
                return new Response<string>(exception.Message);
            }
        }

        public async Task<Response<string>> Delete(int id)
        {
            try
            {
                var sightseeing = unitOfWork.Sightseeings.GetById(id);
                ImageManager.DeleteSightseeingImage(sightseeing);
                unitOfWork.Sightseeings.Delete(sightseeing);
                await unitOfWork.CommitAsync();
                return new Response<string>(true, string.Empty);
            }
            catch (Exception exception)
            {
                return new Response<string>(exception.Message);
            }
        }
    }
}
