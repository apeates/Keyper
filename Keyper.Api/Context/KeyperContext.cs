using Keyper.Data.Entities;
using Keyper.Web.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Keyper.Api.Context;

public class KeyperContext : IdentityDbContext<IdentityUser, IdentityRole, string>
{
    public KeyperContext(DbContextOptions<KeyperContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<SiteInfo>().HasData( // sahte bilgilerini başlangıçta eklemek için
            new SiteInfo
            {
                Id = 1,
                // appuserid = 1, // bu kullanıcıya ait olduğunu ifade eder.
                SiteUserName = "Keyper",
                SiteName = "www.asaf.com",
                SitePassword = "Keyper1234",
                AppUserId= "1a1012a5-8116-4aec-bd9e-fc2618f100e4"
            },           
            new SiteInfo
            {
                Id = 2,
                SiteUserName = "google",
                SiteName = "www.google.com",
                SitePassword = "Keyper1234",
                AppUserId= "1a1012a5-8116-4aec-bd9e-fc2618f100e4"
            }
            
        );
    }

    public DbSet<SiteInfo> SiteInfos { get; set; }
    public DbSet<AppUser> AppUsers { get; set; }
}