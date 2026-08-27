using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Data;
using DataAccess.Models;

namespace DataAccess.Repositories.InfoKhademRepo
{
    public class InfoKademRepositories: IInfoKademRepositories
    {
        private readonly GonbadDbContext _GonbadDbContext;

        public InfoKademRepositories(GonbadDbContext _gonbadDbContext)
        {
            _GonbadDbContext = _gonbadDbContext;
        }

        public IQueryable<InfoKhadem> GetAll(Expression<Func<InfoKhadem, bool>> where = null)
        {
            var data = _GonbadDbContext.InfoKhadems.AsQueryable();
            if (where != null)
            {
                data = data.Where(where);
            }

            return data;
        }

        public async Task<InfoKhadem> GetByid(int id)
        {
            return await _GonbadDbContext.InfoKhadems.FindAsync(id);
        }

        public async Task Add(InfoKhadem infoKhadem)
        {
            _GonbadDbContext.Add(infoKhadem);
            await _GonbadDbContext.SaveChangesAsync();
        }

        public async Task Update(InfoKhadem infoKhadem)
        {
            _GonbadDbContext.Update(infoKhadem);
            await _GonbadDbContext.SaveChangesAsync();
        }

        public async Task Delete(InfoKhadem infoKhadem)
        {
            _GonbadDbContext.Remove(infoKhadem);
            await _GonbadDbContext.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var data = GetByid(id);
            _GonbadDbContext.Remove(data);
            await _GonbadDbContext.SaveChangesAsync();
        }
    }
}
