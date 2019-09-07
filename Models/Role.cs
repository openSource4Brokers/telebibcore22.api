using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace telebibcore22.api.Models
{
    public class Role : IdentityRole<int>
    {
        public ICollection<UserRole> UserRoles { get; set; }
    }
}