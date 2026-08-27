using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Models;
using DataAccess.Repositories.childRepo;
using DataAccess.Repositories.ChildRepo;

namespace Core.ChildInfoService
{
    public class ChildInfoService
    {
        private readonly IChildRepositories _childRepositories;

        public ChildInfoService(IChildRepositories childRepositories)
        {
            _childRepositories = childRepositories;

        }

        public async Task<IEnumerable<InfoChild>> GetAllchild()
        {
            return await _childRepositories.GetAll();
        }
        public async Task<InfoChild> GetchildById(int id)
        {
            return await _childRepositories.GetByid(id);
        }

        public async Task CreateChild(InfoChild infoChild)
        {
           await _childRepositories.Add(infoChild);
            
        }
        public async Task UpdateChild(InfoChild infoChild)
        {
            await _childRepositories.Update(infoChild);

        }
        public async Task DeleteChild(InfoChild infoChild)
        {
            await _childRepositories.Delete(infoChild);

        }
       

    }
}
