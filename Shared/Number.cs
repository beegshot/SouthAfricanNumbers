using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SouthAfricanNumbers.Shared
{
    public class Number
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
