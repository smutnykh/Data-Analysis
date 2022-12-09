using DataAccessLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.UnitOfWork
{
    public interface IUnitOfWork
    {
        IUserRepository Users { get; }
        ISightseeingRepository Sightseeings { get; }
        ITourRepository Tours { get; }
        IRateRepository Rates { get; }
        IAlbumRepository Albums { get; }

        void Commit();
        Task CommitAsync();
        void Rollback();
    }
}
