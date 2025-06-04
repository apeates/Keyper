using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace Keyper.Web1.Models;

public class DashboardViewModel
{
    public int Id { get; set; }
    public string SiteName { get; set; }
    public string SiteUserName { get; set; }
    public string SitePassword { get; set; }
}