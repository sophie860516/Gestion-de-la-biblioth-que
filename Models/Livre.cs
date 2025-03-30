using System.ComponentModel.DataAnnotations;



namespace ExempleEnvoiDonneesVue.Models
{
    public class Livre
    {
        
        public int id { get; set; }
        
       
        public string titre { get; set; }
        
        
        public int idcateg { get; set; }

        public int annee { get; set; }

        public string nom_auteur {  get; set; }
        public int exemplaires {  get; set; }

    }
}
