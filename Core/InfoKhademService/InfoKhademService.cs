using DataAccess.Repositories.TimeshitChildRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Repositories.InfoKhademRepo;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace Core.InfoKhademService
{
    public class InfoKhademService
    {
        private readonly IInfoKademRepositories _InfoKademRepositories;

        public InfoKhademService(IInfoKademRepositories infoKademRepositories)
        {
            _InfoKademRepositories = infoKademRepositories;

        }
        public async Task<IEnumerable<DataAccess.Models.InfoKhadem>> GetAllKhadem()
        {
            return await _InfoKademRepositories.GetAll().ToListAsync();
        }


        public async Task<DataAccess.Models.InfoKhadem> GetkhademById(int id)
        {
            return await _InfoKademRepositories.GetByid(id);
        }


        public async Task CreateKhadem(DataAccess.Models.InfoKhadem infoKhadem)
        {
            await _InfoKademRepositories.Add(infoKhadem);
        }

        public async Task UpdateKhadem(DataAccess.Models.InfoKhadem infoKhadem)
        {
            await _InfoKademRepositories.Update(infoKhadem);

        }
        public async Task DeleteKhadem(DataAccess.Models.InfoKhadem infoKhadem)
        {
            await _InfoKademRepositories.Delete(infoKhadem);

        }
    }
}
