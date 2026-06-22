using Microsoft.AspNetCore.Identity;

namespace ShoppingCart.Models
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; }
    }
}
