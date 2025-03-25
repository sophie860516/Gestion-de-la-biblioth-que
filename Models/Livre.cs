using System.ComponentModel.DataAnnotations;



namespace ExempleEnvoiDonneesVue.Models
{
    public class Livre
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Titre { get; set; }
        [Required]
        [StringLength(50)]
        public string Categorie { get; set; }
    }
}
