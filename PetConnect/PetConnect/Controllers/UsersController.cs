using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PetConnect.Data;
using PetConnect.Models;
using System.Data;

namespace PetConnect.Controllers
{
    
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext db;

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly RoleManager<IdentityRole> _roleManager;

        public UsersController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager
            )
        {
            db = context;

            _userManager = userManager;

            _roleManager = roleManager;
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            var users = from user in db.Users
                        orderby user.UserName
                        select user;

            ViewBag.UsersList = users;

            return View();
        }

        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Show(string id)
        {
            ApplicationUser user = db.Users.Find(id);
            var roles = await _userManager.GetRolesAsync(user);

            ViewBag.Roles = roles;

            return View(user);
        }

        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit(string id)
        {
            ApplicationUser user = db.Users.Find(id);

            user.AllRoles = GetAllRoles();

            var roleNames = await _userManager.GetRolesAsync(user); // Lista de nume de roluri

            // Cautam ID-ul rolului in baza de date
            var currentUserRole = _roleManager.Roles
                                              .Where(r => roleNames.Contains(r.Name))
                                              .Select(r => r.Id)
                                              .First(); // Selectam 1 singur rol
            ViewBag.UserRole = currentUserRole;

            return View(user);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult> Edit(string id, ApplicationUser newData, [FromForm] string newRole)
        {
            ApplicationUser user = db.Users.Find(id);

            user.AllRoles = GetAllRoles();


            if (ModelState.IsValid)
            {
                user.UserName = newData.UserName;
                user.Email = newData.Email;
                user.FirstName = newData.FirstName;
                user.LastName = newData.LastName;
                user.PhoneNumber = newData.PhoneNumber;


                // Cautam toate rolurile din baza de date
                var roles = db.Roles.ToList();

                foreach (var role in roles)
                {
                    // Scoatem userul din rolurile anterioare
                    await _userManager.RemoveFromRoleAsync(user, role.Name);
                }
                // Adaugam noul rol selectat
                var roleName = await _roleManager.FindByIdAsync(newRole);
                await _userManager.AddToRoleAsync(user, roleName.ToString());

                db.SaveChanges();

            }
            return RedirectToAction("Index");
        }

        // EditProfile pentru editarea profilului
        public async Task<IActionResult> EditProfile()
        {
            // Obține utilizatorul autentificat
            ApplicationUser user = await _userManager.GetUserAsync(User);

            // Verifică dacă utilizatorul există
            if (user == null)
            {
                return NotFound();
            }

            // Pasează utilizatorul la view
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(ApplicationUser newData)
        {
            // Obține utilizatorul autentificat
            ApplicationUser user = await _userManager.GetUserAsync(User);

            // Verifică dacă utilizatorul există
            if (user == null)
            {
                return NotFound();
            }

            // Actualizează datele utilizatorului
            user.UserName = newData.UserName;
            user.Email = newData.Email;
            user.FirstName = newData.FirstName;
            user.LastName = newData.LastName;
            user.PhoneNumber = newData.PhoneNumber;

            // Salvează modificările în baza de date
            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                // Redirecționează către o acțiune sau pagină specifică pentru profilul editat
                return RedirectToAction("ShowProfile", new { id = user.Id }); 
            }
            else
            {
                // În caz de erori, adaugă erorile în ModelState
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                // Returnează view-ul cu erorile
                return View(user);
            }
        }

        // vizualizarea profilului
        public async Task<IActionResult> ShowProfile(string id)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return View("ShowProfile", user);
        }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Delete(string id)
        {
            var user = db.Users
                         .Include("Pets")
                         .Include("Comments")
                         .Include("AdoptionRequest")
                         .Where(u => u.Id == id)
                         .First();

            // Delete user pet adoption announcements
            if (user.Pets.Count > 0)
            {
                foreach (var pet in user.Pets)
                {
                    db.Pets.Remove(pet);
                }
            }
            // Delete user comments
            if (user.Comments.Count > 0)
            {
                foreach (var comment in user.Comments)
                {
                    db.Comments.Remove(comment);
                }
            }
            // Delete user pet request
            if (user.AdoptionRequests.Count > 0)
            {
                foreach (var adoptionRequest in user.AdoptionRequests)
                {
                    db.AdoptionRequests.Remove(adoptionRequest);
                }
            }

            db.Users.Remove(user);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        [NonAction]
        public IEnumerable<SelectListItem> GetAllRoles()
        {
            var selectList = new List<SelectListItem>();

            var roles = from role in db.Roles
                        select role;

            foreach (var role in roles)
            {
                selectList.Add(new SelectListItem
                {
                    Value = role.Id.ToString(),
                    Text = role.Name.ToString()
                });
            }
            return selectList;
        }
    }
}
