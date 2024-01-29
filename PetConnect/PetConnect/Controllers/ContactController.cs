using Microsoft.AspNetCore.Mvc;
using PetConnect.Data;
using PetConnect.Models;

public class ContactController : Controller
{
    private readonly ApplicationDbContext _context;

    public ContactController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult New()
    {
        return View();
    }

    [HttpPost]
    public IActionResult New(Contact contact)
    {
        if (ModelState.IsValid)
        {
            try
            {
                _context.Add(contact);
                _context.SaveChanges();
               
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while saving the data.");
               
            }
        }
        return View(contact);
    }

  
}
