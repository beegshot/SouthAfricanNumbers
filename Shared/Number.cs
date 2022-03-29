using System.ComponentModel.DataAnnotations;

namespace SouthAfricanNumbers.Shared
{
    public class Number
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string PhoneNumber { get; set; } = string.Empty;

        public DateTime TimeStamp { get; set; }
    }
}
