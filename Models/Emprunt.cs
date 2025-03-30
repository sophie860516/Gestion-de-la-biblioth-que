namespace ExempleEnvoiDonneesVue.Models
{
    public class Emprunt
    {

        public int id { get; set; }
        public int id_livre { get; set; }
        public int id_membre { get; set; }
        public DateTime date_emprunt { get; set; }
        public DateTime? date_retour { get; set; }
        public DateTime? date_retour_eff { get; set; }
    }
}
