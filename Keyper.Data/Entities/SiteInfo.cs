using Keyper.Data.Entities;

namespace Keyper.Web.Entities;

public class SiteInfo
{
    public int Id { get; set; }
    public string SiteName { get; set; }
    public string SiteUserName { get; set; }
    public string SitePassword { get; set; }
    
    public string AppUserId { get; set; } // Foreign key to AppUser
    public AppUser AppUser { get; set; } // Navigation property to AppUser
}