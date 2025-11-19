using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

//using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RealEstateAPI.Data;
using RealEstateAPI.DTO;
using RealEstateAPI.Models;
using System.Security.Claims;

namespace RealEstateAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropertiesController : ControllerBase
    {
        ApiDbContext db = new ApiDbContext();

        [HttpGet("PropertyList")]
        [Authorize]

        public IActionResult GetProperties(int categoryId)
        {
            var PropertyData = db.Properties.Where(x => x.CategoryId == categoryId);

            if (PropertyData is null)
                return NotFound();

            return Ok(PropertyData);
        
        
        }

        [HttpGet]
        [Authorize]

        public IActionResult Get()
        {
            return Ok(db.Properties);
        }

        [HttpGet("PropertyDetails")]
        [Authorize]
        public IActionResult GetPropertyDetails(int id)
        {
            var propertyData = db.Properties.FirstOrDefault(x => x.Id == id);

            if (propertyData is null)
                return NotFound();

            return Ok(propertyData);
        }

        [HttpGet("TrendingProperties")] 
        [Authorize]
        public IActionResult GetTrendingProperties()
        {
            var propertyData = db.Properties.Where(x => x.IsTrending == true).ToList();

            if (propertyData.Count == 0)
                return NotFound();

            return Ok(propertyData);
        }

        [HttpGet("SearchProperties")]
        [Authorize]
        public IActionResult GetSearchProperties([FromQuery]string address)
        {
            if (string.IsNullOrWhiteSpace(address))
                return Ok(new List<Property>());

            address = address.Trim().ToLower();

            var propertyData = db.Properties.Where(x => x.Address.ToLower().Contains(address)).ToList();

            //if (propertyData.Count == 0)
            //    return NotFound();

            return Ok(propertyData);
        }


        [HttpPost]
        [Authorize]
        public IActionResult Post([FromBody] PropertyDTO property) //Add record
        {
            if (property is null)
            {
                return NoContent();
            }
            else
            {
                var userEmail = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
                var user = db.Users.FirstOrDefault(x => x.Email == userEmail);

                if (user is null)
                    return NotFound();

                var properties = new Property
                {
                    Name = property.Name,
                    Details = property.Details,
                    Address = property.Address,
                    ImageUrel = property.ImageUrel,
                    price = property.price,
                    CategoryId = property.CategoryId,
                    IsTrending = false,
                    UserId = user.Id
                };

                db.Properties.Add(properties);
                db.SaveChanges();

                return StatusCode(StatusCodes.Status201Created);
            }
        }


        [HttpPut("{id}")]
        [Authorize]

        public IActionResult Put(int id, [FromBody] PropertyDTO property)
        {
            var userData = db.Properties.FirstOrDefault(x => x.Id == id);

            if (userData is null)
            {
                return NotFound();
            }
            else
            {
                var userEmail = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
                var user = db.Users.FirstOrDefault(x => x.Email == userEmail);

                if (user is null)
                    return NotFound();

                if (userData.UserId == user.Id)
                {
                    userData.Name = property.Name;
                    userData.Details = property.Details;
                    userData.Address = property.Address;
                    userData.price = property.price;

                    property.IsTrending = false;
                    property.UserId = user.Id;

                    db.SaveChanges();
                    return Ok("Record updated successfully.");

                }

                return BadRequest();
            }
        }

        [HttpDelete("{id}")]
        [Authorize]

        public IActionResult Delete(int id)
        {
            var userData = db.Properties.FirstOrDefault(x => x.Id == id);

            if (userData is null)
                return NotFound();
            else
            {
                var userEmail = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
                var user = db.Users.FirstOrDefault(x => x.Email == userEmail);

                if (user is null)
                    return NotFound();

                if (userData.Id == user.Id)
                {
                    db.Properties.Remove(userData);
                    db.SaveChanges();
                    return Ok("Record deleted successfully.");
                }
                return BadRequest();
            }
        }
    }
}
