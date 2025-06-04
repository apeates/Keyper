using Keyper.Web.Entities;
using Microsoft.AspNetCore.Identity;


namespace Keyper.Data.Entities;

public class AppUser : IdentityUser
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    
    public IEnumerable<SiteInfo> SiteInfos { get; set; }
}