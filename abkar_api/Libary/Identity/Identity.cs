using System.Security.Claims;

namespace abkar_api.Libary.Identity
{
    public static class Identity
    {
        public static int get(System.Security.Principal.IPrincipal User)
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            return int.Parse(claimsIdentity.FindFirst("id").Value);
        }
    }
}