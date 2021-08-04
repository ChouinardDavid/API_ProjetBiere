using ProjetBiere.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetBiere.Services
{
    public interface IBiereService
    {
        Task<Biere> GetBiere(int biereId);
        Task<IEnumerable<Biere>> GetBiere();
        Task<Biere> Post(Biere biere);
        Task<Biere> Update(Biere biereSource, Biere biereDest);
        Task<bool> Delete(Biere biere);
    }
}
