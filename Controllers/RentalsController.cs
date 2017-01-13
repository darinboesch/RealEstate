using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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

        public ActionResult Index(RentalsFilter filters) {
            //var rentals = _context.Rentals.Find(_ => true).ToList();
            var rentals = FilterRentals(filters);
            var model = new RentalsList {
                Rentals = rentals,
                Filters = filters
            };
            return View(model);
        }

        IEnumerable<Rental> FilterRentals(RentalsFilter filters) {
            IQueryable<Rental> rentals = _context
                                            .Rentals
                                            .AsQueryable()
                                            .OrderBy(o => o.Price);

            if (filters.MinimumRooms.HasValue) {
                rentals = rentals
                            .Where(r => r.NumberOfRooms >= filters.MinimumRooms);
            }

            if (filters.PriceLimit.HasValue) {
                rentals = rentals
                            .Where(r => r.Price < filters.PriceLimit);
            }

            return rentals.ToList();
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

        private async Task<Rental> GetRental(string id) {
            return await _context.Rentals.Find(i => i.Id == id).FirstOrDefaultAsync();
        }

        public async Task<ActionResult> AdjustPrice(string id)
        {
            return View(await GetRental(id));
        }

        [HttpPost]
        public async Task<ActionResult> AdjustPrice(string id, AdjustPrice adjustPrice) {
            var rental = await GetRental(id);
            rental.AdjustPrice(adjustPrice);
            await _context.Rentals.ReplaceOneAsync(i => i.Id == id, rental, new UpdateOptions {IsUpsert = true});
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Delete(string id) {
            await _context.Rentals.FindOneAndDeleteAsync(i => i.Id == id);
            return RedirectToAction("Index");
        }
    }
}