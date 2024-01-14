using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PetConnect.Data;
using PetConnect.Models;
using System.Data;

namespace PetConnect.Controllers
{
    public class CommentsController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public CommentsController(ApplicationDbContext context, 
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            db = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // Stergerea unui comentariu asociat unui anunt de adoptie din baza de date
        [HttpPost]
        [Authorize(Roles = "User,Admin")]
        public IActionResult Delete(int id)
        {
            Comment comm = db.Comments.Find(id);


            

            if (comm.UserId == _userManager.GetUserId(User) || User.IsInRole("Admin"))
            {
                db.Comments.Remove(comm);
                db.SaveChanges();
                return Redirect("/Pets/Show/" + comm.PetId);
            }

            else
            {
                TempData["message"] = "Nu aveti dreptul sa stergeti comentariul";
                return RedirectToAction("Index", "Pets");
            }


        }
        [Authorize(Roles = "User,Admin")]
        public IActionResult Edit(int id)
        {
            Comment comm = db.Comments.Find(id);


            if (comm.UserId == _userManager.GetUserId(User) || User.IsInRole("Admin"))
            {
                return View(comm);
            }

            else
            {
                TempData["message"] = "Nu aveti dreptul sa editati comentariul";
                return RedirectToAction("Index", "Pets");
            }
           

        }

        [HttpPost]
        [Authorize(Roles = "User,Admin")]
        public IActionResult Edit(int id, Comment requestComment)
        {
            Comment comm = db.Comments.Find(id);

            /*if (ModelState.IsValid)
            {


                comm.Content = requestComment.Content;

                db.SaveChanges();

                return Redirect("/Pets/Show/" + comm.PetId);
            }
            else
            {
                return View(requestComment);
            }*/
            if (ModelState.IsValid)
            {
                if (comm.UserId == _userManager.GetUserId(User) || User.IsInRole("Admin"))
                {
                    comm.Content = requestComment.Content;

                    db.SaveChanges();

                    return Redirect("/Pets/Show/" + comm.PetId);
                }

                else
                {
                    TempData["message"] = "Nu aveti dreptul sa editati comentariul";
                    return RedirectToAction("Index", "Pets");
                }

            }
            else
            {
                return View(requestComment);
            }

        }


    }
}

