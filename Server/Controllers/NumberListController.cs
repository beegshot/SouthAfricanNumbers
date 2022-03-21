using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SouthAfricanNumbers.Shared;

namespace SouthAfricanNumbers.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NumberListController : ControllerBase
    {
        public List<Number> WNumbers { get; set; } = new List<Number>()
        {
            new Number {Id = 1, PhoneNumber = "0123456789"},
            new Number {Id = 2, PhoneNumber = "9876543210"},
            new Number {Id = 3, PhoneNumber = "5678901234"},
            new Number {Id = 4, PhoneNumber = "4321098765"}
        };

        [HttpGet]
        public ActionResult<List<Number>> GetAllNumber()
        {
            return Ok(WNumbers);
        }

        [HttpGet("{id}")]
        public ActionResult<Number> GetNumberWithId(int id)
        {
            Number CurrentNumber = WNumbers.FirstOrDefault(p => p.Id.Equals(id));
            if(CurrentNumber == null)
            {
                return NotFound("Number Id is missing");
            }
            else
            {
                return Ok(CurrentNumber);
            }
        }
    }
}
