using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PetConnect.Data;
using PetConnect.Models;
using Microsoft.AspNetCore.Authorization;
using System.Drawing;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Filters;

namespace PetConnect.Controllers
{
    public class PetsController : Controller
    {
        private readonly ApplicationDbContext db;
        private ApplicationDbContext _context;
        private IWebHostEnvironment _env;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public PetsController(ApplicationDbContext context,
                              IWebHostEnvironment env,
                              UserManager<ApplicationUser> userManager,
                              RoleManager<IdentityRole> roleManager)
        {
            db = context;
            _env = env;
            _userManager = userManager;
            _roleManager = roleManager;
        }


        public IActionResult Index()
        {

            var pets = db.Pets.Include("User");

            var search = "";

            var speciesFilter = "";

            var breedFilter = "";

            var colorFilter = "";

            var locationFilter = "";

            var ageFilter = "";

            var sizeFilter = "";

            var sexFilter = "";

            var vaccineFilter = "";

            var sterilizeFilter = "";

            // retinem valorile distincte existente pentru fiecare atribut

            ViewBag.SpeciesList = GetDistinctSpecies();
            ViewBag.BreedsList = GetDistinctBreed();
            ViewBag.ColorList = GetDistinctColor();
            ViewBag.LocationList = GetDistinctLocation();
            ViewBag.AgeList = GetDistinctAge();
            ViewBag.SizeList = GetDistinctSize();
            ViewBag.SexList = GetDistinctSex();
            ViewBag.VaccineList = GetDistinctVaccined();
            ViewBag.SterilizeList = GetDistinctSterilized();


            //filtrare

            if (Convert.ToString(HttpContext.Request.Query["speciesFilter"]) != null)
            {

                speciesFilter = Convert.ToString(HttpContext.Request.Query["speciesFilter"]).Trim();

                ViewBag.SpeciesFilter = speciesFilter;

            }

            if (!string.IsNullOrEmpty(speciesFilter))
            {
                pets = pets.Where(p => p.Species == speciesFilter);

            }

            if (Convert.ToString(HttpContext.Request.Query["breedFilter"]) != null)
            {

                breedFilter = Convert.ToString(HttpContext.Request.Query["breedFilter"]).Trim();

                ViewBag.BreedFilter = breedFilter;

            }


            if (!string.IsNullOrEmpty(breedFilter))
            {
                pets = pets.Where(p => p.Breed == breedFilter);

            }


            if (Convert.ToString(HttpContext.Request.Query["colorFilter"]) != null)
            {

                colorFilter = Convert.ToString(HttpContext.Request.Query["colorFilter"]).Trim();

                ViewBag.ColorFilter = colorFilter;

            }

            if (!string.IsNullOrEmpty(colorFilter))
            {
                pets = pets.Where(p => p.Color == colorFilter);

            }

            if (Convert.ToString(HttpContext.Request.Query["locationFilter"]) != null)
            {

                locationFilter = Convert.ToString(HttpContext.Request.Query["locationFilter"]).Trim();

                ViewBag.LocationFilter = locationFilter;

            }

            if (!string.IsNullOrEmpty(locationFilter))
            {
                pets = pets.Where(p => p.Location == locationFilter);

            }

            if (Convert.ToString(HttpContext.Request.Query["ageFilter"]) != null)
            {

                ageFilter = Convert.ToString(HttpContext.Request.Query["ageFilter"]).Trim();

                ViewBag.AgeFilter = ageFilter;
            }

            if (!string.IsNullOrEmpty(ageFilter))
            {
                pets = pets.Where(p => p.Age.ToString() == ageFilter);

            }

            if (Convert.ToString(HttpContext.Request.Query["sizeFilter"]) != null)
            {

                sizeFilter = Convert.ToString(HttpContext.Request.Query["sizeFilter"]).Trim();

                ViewBag.SizeFilter = sizeFilter;

            }

            if (!string.IsNullOrEmpty(sizeFilter))
            {
                pets = pets.Where(p => p.Size.ToString() == sizeFilter);

            }

            if (Convert.ToString(HttpContext.Request.Query["sexFilter"]) != null)
            {

                sexFilter = Convert.ToString(HttpContext.Request.Query["sexFilter"]).Trim();

                ViewBag.SexFilter = sexFilter;

            }

            if (!string.IsNullOrEmpty(sexFilter))
            {
                pets = pets.Where(p => (p.Sex == false && sexFilter == "Femela") || (p.Sex == true && sexFilter == "Mascul"));

            }

            if (Convert.ToString(HttpContext.Request.Query["vaccineFilter"]) != null)
            {

                vaccineFilter = Convert.ToString(HttpContext.Request.Query["vaccineFilter"]).Trim();

                ViewBag.VaccineFilter = vaccineFilter;

            }


            if (!string.IsNullOrEmpty(vaccineFilter))
            {
                pets = pets.Where(p => (p.Vaccined == false && vaccineFilter == "Nevaccinat") || (p.Vaccined == true && vaccineFilter == "Vaccinat"));

            }

            if (Convert.ToString(HttpContext.Request.Query["sterilizeFilter"]) != null)
            {

                sterilizeFilter = Convert.ToString(HttpContext.Request.Query["sterilizeFilter"]).Trim();

                ViewBag.SterilizeFilter = sterilizeFilter;

            }

            if (!string.IsNullOrEmpty(sterilizeFilter))
            {
                pets = pets.Where(p => (p.Sterilized == false && sterilizeFilter == "Nesterilizat") || (p.Sterilized == true && sterilizeFilter == "Sterilizat"));

            }


            // MOTOR DE CAUTARE

            if (Convert.ToString(HttpContext.Request.Query["search"]) != null)
            {
                search = Convert.ToString(HttpContext.Request.Query["search"]).Trim(); // eliminam spatiile libere 

                // Cautare in anunt (Name, species, breed, age, size, sex, color, location, description)


                List<int> petIds = db.Pets.Where(
                                        at => at.Name.Contains(search)
                                              || at.Species.Contains(search)
                                              || at.Breed.Contains(search)
                                              || at.Age.ToString().Contains(search)
                                              || at.Size.ToString().Contains(search)
                                              || at.Color.Contains(search)
                                              || at.Location.Contains(search)
                                        ).Select(a => a.PetId).ToList();


                pets = pets.Where(pet => petIds.Contains(pet.PetId))
                                      .Include("User");

                if (petIds.Count == 0)
                {
                    ViewBag.NullSearchMessage = "Nu au fost gasite animalute dupa atributele cautate.\nIncearca sa cauti dupa:\n- nume\n- specie\n- rasa\n- varsta\n- marime\n- culoare\n- locatie\n- descriere";

                }

            }

            ViewBag.SearchString = search;

            int _perPage = 12; //numarul pe articole per pagina

            if (TempData.ContainsKey("message"))
            {
                ViewBag.message = TempData["message"].ToString();
            }

            int totalItems = pets.Count(); //verificam de fiecare data, e un nr variabil de anunturi

            var currentPage = Convert.ToInt32(HttpContext.Request.Query["page"]); //se preia pagina curenta din view-ul asocial (val. param. page din ruta)

            var offset = 0; //offset 0 pt prima pagina

            if (!currentPage.Equals(0))
            {
                offset = (currentPage - 1) * _perPage; //calculam offset-ul pt celelalte pagini
            }

            var paginatedPets = pets.Skip(offset).Take(_perPage); //se preiau articolele dupa offset

            ViewBag.lastPage = Math.Ceiling((float)totalItems / (float)_perPage); //ultima pagina

            ViewBag.Pets = paginatedPets;

            if (search != "")
            {
                ViewBag.PaginationBaseUrl = $"/Pets/Index/?search={search}&speciesFilter={speciesFilter}&breedFilter={breedFilter}&colorFilter={colorFilter}&locationFilter={locationFilter}&ageFilter={ageFilter}&sizeFilter={sizeFilter}&sexFilter={sexFilter}&vaccineFilter={vaccineFilter}&sterilizeFilter={sterilizeFilter}&page";
            }
            else
            {
                ViewBag.PaginationBaseUrl = $"/Pets/Index/?speciesFilter={speciesFilter}&breedFilter={breedFilter}&colorFilter={colorFilter}&locationFilter={locationFilter}&ageFilter={ageFilter}&sizeFilter={sizeFilter}&sexFilter={sexFilter}&vaccineFilter={vaccineFilter}&sterilizeFilter={sterilizeFilter}&page";
            }

            return View();

        }


