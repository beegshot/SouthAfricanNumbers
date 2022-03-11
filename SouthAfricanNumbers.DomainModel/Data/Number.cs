using System;
using System.ComponentModel.DataAnnotations;

namespace SouthAfricanNumbers.DomainModel
{
    public class Number
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string PhoneNumber { get; set; }
    }
}