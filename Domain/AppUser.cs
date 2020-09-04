using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Domain
{
    public class AppUser : IdentityUser 
    {
        public string DisplayName { get; set; }

        public string DODId { get; set; }

       // public ICollection<UserActivity> UserActivities { get; set; }   // Before Lazy Loading 
         public virtual ICollection<UserActivity> UserActivities { get; set; } 
    }
}