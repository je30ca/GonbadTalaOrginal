using DataAccess.Data;
using DataAccess.Models;
using DataAccess.Repositories.childRepo;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DataAccess.Repositories.ChildRepo
{
    public class ChildRepositories : IChildRepositories
    {
        private readonly GonbadDbContext _GonbadDbContext;

        public ChildRepositories(GonbadDbContext gonbadDbContext)
        {
            _GonbadDbContext= gonbadDbContext;
        }
        public async Task Add(InfoChild infochild)
        {
            _GonbadDbContext.Add(infochild);
            await _GonbadDbContext.SaveChangesAsync();
        }

        public async Task Delete(InfoChild infochild)
        {
            _GonbadDbContext.InfoChilds.Remove(infochild);
            await _GonbadDbContext.SaveChangesAsync();

        }

        public async Task Delete(int id)
        {
            var data = await GetByid(id);
             _GonbadDbContext.InfoChilds.Remove(data);
            await _GonbadDbContext.SaveChangesAsync();
        }

        public async Task ExitTime(InfoChild infochild)
        {
            _GonbadDbContext.Update(infochild);
            await _GonbadDbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<InfoChild>> GetAll()
        {
            var data=await _GonbadDbContext.InfoChilds.ToListAsync();
            return data;
        }

        public async Task<InfoChild> GetByid(int id)
        {
            return await  _GonbadDbContext.InfoChilds.FindAsync(id);
        }

        public async Task Update(InfoChild infochild)
        {
            _GonbadDbContext.Update(infochild);
            await _GonbadDbContext.SaveChangesAsync();
        }
    }
}
