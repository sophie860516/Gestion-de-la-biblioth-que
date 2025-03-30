using ExempleEnvoiDonneesVue.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ExempleEnvoiDonneesVue.Controllers
{
    public class MembreController : Controller
    {

        private readonly string? _chaineConnexion;


        public MembreController(IConfiguration configuration)
        {   // configuration.GetConnectionString est un diminutif pour
            // GetSection("ConnectionStrings")["BdFilmsConnectionString"]
            // du fichier appsettings.json
            _chaineConnexion = configuration.GetConnectionString("BdBiblioConnectionString");
        }

        // GET: MembreController
        public IActionResult Index()
        {
            DataTable tablLivres = new DataTable();
            using (SqlConnection conn = new SqlConnection(_chaineConnexion))
            {
                conn.Open();
                SqlDataAdapter adp = new SqlDataAdapter("SELECT * FROM Livres", conn);
                adp.Fill(tablLivres);
            }
            return View(tablLivres);
        }
        // GET: MembreController/Details/5
        public IActionResult Details(int id)
        {
            Livre? livre = null;

            using (SqlConnection connexion = new SqlConnection(_chaineConnexion))
            {
                string requete = "SELECT * FROM films WHERE id = @Id";
                var commande = new SqlCommand(requete, connexion);
                commande.Parameters.AddWithValue("@Id", id);

                connexion.Open();

                using (var lecteur = commande.ExecuteReader())
                {
                    if (lecteur.Read())
                    {
                        livre = new Livre
                        {
                            id = lecteur.GetInt32("Id"),
                            titre = lecteur.GetString("Titre"),
                            annee = lecteur.GetInt32("annee"),
                            nom_auteur = lecteur.GetString("nom_auteur"),
                            idcateg = lecteur.GetInt32("idcateg"),

                        };
                    }
                }
            }

            if (livre == null)
            {
                return NotFound();
            }

            return View(livre);
        }
        // GET: Emprunt
        // GET: Borrow/5 (id_livre)
        public IActionResult Emprunt(int id_livre)
        {
            // Fetch the livre details (optional, you could use it to show more info in the borrow form)
            Livre livre = null;
            using (SqlConnection conn = new SqlConnection(_chaineConnexion))
            {

                string query = "SELECT * FROM Livres WHERE Id = @Id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", id_livre);
                conn.Open();

                var reader = cmd.ExecuteReader();
                

                Console.WriteLine("Querying for id_livre: " + id_livre);

                if (reader.Read())
                {
                    
                    livre = new Livre
                    {
                        id = (int)reader["Id"],
                        titre = reader["Titre"].ToString(),
                        nom_auteur = reader["nom_auteur"].ToString()
                    };
                    
                }
               
                reader.Close();
            }

            if (livre == null)
            {
                return NotFound(); // If no livre found, return 404 error
            }

            Console.WriteLine("test2");
            Emprunt emprunt = new Emprunt
            {
                id_livre = livre.id,
                id_membre = 1,
                date_emprunt = DateTime.Now.Date,
                date_retour = null,
                date_retour_eff=null
            };
            // Pass livre to the view (to show livre details)
            ViewData["Livre"] = livre;
            
            return View(emprunt);
        }


        // POST: Emprunt
        // POST: Borrow/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Emprunt(Emprunt emprunt)
        {
            // Check if the membre has already borrowed 3 livres
            
            int borrowCount = GetBorrowedLivresCount(emprunt.id_membre);
            if (borrowCount >= 3)
            {
                ViewBag.ErrorMessage = "Vous pouvez emprunter jusqu'à 3 livres maximum.";
                return View(emprunt);  // Show error message if the membre tries to borrow more than 3 livres
            }

            //check disponibilité du livre
            if (CheckLivreDispo(emprunt.id_livre))
            {
                ViewBag.ErrorMessage = "Livre emprunté!";
                return View(emprunt);
            }
            /*
            DateTime date_emprunt = DateTime.Now.Date;
            */
            DateTime date_retour= DateTime.Now.Date.AddDays(30);
            

            using (SqlConnection conn = new SqlConnection(_chaineConnexion))
            {
                Console.WriteLine("test4");
                string query = "INSERT INTO Emprunt (id_membre, id_livre, date_emprunt, date_retour) VALUES (@id_membre, @id_livre, @date_emprunt, @date_retour)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id_membre", emprunt.id_membre);
                cmd.Parameters.AddWithValue("@id_livre", emprunt.id_livre);
                cmd.Parameters.AddWithValue("@date_emprunt", DateTime.Now.Date);
                cmd.Parameters.AddWithValue("@date_retour",date_retour);
                //cmd.Parameters.AddWithValue("@date_retour_eff", emprunt.date_retour_eff);
                conn.Open();
                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("Index");  // Redirect to book list or another page after successful borrow
        }

        // Method to check how many livres a membre has borrowed
        private int GetBorrowedLivresCount(int id_membre)
        {
            int borrowCount = 0;

            using (SqlConnection conn = new SqlConnection(_chaineConnexion))
            {
                string query = "SELECT COUNT(*) FROM Emprunt WHERE id_membre = @id_membre";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id_membre", id_membre);
                conn.Open();

                borrowCount = (int)cmd.ExecuteScalar();
            }

            return borrowCount;
        }

        private bool CheckLivreDispo(int id_livre)
        {
            // Check if the book is already borrowed
            bool isBookBorrowed = false;

            using (SqlConnection conn = new SqlConnection(_chaineConnexion))
            {
                string query = @"
                SELECT COUNT(*) 
                FROM Emprunt 
                WHERE id_livre = @id_livre
                AND (date_retour_eff IS NULL OR date_retour_eff  > GETDATE())";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id_livre", id_livre);
                conn.Open();

                int count = (int)cmd.ExecuteScalar();
                Console.WriteLine(count);
                if (count > 0)
                {
                    isBookBorrowed = true;
                }
            }

            return isBookBorrowed;

        }
        // GET: MembreController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MembreController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: MembreController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: MembreController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: MembreController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: MembreController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