        public ActionResult Show(int id)
        {/*
            Pet pet = db.Pets.Find(id);*/
            Pet pet = db.Pets.Include("User")
                                .Include("Comments")
                                .Include("Comments.User")
                                .Where(pet => pet.PetId == id)
                                .First();
            SetAccessRights();
            return View(pet);
        }


        // butoanele editare/stergere sunt vizibile doar adminului 
        // care le-a adaugat
        private void SetAccessRights()
        {
            ViewBag.AfisareButoane = false;

            ViewBag.UserCurent = _userManager.GetUserId(User);

            ViewBag.EsteAdmin = User.IsInRole("Admin");

            ViewBag.EsteUser = User.IsInRole("User");

        }


        // Adaugarea unui comentariu 
        [HttpPost]
        [Authorize(Roles = "User,Admin")]
        public IActionResult Show([FromForm] Comment comment)
        {
            comment.Date = DateTime.Now;
            comment.UserId = _userManager.GetUserId(User);

            if (ModelState.IsValid)
            {
                db.Comments.Add(comment);
                db.SaveChanges();
                return Redirect("/Pets/Show/" + comment.PetId);
            }

            else
            {
                Pet pet = db.Pets.Include("User")
                                   .Include("Comments")
                                   .Include("Comments.User")
                                   .Where(pet => pet.PetId == comment.PetId)
                                   .First();

                SetAccessRights();

                //return Redirect("/Articles/Show/" + comm.ArticleId);

                return View(pet);
            }
        }


