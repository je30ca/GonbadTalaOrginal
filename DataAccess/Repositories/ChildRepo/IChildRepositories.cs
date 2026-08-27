using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DataAccess.Repositories.childRepo
{
    public interface IChildRepositories
    {
      Task<IEnumerable<InfoChild>> GetAll();

        Task<InfoChild> GetByid(int id);

        Task Add(InfoChild infochild);

        Task Update(InfoChild infochild);

        Task Delete(InfoChild infochild);

        Task ExitTime(InfoChild infochild);

        Task Delete(int id);

    }
}
