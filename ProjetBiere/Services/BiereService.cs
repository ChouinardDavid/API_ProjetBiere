using AutoMapper;
using ProjetBiere.Entity;
using ProjetBiere.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetBiere.Services
{
    public class BiereService : IBiereService
    {
        private readonly IBiereRepository _biereRepository;
        private readonly IMapper _mapper;

        public BiereService(IBiereRepository biereRepository, IMapper mapper)
        {
            this._biereRepository = biereRepository;
            this._mapper = mapper;
        }

        public async Task<IEnumerable<Biere>> GetBiere()
        {
            return await _biereRepository.GetBiere();
        }

        public async Task<Biere> GetBiere(int biereId)
        {
            return await _biereRepository.GetBiere(biereId);
        }

        public async Task<Biere> Post(Biere biere)
        {
            return await _biereRepository.Post(biere);
        }

        public async Task<Biere> Update(Biere biereSource, Biere biereDest)
        {
            return await _biereRepository.Update(MergeBiere(biereSource, biereDest));
        }

        public async Task<bool> Delete(Biere biere)
        {
            return (await _biereRepository.Delete(biere) > 0);
        }

        //EntityFramework ne permet pas de copier un object qui est suivi
        private Biere MergeBiere(Biere biereSource, Biere biereDestination)
        {
            biereDestination.Id = biereSource.Id;
            biereDestination.IBU = biereSource.IBU;
            biereDestination.ABV = biereSource.ABV;
            biereDestination.Nom = biereSource.Nom;
            biereDestination.Saisonniere = biereSource.Saisonniere;
            biereDestination.SRM = biereSource.SRM;
            biereDestination.Style = biereSource.Style;
            return biereDestination;
        }
    }
}
