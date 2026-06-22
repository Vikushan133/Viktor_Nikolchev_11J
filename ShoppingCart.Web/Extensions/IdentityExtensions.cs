using System.Security.Claims;

namespace ShoppingCart.Web.Models
{
    public static class IdentityExtensions
    {
        public static string GetUserId(this ClaimsPrincipal principal)
        {
            return principal.FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }
}
