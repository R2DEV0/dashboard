using Microsoft.AspNetCore.Identity;

namespace dash.Entities
{
    public class UserAccount : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        // add list of permission ids for permission tree
    }
}