        [Authorize(Roles = "User,Admin")]
        public IActionResult New()
        {
            Pet pet = new Pet();

            // editorul trimite cereri adminului pt adaugare
            pet.UserId = _userManager.GetUserId(User);
            pet.Approved = User.IsInRole("Admin");

            return View(pet);
        }


        [HttpPost]
        [Authorize(Roles = "User,Admin")]
        public IActionResult New(Pet pet)
        {
            pet.UserId = _userManager.GetUserId(User);
            pet.Approved = User.IsInRole("Admin");

            if (ModelState.IsValid)
            {
                db.Pets.Add(pet);
                db.SaveChanges();

                if (User.IsInRole("Admin"))
                {
                    TempData["message"] = "Anuntul a fost adaugat";
                }
                else
                {
                    TempData["message"] = "Anuntul asteapta aprobarea adminului";
                }

                return RedirectToAction("Index");


            }
            else
            {
                return View(pet);
            }
        }


        [Authorize(Roles = "Admin")]
        public IActionResult Approve()
        {
            var pets = db.Pets.Include("User");
            ViewBag.Pets = pets;

            if (TempData.ContainsKey("message"))
                ViewBag.Message = TempData["message"];

            return View();
        }



        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Approve(int id)
        {
            Pet pet = db.Pets.Find(id);
            pet.Approved = true;
            if (ModelState.IsValid)
            {
                //db.Books.Add(book);
                db.SaveChanges();
                TempData["message"] = "Anuntul a fost adaugat";
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }

        }


        //Imagini
        [Authorize]
        public IActionResult UploadImage()
        {
            return View();
        }


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UploadImage(Pet pet, IFormFile PetImage)
        {
            var databaseFileName = "";
            if (PetImage != null)
            {
                if (PetImage.Length > 0)
                {
                    // Generam calea de stocare a fisierului
                    var storagePath = Path.Combine(
                    _env.WebRootPath, // Luam calea folderului wwwroot
                    "images", // Adaugam calea folderului images
                    PetImage.FileName // Numele fisierului
                    );

                    databaseFileName = "/images/" + PetImage.FileName;
                    // Uploadam fisierul la calea de storage
                    using (var fileStream = new FileStream(storagePath, FileMode.Create))
                    {
                        await PetImage.CopyToAsync(fileStream);
                    }
                }

                //Salvam storagePath-ul in baza de date

                pet.Image = databaseFileName;
                pet.UserId = _userManager.GetUserId(User);
                db.Pets.Add(pet);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return Redirect("/Pets/New/" + pet);
            }
        }


        [Authorize(Roles = "User, Admin")]
        public IActionResult Edit(int id)
        {
            Pet pet = db.Pets.Find(id);
            string currentUserId = _userManager.GetUserId(User);

            if (pet.UserId == _userManager.GetUserId(User) || User.IsInRole("Admin"))
            {
                return View(pet);
            }
            else
            {
                TempData["message"] = "Nu aveti dreptul sa faceti modificari asupra unui anunt care nu va apartine";
                return RedirectToAction("Index");
            }


        }


