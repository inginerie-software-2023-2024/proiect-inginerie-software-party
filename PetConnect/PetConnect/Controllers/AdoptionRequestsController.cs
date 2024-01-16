using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetConnect.Data;
using PetConnect.Models;

namespace PetConnect.Controllers
{
    public class AdoptionRequestsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _db;
        public AdoptionRequestsController(ApplicationDbContext db,
                                          UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _db = db;
        }
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult New(int id) // id = petId
        {
            AdoptionRequest adoptionRequest = new AdoptionRequest();
            adoptionRequest.PetId = id;

            return View(adoptionRequest);
        }


        [HttpPost]
        [Authorize]
        public IActionResult New(AdoptionRequest adoptionRequest)
        {
            if (ModelState.IsValid)
            {
                adoptionRequest.AdopterId = _userManager.GetUserId(User);
                adoptionRequest.Status = 1; // 1 = pending
                adoptionRequest.RequestDate = DateTime.Now;
                _db.AdoptionRequests.Add(adoptionRequest);
                _db.SaveChanges();

                return RedirectToAction("Index", "Pets");
            }
            else
            {
                return View(adoptionRequest);
            }
        }

        [Authorize]
        public IActionResult Show(int id)
        {
            var adoptionRequest = _db.AdoptionRequests.Include("Adopter")
                                                      .Include("Pet")
                                                      .Where(ar => ar.RequestId == id)
                                                      .First();

            if (adoptionRequest == null)
            {
                return RedirectToAction("Index", "Pets");
            }

            var currentUserId = _userManager.GetUserId(User);
            if (User.IsInRole("Admin") || currentUserId == adoptionRequest.Pet.UserId)
            {
                return View(adoptionRequest);
            }

            return RedirectToAction("Index", "Pets");
        }

        [HttpPost]
        [Authorize]
        public IActionResult ChangeStatus(int id, int requestStatus)
        {
            AdoptionRequest adoptionRequest = _db.AdoptionRequests.Find(id);
            var currentUserId = _userManager.GetUserId(User);

            if (User.IsInRole("Admin") || currentUserId == adoptionRequest.Pet.UserId)
            {
                adoptionRequest.Status = requestStatus;
                _db.SaveChanges();
            }

            return RedirectToAction("Index", "Pets");
        }

        [Authorize]
        public IActionResult ShowReceivedRequests()
        {
            string currentUserId = _userManager.GetUserId(User);

            var adoptionRequests = _db.AdoptionRequests.Include("Pet")
                                                       .Include("Adopter")
                                                       .Where(ar => ar.Pet.UserId == currentUserId &&
                                                              ar.Status == 1);

            ViewBag.AdoptionRequests = adoptionRequests;
            return View();
        }

        [Authorize]
        public IActionResult ShowSentRequests()
        {
            string currentUserId = _userManager.GetUserId(User);

            var adoptionRequests = _db.AdoptionRequests.Include("Pet")
                                                       .Include("Adopter")
                                                       .Where(ar => ar.AdopterId == currentUserId);

            ViewBag.AdoptionRequests = adoptionRequests;
            return View();
        }
    }
}
