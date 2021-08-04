using AutoMapper;
using ProjetBiere.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetBiere.Modeles
{
    public class ProjetBiereProfile : Profile
    {
        public ProjetBiereProfile()
        {
            this.CreateMap<Biere, BiereModele>().ReverseMap();
        }
    }
}
