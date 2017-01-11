using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using RealEstate.App_Start;
using RealEstate.Rentals;

namespace RealEstate.Controllers
{
    public class RentalsController : Controller
    {
        private RealEstateContext _context;

        public RentalsController(RealEstateContext context) {
            _context = context;
        }

        public ActionResult Index() {
            var rentals = _context.Rentals.Find(_ => true).ToList();
            return View(rentals);
        }

        public ActionResult Post() {
            return View();
        }

        [HttpPost]
        public ActionResult Post(PostRental postRental) {
            var rental = new Rental(postRental);
            _context.Rentals.InsertOne(rental);
            return RedirectToAction("Index");
        }
    }
}