using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PetConnect.Data;
using PetConnect.Models;
using System;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;

namespace PetConnect.Controllers
{
    public class PetsController : Controller
    {
        private readonly ApplicationDbContext db;
        private IWebHostEnvironment _env;

        public PetsController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            db = context;
            _env = env;
        }


        public IActionResult Index()
        {
            
            var pets = from pet in db.Pets
                           orderby pet.Name
                           select pet;
            ViewBag.Pets = pets;
            return View();

        }
        public ActionResult Show(int id)
        {
            Pet pet = db.Pets.Find(id);
            ViewBag.Pet = pet;
            return View();
        }

        public IActionResult New()
        {
            return View();
        }
        [HttpPost]
        public IActionResult New(Pet p)
        {
            try
            {
                db.Pets.Add(p);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return View();
            }
        }
        //Imagini
        public IActionResult UploadImage()
        {
            return View();
        }

        [HttpPost]
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
            db.Pets.Add(pet);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        public IActionResult Edit(int id)
        {
            Pet pet = db.Pets.Find(id);
            return View(pet);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(int id, Pet requestPet, IFormFile PetImage)
        {
            Pet pet = db.Pets.Find(id);
          
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




                db.SaveChanges();

                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    return RedirectToAction("Edit", pet.PetId);
                }
            }
       


        [HttpPost]
        public ActionResult Delete(int id)
        {
            Pet pet = db.Pets.Find(id);
            db.Pets.Remove(pet);
            db.SaveChanges();
            return RedirectToAction("Index");
        }





    }
}
