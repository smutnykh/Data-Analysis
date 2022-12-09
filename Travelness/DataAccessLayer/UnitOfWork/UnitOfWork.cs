using DataAccessLayer.Interfaces;
using DataAccessLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TravelnessContext dbContext;
        public IUserRepository Users { get; }
        public ISightseeingRepository Sightseeings { get; }
        public ITourRepository Tours { get; }
        public IRateRepository Rates { get; }
        public IAlbumRepository Albums { get; }

        public UnitOfWork(TravelnessContext context, IUserRepository userRepository, ISightseeingRepository sightseeingsRepository, 
            ITourRepository tourRepository, IRateRepository rateRepository, IAlbumRepository albumRepository)
        {
            dbContext = context;
            Users = userRepository;
            Sightseeings = sightseeingsRepository;
            Tours = tourRepository;
            Rates = rateRepository;
            Albums = albumRepository;
        }

        public void Commit()
        {
            dbContext.SaveChanges();
        }

        public Task CommitAsync()
        {
            return dbContext.SaveChangesAsync();
        }

        public void Rollback()
        {
            dbContext.Dispose();
        }
    }
}
