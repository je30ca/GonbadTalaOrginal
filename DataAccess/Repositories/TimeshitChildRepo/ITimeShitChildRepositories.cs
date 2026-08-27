using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Models;

namespace DataAccess.Repositories.TimeshitChildRepo
{
    public interface ITimeShitChildRepositories
    {
        IQueryable<TimeShitChild> GetAll(Expression<Func<TimeShitChild,bool>> where=null);
        Task<TimeShitChild> GetByid(int id);

        Task Add(TimeShitChild timeShitChild);

        Task Update(TimeShitChild timeShitChild);

        Task Delete(TimeShitChild timeShitChild);

        Task Delete(int id);

    }
}
