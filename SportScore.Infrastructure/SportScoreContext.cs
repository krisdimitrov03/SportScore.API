using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SportScore.Infrastructure.Data;

namespace SportScore.Infrastructure;

public class SportScoreContext : IdentityDbContext<ApplicationUser>
{
    public SportScoreContext(DbContextOptions<SportScoreContext> options)
        : base(options)
    {
    }

    public DbSet<UserFavouriteMatch> UsersFavouriteMatches { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
        //line not important
    }
}
