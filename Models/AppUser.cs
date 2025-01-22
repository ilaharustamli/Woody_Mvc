using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace Woody_Mvc.Models
{
    public class AppUser :IdentityUser
    {
        public string Name { get; set; }
        public string SurName { get; set; }
    }
}
