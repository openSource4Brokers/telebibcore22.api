using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using telebibcore22.api.Models;
using Microsoft.AspNetCore.Identity;

namespace telebibcore22.api.Models
{
    public class User : IdentityUser<int>
    {
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string KnownAs { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }
        public string Introduction { get; set; }
        public string LookingFor { get; set; }
        public string Interests { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string BerNumber { get; set; }
        public string ClientNumber { get; set; }
        public ICollection<Photo> Photos { get; set; }
        public ICollection<Like> Likers { get; set; }
        public ICollection<Like> Likees { get; set; }
        public ICollection<Message> MessagesSent { get; set; }
        public ICollection<Message> MessagesReceived { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
        /* public User()
        {
            Photos = new Collection<Photo>();
        } */
    }
}