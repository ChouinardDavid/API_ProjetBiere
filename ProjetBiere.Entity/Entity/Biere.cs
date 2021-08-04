
namespace ProjetBiere.Entity
{
    public class Biere
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public double ABV { get; set; }
        public Style Style { get; set; }
        public int SRM { get; set; }
        public int IBU { get; set; }
        public int Saisonniere { get; set; }
    }
}

