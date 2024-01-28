using Microsoft.AspNetCore.Mvc;

namespace PetConnect.Controllers
{
    public class ContactController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string name, string email, string _subject, string message)
        {
            // logica pentru a procesa datele formularului
            return RedirectToAction("ThankYou");
        }

        public ActionResult ThankYou()
        {
            return View();
        }
    }
}
