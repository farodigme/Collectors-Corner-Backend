using System.ComponentModel.DataAnnotations;

namespace Collectors_Corner_Backend.Models
{
    public class LoginModel
    {
        [Required]
        [EmailAddress]
		[MaxLength(50)]

		public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
