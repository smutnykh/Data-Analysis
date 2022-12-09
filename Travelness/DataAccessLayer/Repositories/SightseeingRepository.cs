using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class SightseeingRepository : Repository<Sightseeing>, ISightseeingRepository
    {
        public SightseeingRepository(TravelnessContext context) : base(context)
        {
        }

        public Sightseeing GetByIdWithTours(int id)
        {
            return dbSet.Where(x => x.Id == id).Include(x => x.Tours.OrderByDescending(x => x.Id)).ThenInclude(x => x.Sightseeings).
                Include(x => x.Tours).ThenInclude(x => x.User).First();
        }
    }
}
