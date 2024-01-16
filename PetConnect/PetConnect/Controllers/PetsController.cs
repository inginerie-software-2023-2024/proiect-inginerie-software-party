using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PetConnect.Data;
using PetConnect.Models;
using Microsoft.AspNetCore.Authorization;
using System.Drawing;
using Microsoft.EntityFrameworkCore;

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

            /*var pets = from pet in db.Pets
                       orderby pet.Name
                       select pet;*/

            var pets = db.Pets.Include("User");
            ViewBag.Pets = pets;


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

            ViewBag.PaginationBaseUrl = "/Pets/Index/?page";
            
            return View();

        }


        public ActionResult Show(int id)
        {/*
            Pet pet = db.Pets.Find(id);*/
            Pet pet   =  db.Pets.Include("User")
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


        [Authorize(Roles = "Admin")]
        public IActionResult New()
        {
            Pet pet = new Pet();

            // editorul trimite cereri adminului pt adaugare
            pet.UserId = _userManager.GetUserId(User);
            pet.Approved = User.IsInRole("Admin");

            return View(pet);
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
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

            return View();
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

     
        [Authorize(Roles = "Admin")]
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

        
        [Authorize(Roles = "Admin")]
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

        
        [Authorize(Roles = "Admin")]
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
                TempData["message"] = "Anuntul  a fost stears";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["message"] = "Nu aveti dreptul sa stergeti un anunt care nu va apartine";
                return RedirectToAction("Index");
            }
        }

    }
}
