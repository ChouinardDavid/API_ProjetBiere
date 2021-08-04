using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProjetBiere.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetBiere.Repository
{
    internal class BiereRepository : IBiereRepository
    {
        private readonly ProjetBiereContext _context;

        public BiereRepository(ProjetBiereContext context)
        {
            this._context = context;
        }

        public async Task<Biere> GetBiere(int biereId)
        {
            var biere = await _context.Bieres.FirstOrDefaultAsync(b => b.Id == biereId);
            return biere;
        }

        public async Task<IEnumerable<Biere>> GetBiere()
        {
            var biere = _context.Bieres;
            return biere;
        }

        public async Task<Biere> Post(Biere biere)
        {
            //TODO: Passer directement la biere?
            _context.Add(biere);
            await _context.SaveChangesAsync();
            return await GetBiere(biere.Id);
        }

        public async Task<Biere> Update(Biere biere)
        {
            _context.Update(biere);
            await _context.SaveChangesAsync();
            return await GetBiere(biere.Id);
        }

        public async Task<int> Delete(Biere biere)
        {
            _context.Remove(biere);
            return await _context.SaveChangesAsync();
        }
    }
}