using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace STS.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        // mpp - Add properties here to have more columns in the dbo.AspNetUsers table
        // public int Age { get; set; }
        // To add this property to the claims on the cookie, you must create a class that inherits from UserClaimsPrincipleFactory<ApplicationUser>
        // See minute 36 of this video from Brock Allen: https://vimeo.com/172009501
    }
}
