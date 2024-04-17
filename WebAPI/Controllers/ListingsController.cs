using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Entities;
using WebAPI.Messages;

namespace WebAPI.Controllers
{
    [Route("api/Listings")]
    [ApiController]
    public class ListingsController : ControllerBase
    {
        private readonly ListingDbContext _dBcontext;

        private string GenerateFullUrl(string path)
        {
            return $"{Request.Scheme}://{Request.Host}{path}";
        }

        public ListingsController(ListingDbContext context)
        {
            _dBcontext = context;
        }

        [HttpGet]
        public IActionResult GetListingApiHome()
        {
            // Creating a DTO representing the home of the Listing API
            var apiHomeViewModel = new ListingApiHomeDTO
            {
                Links = new Dictionary<string, Link>
                {
                    { "self", new Link(GenerateFullUrl("/api/Listings"), "self", "GET") },
                    { "lisings", new Link(GenerateFullUrl("/api/Listings/All"), "listings", "GET") },
                    { "createListing", new Link(GenerateFullUrl("/api/Listings"), "createListing", "POST") },
                    { "updateListing", new Link(GenerateFullUrl("/api/Listings/{id}"), "updateListing", "PUT") },
                    { "deleteListing", new Link(GenerateFullUrl("/api/Listings/{id}"), "deleteListing", "DELETE") }
                },
                ApiVersion = "1.0",
                Creator = "Group 5"
            };

            return Ok(apiHomeViewModel);
        }


        [HttpPost]
        public async Task<IActionResult> CreateListing([FromBody] ListingRequest listing)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // TO DO: Implement a way to store all information into a Listing object

            var newListing = new Listings();

            _dBcontext.Listings.Add(newListing);
            await _dBcontext.SaveChangesAsync();

            return CreatedAtAction("GetListing", new { id = newListing.listingsId }, newListing); ;
        }

        // GET: api/Listings/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetListing(int id)
        {
            var listing = await _dBcontext.Listings
                .Include(l => l.Car)  // Including details of Car
                .FirstOrDefaultAsync(l => l.listingsId == id);

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
            if (id != listing.listingsId)
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

        [HttpGet("All")]
        public async Task<IActionResult> GetListings([FromQuery] Filter filter)
        {
            IQueryable<Listings> query = _dBcontext.Listings.Include(l => l.Car);

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
                query = query.OrderBy(l => l.listingsId); 
            }

            var listings = await query.ToListAsync();

            return Ok(listings);
        }


        private bool ListingExists(int id)
        {
            return _dBcontext.Listings.Any(e => e.listingsId == id);
        }
    }
}
 