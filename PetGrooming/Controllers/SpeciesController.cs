using System;
using System.Collections.Generic;
using System.Data;
//required for SqlParameter class
using System.Data.SqlClient;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PetGrooming.Data;
using PetGrooming.Models;
using System.Diagnostics;

namespace PetGrooming.Controllers
{
    public class SpeciesController : Controller
    {
        private PetGroomingContext db = new PetGroomingContext();
        // GET: Species
        public ActionResult Index()
        {
            return View();
        }

        // GET: This method is for getting the list of Species
        public ActionResult List()
        {
            //Query to select the list of Species
            List<Species> myspecies = db.Species.SqlQuery("Select * from species").ToList();
            //Calling the Species List View to display the Species list
            return View(myspecies);
        }

        // GET: This method is used to display a specific Species based on the id
        public ActionResult Show(int? id)
        {
            //Checking if speciesid is null
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Query to get the species with the speciesid 'id'.
            Species species = db.Species.SqlQuery("select * from species where SpeciesID=@SpeciesID", new SqlParameter("@SpeciesID", id)).FirstOrDefault();
            if (species == null)
            {
                return HttpNotFound();
            }
            //Calling the Species show view to display the specific species
            return View(species);
        }

        //GET: Function to call the species view where user will enter the data.
        public ActionResult New()
        {  
            return View();
        }

        //POST: This method is used get the values entered in the form and update in the database.
        [HttpPost]
        public ActionResult New(string SpeciesName)
        {
            //Query to insert the Species into the database
            string query = "insert into Species (Name) values (@SpeciesName)";

            //Creating the sqlparameter object and defining it
            SqlParameter[] sqlparams = new SqlParameter[1]; 
            sqlparams[0] = new SqlParameter("@SpeciesName", SpeciesName);

            //Executing the sql command along with the parameters defined above  
            db.Database.ExecuteSqlCommand(query, sqlparams);


            //Calling the list page. 
            return RedirectToAction("List");
        }

        //GET: Function to get the specific Species details that will be displayed in the update species page 
        public ActionResult Update(int id)
        {
            //Query to get  details about the specific species
            Species selectedspecies = db.Species.SqlQuery("select * from species where speciesid = @id", new SqlParameter("@id", id)).FirstOrDefault();

            //Calliong the update view along with the species details 
            return View(selectedspecies);
        }

        //POST: Function to get the details from the form and update in the database
        [HttpPost]
        public ActionResult Update(int? id, string Name)
        {

            Debug.WriteLine("I am trying to edit a species name to " + Name);

            //Query to update the database with the new species value
            string query = "update species set Name = @Name where SpeciesID = @SpeciesID";

            //Creating the sqlparams object and defining the parameters used in the query
            SqlParameter[] sqlparams = new SqlParameter[2]; 
            sqlparams[0] = new SqlParameter("@Name", Name);
            sqlparams[1] = new SqlParameter("@SpeciesID", id);

            //Executing the update query along with the above defined parameters
            db.Database.ExecuteSqlCommand(query, sqlparams);
            return RedirectToAction("List");
        }

        //Method to delete the Species based on the speciesID passed

        public ActionResult Delete(int id)
        {
            //Query to delete the Species from the database
            string query = "delete from species where SpeciesID = @SpeciesID";

            //Creating the sqlparams object and definig it
            SqlParameter[] sqlparams = new SqlParameter[1];
            sqlparams[0] = new SqlParameter("@SpeciesID", id);

            //Executing the query along with the parameters passed above
            db.Database.ExecuteSqlCommand(query, sqlparams);
            //Going back to the list page
            return RedirectToAction("List");
        }
    }

}