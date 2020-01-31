using System;
using System.Collections.Generic;
using System.Data;
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
    public class PetController : Controller
    {

        private PetGroomingContext db = new PetGroomingContext();

        // GET: This method is for getting the list of pets
        public ActionResult List()
        {
            //Query to select the list of pets
            List<Pet> pets = db.Pets.SqlQuery("Select * from Pets").ToList();
            //Calling the Pets List View to display the pet list
            return View(pets);
           
        }

        // GET: This method is used to display a specific pet based on the id
        public ActionResult Show(int? id)
        {
            //Checking if petid is null
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            //Query to get the pet with the petid 'id'.
            Pet pet = db.Pets.SqlQuery("select * from pets where petid=@PetID", new SqlParameter("@PetID",id)).FirstOrDefault();
            if (pet == null)
            {
                return HttpNotFound();
            }
            //Calling the pet show view to display the specific pet
            return View(pet);
        }

        //POST: This method is used get the values entered in the form and update in the database.
        [HttpPost]
        public ActionResult Add( string PetName, Double PetWeight, String PetColor, int SpeciesID, string PetNotes)
        {
            
            //Query to insert the pet into the database
            string query = "insert into pets (PetName, Weight, color, SpeciesID, Notes) values (@PetName,@PetWeight,@PetColor,@SpeciesID,@PetNotes)";
            SqlParameter[] sqlparams = new SqlParameter[5]; //Creating an object of class SQLParameter
            
            //Defining the parameters that are used in the query above.
            sqlparams[0] = new SqlParameter("@PetName",PetName);
            sqlparams[1] = new SqlParameter("@PetWeight", PetWeight);
            sqlparams[2] = new SqlParameter("@PetColor", PetColor);
            sqlparams[3] = new SqlParameter("@SpeciesID", SpeciesID);
            sqlparams[4] = new SqlParameter("@PetNotes",PetNotes);

            
            //Executing the query with the parameters defined above
            db.Database.ExecuteSqlCommand(query, sqlparams);

            
            //Returning back to the List page
            return RedirectToAction("List");
        }

        //GET: Function to get the list of species that will be displayed in the create pet page
        public ActionResult New()
        {

            //Query to get the list of species from the database.

            List<Species> species = db.Species.SqlQuery("select * from Species").ToList();

            //Calling the Pet view again along with the species list
            return View(species);
        }

        //GET: Function to get the specific pet details that will be displayed in the update pet page 
        public ActionResult Update(int id)
        {
            //Getting information about the specific pet
            Pet selectedpet = db.Pets.SqlQuery("select * from pets where petid = @id", new SqlParameter("@id",id)).FirstOrDefault();

            //Calling the view with the details about the pet to be displayed for update. 
            return View(selectedpet);
        }

        //POST: Function to get the details from the form and update in the database
        [HttpPost]
        public ActionResult Update(int ?id, string PetName, string PetColor, double PetWeight,string PetNotes)
        {

            Debug.WriteLine("I am trying to edit a pet's name to "+PetName+" and change the weight to "+PetWeight.ToString());

            //Query to update the pet with the values passed in the form
            string query = "update pets set PetName = @PetName, Weight = @PetWeight, color = @PetColor, Notes = @PetNotes where PetID = @PetID";
            SqlParameter[] sqlparams = new SqlParameter[5]; 

            sqlparams[0] = new SqlParameter("@PetName", PetName);
            sqlparams[1] = new SqlParameter("@PetWeight", PetWeight);
            sqlparams[2] = new SqlParameter("@PetColor", PetColor);
            sqlparams[3] = new SqlParameter("@PetNotes", PetNotes);
            sqlparams[4] = new SqlParameter("@PetID", id);

            //Executing the sql query along with the Parameters defined above
            db.Database.ExecuteSqlCommand(query, sqlparams);
            //Going back to the list pet page. 
            return RedirectToAction("List");
        }

        //Method to delete the pet based on the petID passed
        public ActionResult Delete(int id)
        {
            //Query to delete the pet from the database
            string query = "delete from pets where PetID = @PetID";

            //Creating the parameter object and definig it
            SqlParameter[] sqlparams = new SqlParameter[1]; 
            sqlparams[0] = new SqlParameter("@PetID", id);

            //Executing the query along with the parameters passed above
            db.Database.ExecuteSqlCommand(query, sqlparams);
            //Going back to the list page
            return RedirectToAction("List");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
