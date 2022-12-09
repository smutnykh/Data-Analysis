using AutoMapper;
using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TravelnessAPI.Models;
using TravelnessAPI.Models.Album;
using TravelnessAPI.Models.Sightseeing;
using TravelnessAPI.Models.Tour;
using TravelnessAPI.Models.User;

namespace TravelnessAPI.Utils
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserRegisterViewModel, User>();
            CreateMap<UserEditViewModel, User>();
            CreateMap<User, UserResponseViewModel>();
            CreateMap<User, UserForTourViewModel>();

            CreateMap<SightseeingCreateEditViewModel, Sightseeing>();
            CreateMap<Sightseeing, SightseeingShowViewModel>();

            CreateMap<Tour, TourResponseViewModel>();
            CreateMap<Tour, TourForSightseeingViewModel>();
            CreateMap<TourCreateViewModel, Tour>();
            CreateMap<TourEditViewModel, Tour>();

            CreateMap<AlbumCreateViewModel, Album>();
            CreateMap<AlbumEditViewModel, Album>();
            CreateMap<Album, AlbumForUserViewModel>();
        }
    }
}
