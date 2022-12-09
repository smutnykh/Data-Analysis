using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Interfaces
{
    public interface ISightseeingRepository : IRepository<Sightseeing>
    {
        Sightseeing GetByIdWithTours(int id);
    }
}
