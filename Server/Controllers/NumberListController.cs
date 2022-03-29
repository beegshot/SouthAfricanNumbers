using Microsoft.AspNetCore.Mvc;
using SouthAfricanNumbers.Server.Data;
using SouthAfricanNumbers.Shared;
using System.Text.RegularExpressions;

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
            return Ok(_context.Numbers.ToList());
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
        public async Task<ActionResult<NumberResponse>> EditNumber(Number request)
        {
            Number CurrentNumber = null;

            var res = await AddNumber(request);

            await _context.SaveChangesAsync();

            if(res.message.Contains("created"))
            {
                return this.StatusCode(StatusCodes.Status201Created, res);
            }
            if(res.message.Contains("updated"))
            {
                return this.StatusCode(StatusCodes.Status200OK, res);
            }
            if(res.message.Contains("duplicated"))
            {
                return this.StatusCode(StatusCodes.Status409Conflict, res);
            }
            if (res.message.Contains("wrong format"))
            {
                return this.StatusCode(StatusCodes.Status422UnprocessableEntity, res);
            }
            return this.StatusCode(StatusCodes.Status500InternalServerError, "phone number processing failed");
        }

        [HttpPost("Upload/")]
        public async Task<ActionResult<List<UpFileResponse>>> UploadFile(UpFile request)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(request.FileContent);
            string csvRawContent = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);

            if (!csvRawContent.StartsWith("id,sms_phone"))
            {
                var tempResponse = new List<UpFileResponse>();
                tempResponse.Add(new UpFileResponse { message = "Wrong CSV format" });
                return this.StatusCode(StatusCodes.Status415UnsupportedMediaType, tempResponse);
            }

            string csvContent = csvRawContent.Replace("\r\n", "\n").Replace("\r", "\n");

            var csvLines = csvContent.Split('\n').Skip(1);

            var csvEntries = new List<Tuple<string, string>>();
            var csvResults = new List<UpFileResponse>();

            foreach (string line in csvLines)
            {
                var props = line.Split(',');
                csvEntries.Add(Tuple.Create(props[0], props[1]));
            }

            foreach (var entry in csvEntries)
            {
                var res = await AddNumber(new Number {Id = Guid.Empty, PhoneNumber = entry.Item2});
                csvResults.Add(new UpFileResponse { number = entry.Item2, message = res.message });
            }

            await _context.SaveChangesAsync();

            return csvResults;
        }

        public async Task<NumberResponse> AddNumber(Number num)
        {
            String status = "";
            var currentNumber = "";
            Regex baseRgx = new Regex(@"^[0-9]{11}$");
            Regex delRgx = new Regex(@"^[0-9]{11}_([A-Za-z]+)_([0-9]+)$");

            if (delRgx.IsMatch(num.PhoneNumber))
            {
                status = status + " - Removed concatenated _DELETED_ entry";
                var splittedNum = num.PhoneNumber.Split("_");
                currentNumber = splittedNum[0];
            }
            else
            {
                currentNumber = num.PhoneNumber;
            }

            if (currentNumber.Contains("_DELETED_"))
            {
                status = status + " - Removed _DELETED_ entry";
            }
            currentNumber = currentNumber.Replace("_DELETED_", "");


            if (baseRgx.IsMatch(currentNumber))
            {
                num.PhoneNumber = currentNumber;

                var n_elem = _context.Numbers.FirstOrDefault(o => o.PhoneNumber == num.PhoneNumber);
                if (n_elem != null)
                {
                    return new NumberResponse { message = "duplicated" + status, serverNumber = n_elem };
                }
                else
                {
                    var elem = _context.Numbers.FirstOrDefault(o => o.Id == num.Id);
                    if (elem == null)
                    {
                        if(num.Id == Guid.Empty)
                        {
                            num.Id = Guid.NewGuid();
                        }
                        num.TimeStamp = DateTime.Now;
                        _context.Add(num);
                        return new NumberResponse { message = "created" + status, serverNumber = num };
                    }
                    else
                    {
                        num.TimeStamp = DateTime.Now;
                        _context.Entry(elem).CurrentValues.SetValues(num);
                        return new NumberResponse { message = "updated" + status, serverNumber = num };
                    }
                }
            }
            else
            {
                return new NumberResponse { message = "wrong format", serverNumber = new Number { } };
            }
        }
    }
}
