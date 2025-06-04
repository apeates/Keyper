using Keyper.Api.Context;
using Keyper.Data.DTOs;
using Keyper.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Keyper.Web.Entities;


var builder = WebApplication.CreateBuilder(args);


// Add CORS policy (Web projesi 5128 portunda)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .WithOrigins("http://localhost:5128") // Web projesi portu
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddDbContext<KeyperContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("KeyperDbConnection"));
});

builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<KeyperContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;

    options.User.RequireUniqueEmail = true;
    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyz0123456789_";

    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(6);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    options.SignIn.RequireConfirmedEmail = false;
});

// Cookie Authentication yapılandırması
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/account/login"; // istenirse değiştirilebilir
        options.Cookie.Name = "keyper_auth";
        options.Cookie.HttpOnly = true;
        options.Cookie.SameSite = SameSiteMode.Strict;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    });

builder.Services.AddAuthorization();

var app = builder.Build();




if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors("AllowFrontend");
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

// Register endpoint
app.MapPost("/account/register", async ([FromBody] UserDtoForRegister userDtoForRegister, UserManager<AppUser> userManager) =>
{
    var newUser = new AppUser
    {
        UserName = userDtoForRegister.UserName,
        Email = userDtoForRegister.Email,
        FirstName = userDtoForRegister.FirstName,
        LastName = userDtoForRegister.LastName
    };

    var result = await userManager.CreateAsync(newUser, userDtoForRegister.Password);

    if (result.Succeeded)
        return Results.Ok();

    var errors = result.Errors.Select(e => e.Description).ToList();
    return Results.BadRequest(errors);
});

// Login kontrolü (sadece şifre doğrulama)
app.MapPost("/account/check-login", async ([FromBody] UserDtoForLogin userDtoForLogin, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager) =>
{
    var user = await userManager.FindByNameAsync(userDtoForLogin.UserName);
    if (user == null)
        return Results.BadRequest(new { message = "Kullanıcı bulunamadı" });

    var result = await signInManager.CheckPasswordSignInAsync(user, userDtoForLogin.Password, false);
    if (!result.Succeeded)
        return Results.BadRequest(new { message = "Kullanıcı adı veya şifre yanlış" });
    

    return Results.Ok(); // Cookie web tarafında oluşturulacak
});
app.MapGet("/getUserId/{username}", async (string username, KeyperContext context) =>
{
    var user = await context.AppUsers.FirstOrDefaultAsync(u => u.UserName == username);
    if (user == null)
        return Results.NotFound("Kullanıcı bulunamadı");

    return Results.Ok(user.Id); // sadece ID dönüyor
});


// Kullanıcıya ait site bilgileri (cookie üzerinden erişim yapılacak)
app.MapGet("/getUserSites/{username}", async ( KeyperContext context, string username) =>
{
    Console.WriteLine("API tarafında gelen kullanıcı: " + username);

    if (string.IsNullOrEmpty(username))
        return Results.Unauthorized();

    var user = await context.AppUsers
        .Include(u => u.SiteInfos)
        .FirstOrDefaultAsync(u => u.UserName == username);

    if (user == null)
        return Results.NotFound(new { message = "Kullanıcı bulunamadı" });

    return Results.Ok(user.SiteInfos.Select(s => new
    {
        s.Id,
        s.SiteName,
        s.SiteUserName,
        s.SitePassword
    }));
});

#region Crud işlemleri
app.MapPost("/sites",  async ([FromBody] SiteInfoDtoForCreate siteInfoDtoForCreate,KeyperContext context) =>
{
    var newSiteInfo = new SiteInfo()
    {
        SiteName = siteInfoDtoForCreate.SiteName,
        SiteUserName = siteInfoDtoForCreate.SiteUserName,
        SitePassword = siteInfoDtoForCreate.SitePassword,
        AppUserId = siteInfoDtoForCreate.UserId,
    };
    context.SiteInfos.Add(newSiteInfo);
    await context.SaveChangesAsync();
    return Results.Created($"/sites/{newSiteInfo.Id}", newSiteInfo);
});


app.MapPut("/sites/{id}",  async ([FromRoute] int id,SiteInfoDtoForUpdate siteInfoDtoForUpdate,KeyperContext context) =>
{
   var site = await context.SiteInfos.FindAsync(id);
    if (site == null)
        return Results.NotFound(new { message = "Site bulunamadı" });

    // Güncelleme işlemleri
    site.SiteName = siteInfoDtoForUpdate.SiteName ?? site.SiteName;
    site.SiteUserName = siteInfoDtoForUpdate.SiteUserName ?? site.SiteUserName;
    site.SitePassword = siteInfoDtoForUpdate.SitePassword ?? site.SitePassword;

    context.SiteInfos.Update(site);
    await context.SaveChangesAsync();

    Console.WriteLine("API Site güncellendi: " + site.SiteName);

    return Results.Ok(site);
});


app.MapDelete("/sites/{id:int}", async (int id, KeyperContext context) =>
{
    var site = await context.SiteInfos.FindAsync(id);
    if (site == null)
    {
        return Results.NotFound(new { message = "Site bulunamadı" });
    }
    context.SiteInfos.Remove(site);
    context.SaveChanges();
    return Results.Ok();
});


#endregion




app.Run();
