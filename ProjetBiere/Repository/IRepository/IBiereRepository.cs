using ProjetBiere.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjetBiere.Repository
{
    public interface IBiereRepository
    {
        Task<Biere> GetBiere(int biereId);
        Task<IEnumerable<Biere>> GetBiere();
        Task<Biere> Post(Biere biere);
        Task<Biere> Update(Biere biere);
        Task<int> Delete(Biere biere);
    }
}