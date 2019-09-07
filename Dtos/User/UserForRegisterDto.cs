using System;
using System.ComponentModel.DataAnnotations;

namespace telebibcore22.api.Dtos.User
{
    public class UserForRegisterDto
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(36, MinimumLength = 4, ErrorMessage = "You must specify a password between 8 and 36 characters")]
        public string Password { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        public string KnownAs { get; set; }
        [Required]
        public string DateOfBirth { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Country { get; set; }
        public string BerNumber { get; set; }

        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }
        public string ClientNumber { get; set; }
        public UserForRegisterDto()
        {
            Created = DateTime.Now;
            LastActive = DateTime.Now;
            ClientNumber = "220750"; // Demo client
        }
    }
}
