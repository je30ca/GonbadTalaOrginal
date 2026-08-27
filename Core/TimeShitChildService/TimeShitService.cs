using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Models;
using DataAccess.Repositories.TimeshitChildRepo;
using Microsoft.EntityFrameworkCore;

namespace Core.TimeShitChild
{
    public class TimeShitService
    {
        private readonly ITimeShitChildRepositories _TimeShitChildRepositories;

        public TimeShitService(ITimeShitChildRepositories timeshitChildRepo)
        {
            _TimeShitChildRepositories = timeshitChildRepo;

        }
        public async Task<IEnumerable<DataAccess.Models.TimeShitChild>> GetAllTimeShitChild()
        {
            return await  _TimeShitChildRepositories.GetAll().ToListAsync();
        }
        public async Task<IEnumerable<DataAccess.Models.TimeShitChild>> GetAllTimeShitChildWithInfo(Expression<Func<DataAccess.Models.TimeShitChild, bool>> where = null)
        {
            return await _TimeShitChildRepositories.GetAll(where).Include(a => a.InfoChild)
                .Include(a => a.InfoKhadem).ToListAsync();
        }

        public async Task<DataAccess.Models.TimeShitChild> GetTimeShitChildById(int id)
        {
            return await _TimeShitChildRepositories.GetByid(id);
        }


        public async Task CreateTimeShitChild(DataAccess.Models.TimeShitChild timeshch)
        {
            await _TimeShitChildRepositories.Add(timeshch);
        }

        public async Task UpdateTimeShitChild(DataAccess.Models.TimeShitChild timeshch)
        {
            await _TimeShitChildRepositories.Update(timeshch);

        }
        public async Task DeleteTimeShitChild(DataAccess.Models.TimeShitChild timeshch)
        {
            await _TimeShitChildRepositories.Delete(timeshch);

        }

    } 
}
