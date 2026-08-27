using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories.InfoKhademRepo
{
    public interface IInfoKademRepositories
    {
        IQueryable<InfoKhadem> GetAll(Expression<Func<InfoKhadem, bool>> where = null);

        Task<InfoKhadem> GetByid(int id);

        Task Add(InfoKhadem infoKhadem);

        Task Update(InfoKhadem infoKhadem);

        Task Delete(InfoKhadem infoKhadem);

        Task Delete(int id);
    }
}
