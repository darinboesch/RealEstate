
using Microsoft.AspNetCore.Mvc;
using RealEstate.App_Start;

namespace RealEstate.Controllers
{
    public class HomeController : Controller
    {
        private RealEstateContext _context;

        public HomeController(RealEstateContext context) {
            _context = context;
        }

        [HttpGet]
        public IActionResult Get() {
            return Json(_context.Database.Settings);
        }
    }
}