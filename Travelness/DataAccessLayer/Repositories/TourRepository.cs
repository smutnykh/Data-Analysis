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
    public class TourRepository : Repository<Tour>, ITourRepository
    {
        public TourRepository(TravelnessContext context) : base(context)
        {

        }
        public Tour GetByIdWithSightseeings(int id)
        {
            return dbSet.Where(x => x.Id == id).Include(x => x.User).Include(x => x.Sightseeings.OrderByDescending(x => x.Id)).
                    ThenInclude(x => x.Tours).First();
        }
    }
}
