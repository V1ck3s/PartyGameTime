using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using PartyGameTime.Core.Data;
using Microsoft.AspNetCore.Identity;
using PartyGameTime.Core.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using PartyGameTime.Core.Auth;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddMudServices();
builder.Services.AddSingleton<GlobalValuesManager>();


builder.Services.AddDbContext<PgtDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("PgtDbContext")));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Authentication services
builder.Services.AddIdentity<Account, AccountRole>(options =>
    {
        // Identity settings
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireNonAlphanumeric = true;
        options.Password.RequiredLength = 8;

        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
        options.Lockout.MaxFailedAccessAttempts = 5;

        options.SignIn.RequireConfirmedEmail = false;
        options.SignIn.RequireConfirmedAccount = false;

        // User settings
        options.User.AllowedUserNameCharacters =
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
        options.User.RequireUniqueEmail = false;
    })
    .AddEntityFrameworkStores<PgtDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthorization();




builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/auth/login";
        options.LogoutPath = "/auth/logout";
        options.AccessDeniedPath = "/auth/accessdenied";
    });
// Authentication services end

// Services injection
builder.Services.AddScoped<UserManager<Account>>();
builder.Services.AddScoped<RoleManager<AccountRole>>();
builder.Services.AddScoped<SignInManager<Account>>();
//builder.Services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();
builder.Services.AddScoped<AccountManager>();



// Services injection end

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<PgtDbContext>();
    context.Database.EnsureCreated();
}



app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

// Authentication
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<BlazorCookieLoginMiddleware>();
// Authentication end

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
