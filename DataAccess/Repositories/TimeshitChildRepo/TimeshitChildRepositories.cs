using DataAccess.Data;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories.TimeshitChildRepo
{
    public class TimeshitChildRepositories :ITimeShitChildRepositories
    {
        private readonly GonbadDbContext _GonbadDbContext;

        public TimeshitChildRepositories(GonbadDbContext gonbadDbContext)
        {
            _GonbadDbContext = gonbadDbContext;
        }

        public async Task Add(TimeShitChild timeShitChild)
        {
            _GonbadDbContext.TimeShitChilds.Add(timeShitChild);
            await _GonbadDbContext.SaveChangesAsync();


        }

        public async Task Delete(TimeShitChild timeShitChild)
        {
            _GonbadDbContext.TimeShitChilds.Remove(timeShitChild);
            await _GonbadDbContext.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var temp =await GetByid(id);
            _GonbadDbContext.TimeShitChilds.Remove(temp);
            await _GonbadDbContext.SaveChangesAsync();

        }

        public IQueryable<TimeShitChild> GetAll(Expression<Func<TimeShitChild, bool>> where = null) 
        {
            var data= _GonbadDbContext.TimeShitChilds.AsQueryable();
            if (where != null)
            {
                data=data.Where(where); 
            }

            return data;
        }

        public async Task<TimeShitChild> GetByid(int id)
        {
            return await _GonbadDbContext.TimeShitChilds.FirstOrDefaultAsync(a => a.Id == id);
        }



        public async Task Update(TimeShitChild timeShitChild)
        {
            _GonbadDbContext.TimeShitChilds.Update(timeShitChild);
            await _GonbadDbContext.SaveChangesAsync();
        }
    }
}
