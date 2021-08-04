using ProjetBiere.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetBiere.Modeles
{
    public class BiereModele
    {
        [Key]
        public int Id { get; set; }
        public string Nom { get; set; }
        public double ABV { get; set; }
        [Display(Name = "")]
        public Style Style { get; set; }
        public int SRM { get; set; }
        public int IBU { get; set; }
        public int Saisonniere { get; set; }

    }
}