        [Authorize(Roles = "User, Admin")]
        [HttpPost]
        public async Task<ActionResult> Edit(int id, Pet requestPet, IFormFile PetImage)
        {
            Pet pet = db.Pets.Find(id);
            string currentUserId = _userManager.GetUserId(User);
            if (currentUserId == pet.UserId || User.IsInRole("Admin"))
            {
                try
                {
                    pet.Name = requestPet.Name;
                    pet.Species = requestPet.Species;
                    pet.Breed = requestPet.Breed;
                    pet.Age = requestPet.Age;
                    pet.Size = requestPet.Size;
                    pet.Sex = requestPet.Sex;
                    pet.Color = requestPet.Color;
                    pet.Vaccined = requestPet.Vaccined;
                    pet.Sterilized = requestPet.Sterilized;
                    pet.Description = requestPet.Description;
                    pet.Location = requestPet.Location;

                    if (PetImage != null && PetImage.Length > 0)
                    {
                        // Generați o nouă cale pentru imagine
                        var storagePath = Path.Combine(
                            _env.WebRootPath,
                            "images",
                            PetImage.FileName
                        );

                        // Încărcați imaginea la calea de stocare
                        using (var fileStream = new FileStream(storagePath, FileMode.Create))
                        {
                            await PetImage.CopyToAsync(fileStream);
                        }

                        // Actualizați calea imaginii în obiectul Pet
                        pet.Image = "/images/" + PetImage.FileName;
                    }
                    /*  TempData["message"] = "Anuntul a fost modificat";*/
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    /*TempData["message"] = "Nu aveti dreptul sa faceti modificari asupra unui anunt care nu va apartine";*/
                    return RedirectToAction("Index");
                }

            }
            return RedirectToAction("Index");

        }


        [Authorize(Roles = "User,Admin")]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            Pet pet = db.Pets.Include("Comments")
                                 .Where(pet => pet.PetId == id)
                                 .First();

            if (pet.UserId == _userManager.GetUserId(User) || User.IsInRole("Admin"))
            {
                db.Pets.Remove(pet);
                db.SaveChanges();
                TempData["message"] = "Anuntul  a fost sters";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["message"] = "Nu aveti dreptul sa stergeti un anunt care nu va apartine";
                return RedirectToAction("Index");
            }
        }


        //functions to get lists of pets for dropdown filtering
        public List<string> GetDistinctSpecies()
        {
            return db.Pets.Select(p => p.Species).Distinct().ToList(); //gets a list of distinct species found in Pets table
        }

        public List<string> GetDistinctBreed()
        {
            return db.Pets.Select(p => p.Breed).Distinct().ToList();
        }

        public List<string> GetDistinctColor()
        {
            return db.Pets.Select(p => p.Color).Distinct().ToList();
        }
        public List<string> GetDistinctLocation()
        {
            return db.Pets.Select(p => p.Location).Distinct().ToList();
        }

        public List<string> GetDistinctAge()
        {
            return db.Pets.Select(p => (p.Age).ToString()).Distinct().ToList();
        }

        public List<string> GetDistinctSize()
        {
            return db.Pets.Select(p => (p.Size).ToString()).Distinct().ToList();
        }

        public List<string> GetDistinctSex()
        {
            var distinctSexValues = db.Pets.Select(p => p.Sex).Distinct().ToList();

            List<string> sexList = new List<string>();

            foreach (var sexValue in distinctSexValues)
            {
                string sexString = !sexValue ? "Femela" : "Mascul";
                sexList.Add(sexString);
            }

            return sexList;
        }

        public List<string> GetDistinctVaccined()
        {
            var distinctVaccinevalues = db.Pets.Select(p => p.Vaccined).Distinct().ToList();

            List<string> vaccineList = new List<string>();

            foreach (var vaccval in distinctVaccinevalues)
            {
                string vaccstring = !vaccval ? "Nevaccinat" : "Vaccinat";
                vaccineList.Add(vaccstring);
            }

            return vaccineList;
        }

        public List<string> GetDistinctSterilized()
        {
            var distinctSterilizedvalues = db.Pets.Select(p => p.Sterilized).Distinct().ToList();

            List<string> sterList = new List<string>();

            foreach (var sterval in distinctSterilizedvalues)
            {
                string vaccstring = !sterval ? "Nesterilizat" : "Sterilizat";
                sterList.Add(vaccstring);
            }

            return sterList;
        }

    }
}
