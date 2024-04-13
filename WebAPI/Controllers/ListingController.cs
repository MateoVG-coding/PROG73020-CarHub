using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ListingController : ControllerBase
    {
        private readonly ListingDbContext _dBcontext;

        public ListingController(ListingDbContext context)
        {
            _dBcontext = context;
        }

        // POST: api/Listings
        [HttpPost]
        public async Task<IActionResult> CreateListing([FromBody] Listings listing)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _dBcontext.Listings.Add(listing);
            await _dBcontext.SaveChangesAsync();

            return CreatedAtAction("GetListing", new { id = listing.listingId }, listing);
        }

        // GET: api/Listings/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetListing(int id)
        {
            var listing = await _dBcontext.Listings
                .Include(l => l.Car)  // Including details of Car
                .Include(l => l.User) // Including details of User
                .FirstOrDefaultAsync(l => l.listingId == id);

            if (listing == null)
            {
                return NotFound();
            }

            return Ok(listing);
        }

        // PUT: api/Listings/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateListing(int id, [FromBody] Listings listing)
        {
            if (id != listing.listingId)
            {
                return BadRequest();
            }

            _dBcontext.Entry(listing).State = EntityState.Modified;

            try
            {
                await _dBcontext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ListingExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Listings/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteListing(int id)
        {
            var listing = await _dBcontext.Listings.FindAsync(id);
            if (listing == null)
            {
                return NotFound();
            }

            _dBcontext.Listings.Remove(listing);
            await _dBcontext.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> GetListings([FromQuery] Filter filter)
        {
            IQueryable<Listings> query = _dBcontext.Listings.Include(l => l.Car).Include(l => l.User);

            // Basic filtering
            if (!string.IsNullOrEmpty(filter.Brand))
            {
                query = query.Where(l => l.Car.Brand.ToLower() == filter.Brand.ToLower());
            }

            if (filter.MinYear.HasValue)
            {
                query = query.Where(l => l.Car.Year >= filter.MinYear.Value);
            }

            if (filter.MaxYear.HasValue)
            {
                query = query.Where(l => l.Car.Year <= filter.MaxYear.Value);
            }

            if (!string.IsNullOrEmpty(filter.Model))
            {
                query = query.Where(l => l.Car.Model.ToLower() == filter.Model.ToLower());
            }

            // Sorting
            if (filter.SortByDate == true)
            {
                query = query.OrderByDescending(l => l.PostingDate);
            }
            else
            {
                query = query.OrderBy(l => l.listingId);
            }

            var listings = await query.ToListAsync();

            return Ok(listings);
        }


        private bool ListingExists(int id)
        {
            return _dBcontext.Listings.Any(e => e.listingId == id);
        }

    }
}
