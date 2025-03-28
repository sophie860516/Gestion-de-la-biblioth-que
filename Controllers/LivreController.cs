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
        public IActionResult DetailsLivres()
        {
            Livre livre = new Livre { Id = 1, Titre = "test"};
            // Envoi de l'objet Film à la vue
            return View(livre);
        }
        // GET: LivresController/Details/5
        public IActionResult Details(int id)
        {
            return View();
        }

        private readonly string _connectionString;


        // GET: LivresController/Create
        public IActionResult Create()
        {
            return View(new Livre());
        }

        // POST: LivresController/Create
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Livre livre)
        {
            try
            {


                if (ModelState.IsValid)
                {
                    using (SqlConnection conn = new SqlConnection(chaineConnexion))
                    {
                    
                    
                            conn.Open();
                            string query = "INSERT INTO Livres VALUES ( @Id,@Titre, @Categorie)";
 
                            SqlCommand cmd = new SqlCommand(query, conn);
                            //cmd.Parameters.AddWithValue("@Id", livre.Id);
                            cmd.Parameters.AddWithValue("@Id", livre.Id);
                            cmd.Parameters.AddWithValue("@Titre", livre.Titre);
                            cmd.Parameters.AddWithValue("@Categorie", livre.Categorie);
                            cmd.ExecuteNonQuery();


                    }

                    return RedirectToAction("Index"); // Redirect to the book list after adding
                }
                else
                {
                    // If model is invalid, return to the Create view with error messages
                    return View(livre);
                }

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
                        Id = (int)lecteur["Id"],
                        Titre = lecteur["Titre"].ToString(),
                        Categorie = lecteur["Categorie"].ToString()
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
            if (id != livre.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                using (SqlConnection conn = new SqlConnection(chaineConnexion))
                {
                    conn.Open();

                    string query = "UPDATE Livres SET Titre = @Titre, Categorie = @Categorie WHERE Id = @Id";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Titre", livre.Titre);
                    cmd.Parameters.AddWithValue("@Categorie", livre.Categorie);
                    cmd.Parameters.AddWithValue("@Id", livre.Id);

                    // Execute the update query
                    int rowsAffected = cmd.ExecuteNonQuery();

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
                }
            }

            return View(livre);
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
                        Id = (int)lecteur["Id"],
                        Titre = lecteur["Titre"].ToString(),
                        Categorie = lecteur["Categorie"].ToString()
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
