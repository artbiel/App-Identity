using Identity.Api.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Identity.Api.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
    }
}
