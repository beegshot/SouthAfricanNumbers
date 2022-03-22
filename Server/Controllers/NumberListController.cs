using Microsoft.AspNetCore.Mvc;
using SouthAfricanNumbers.Server.Data;
using SouthAfricanNumbers.Shared;

namespace SouthAfricanNumbers.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NumberListController : ControllerBase
    {
        private readonly DataContext _context;

        public NumberListController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<List<Number>> GetAllNumber()
        {
            return Ok(_context.Numbers);
        }

        [HttpGet("{id}")]
        public ActionResult<Number> GetNumberWithId(string Id)
        {
            Guid _id = new Guid(Id);

            Number CurrentNumber = null;

            foreach (var num in _context.Numbers)
            {
                if (num.Id == _id)
                {
                    CurrentNumber = num;
                }
            }

            if (CurrentNumber == null)
            {
                return NotFound("This phone number does not exist");
            }
            else
            {
                return Ok(CurrentNumber);
            }
        }

        [HttpPost]
        public async Task<ActionResult<Number>> EditNumber(Number request)
        {
            Number CurrentNumber = null;

            foreach (var st in _context.Numbers)
            {
                if (st.Id == request.Id)
                {
                    CurrentNumber = st;
                }
            }

            if (CurrentNumber == null)
            {
                _context.Add(request);
            }
            else
            {
                _context.Entry(CurrentNumber).CurrentValues.SetValues(request);
            }

            await _context.SaveChangesAsync();
            return request;
        }
    }
}
