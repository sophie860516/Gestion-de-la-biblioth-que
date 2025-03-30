using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ExempleEnvoiDonneesVue.Models;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using System.Collections;


namespace ExempleEnvoiDonneesVue.Controllers
{
    public class LivreController : Controller
    {
        string chaineConnexion = @"Data Source = (LocalDb)\MSSQLLocalDB;Initial Catalog=bdgplcc;Integrated Security=true";
        // GET: LivresController
        public IActionResult Index()
        {
            DataTable tablLivres = new DataTable();
            using (SqlConnection conn = new SqlConnection(chaineConnexion))
            {
                conn.Open();
                SqlDataAdapter adp = new SqlDataAdapter("SELECT * FROM Livres", conn);
                adp.Fill(tablLivres);
            }
            return View(tablLivres);
        }
        // GET: LivresController/Details/5
        public IActionResult Details(int id)
        {
            Livre? livre = null;

            using (SqlConnection connexion = new SqlConnection(chaineConnexion))
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
       


        // GET: LivresController/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: LivresController/Create
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Livre livre)
        {
            try
            {

                    using (SqlConnection conn = new SqlConnection(chaineConnexion))
                    {
                    
                    
                            conn.Open();
                            string query = "INSERT INTO Livres(titre,idcateg, annee, nom_auteur, exemplaires) VALUES (@Titre, @idcateg,@annee,@nom_auteur, @exemplaires)";
 
                            SqlCommand cmd = new SqlCommand(query, conn);
                            
                            //cmd.Parameters.AddWithValue("@Id", livre.id);
                            cmd.Parameters.AddWithValue("@Titre", livre.titre);
                            cmd.Parameters.AddWithValue("@idcateg", livre.idcateg);
                            cmd.Parameters.AddWithValue("@annee", livre.annee);
                            cmd.Parameters.AddWithValue("@nom_auteur", livre.nom_auteur);
                            cmd.Parameters.AddWithValue("@exemplaires", livre.exemplaires);
                            cmd.ExecuteNonQuery();


                    }

                    return RedirectToAction("Index"); // Redirect to the book list after adding

            }

            catch
            {
                Console.WriteLine("erreur, impossible d'ajouter le livre"); 
                return View(livre);
            }
            
        }

        // GET: LivresController/Edit/5
        public IActionResult Edit(int id)
        {
            Livre livre = null;

            using (SqlConnection conn = new SqlConnection(chaineConnexion))
            {   
                
                string query = "SELECT * FROM Livres WHERE Id = @Id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", id);
                conn.Open();


                SqlDataReader lecteur = cmd.ExecuteReader();
                if (lecteur.Read())
                {
                    livre = new Livre
                    {
                        id = (int)lecteur["id"],
                        titre = lecteur["titre"].ToString(),
                        idcateg = (int)lecteur["idcateg"]
                    };
                }
            }

            return View(livre);  // Return the view with the current Livre data
        }

        // POST: LivresController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Livre livre)
        {
            Console.WriteLine("test 0");
            if (id != livre.id)
            {
                Console.WriteLine("test 1");
                return NotFound();
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(chaineConnexion))
                {
                    conn.Open();

                    string query = "UPDATE Livres SET Titre = @Titre, idcateg= @idcateg WHERE Id = @Id";
                    Console.WriteLine("test ");

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Id",id);
                    cmd.Parameters.AddWithValue("@Titre", livre.titre);
                    cmd.Parameters.AddWithValue("@idcateg", livre.idcateg);
                    

                    // Execute the update query
                    int rowsAffected = cmd.ExecuteNonQuery();
                    Console.WriteLine("Rows Affected: " + rowsAffected);
                    if (rowsAffected > 0)
                    {
                        // Successfully updated
                        return RedirectToAction("Index");  // Redirect back to the list
                    }
                    else
                    {
                        // If no rows were updated, the record might not exist.
                        return NotFound();
                    }
                    Console.WriteLine("fin");
                }
            }
            catch{ 
                return View(livre);
            }
            
        }

        // GET: LivresController/Delete/5
        public IActionResult Delete(int id)
        {
            Livre? livre = null;
            using (SqlConnection connexion = new SqlConnection(chaineConnexion))
            {
                string requete = "SELECT * FROM Livres WHERE Id = @Id";
                SqlCommand cmd = new SqlCommand(requete, connexion);
                cmd.Parameters.AddWithValue("@Id", id);

                connexion.Open();
                SqlDataReader lecteur = cmd.ExecuteReader();
                if (lecteur.Read())
                {
                    livre = new Livre
                    {
                        id = (int)lecteur["Id"],
                        titre = lecteur["Titre"].ToString(),
                        idcateg = (int)lecteur["idcateg"]
                    };
                }
            }
            if (livre == null)
            {
                return NotFound();
            }
            return View(livre);
        }

        // POST: LivresController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id, Livre livre)
        {
            try
            {
                using (SqlConnection connexion = new SqlConnection(chaineConnexion))
                {
                    string requete = "DELETE FROM Livres WHERE Id = @Id";
                    SqlCommand cmd = new SqlCommand(requete, connexion);
                    cmd.Parameters.AddWithValue("@Id", id);

                    connexion.Open();
                    cmd.ExecuteNonQuery();
                }
                return RedirectToAction("Index");
            }
            catch
            {
                // Gérer l'exception ou afficher un message d'erreur
                return View();
            }
        }
        
    }
}
