using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace DataLibrary.Models
{

    public partial class User : IdentityUser, IEntity
    {
        public bool Active { get; set; }
        public DateTime? Birthdate { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Roles { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<AppTask> Tasks { get; set; }

        public User()
        {
            Tasks = new HashSet<AppTask>();
            Comments = new HashSet<Comment>();
        }
    }
}
