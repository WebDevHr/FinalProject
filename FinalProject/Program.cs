using FinalProject.Service.Data;
using FinalProject.Service.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using FinalProject.Service.Models;
using FinalProject.Areas;
using Microsoft.AspNetCore.Authentication.Cookies;
using FinalProject.Areas.Identity.Pages.Account.Manage;
using Microsoft.Extensions.DependencyInjection;
using FinalProject.Filters;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews(options =>
        {
            options.Filters.Add<IsAdminFilter>();
        });

        builder.Services.AddDistributedMemoryCache(); // Use in-memory cache for session storage
        builder.Services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromMinutes(30); // Set session timeout
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
        });

        

        // Add DbContext for Identity
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        // Add Identity
        builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
        {
            options.SignIn.RequireConfirmedAccount = true;
            options.ClaimsIdentity.UserNameClaimType = "FirstName";
        })
        .AddRoles<IdentityRole>() // Add this line to enable roles
        .AddEntityFrameworkStores<ApplicationDbContext>();

        builder.Services.AddTransient<IEmailSender, DummyEmailSender>();


        builder.Services.AddScoped<ProductService>();
        builder.Services.AddScoped<UserService>();
        builder.Services.AddScoped<CartService>();
        builder.Services.AddScoped<FavoriteService>();
        // Replace the default UserClaimsPrincipalFactory with the custom one

        builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseSession();

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.MapRazorPages();

        // Add this block to seed roles
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            try
            {
                await SeedRoles(services);
            }
            catch (Exception ex)
            {
                DirectoryNotFoundException directoryNotFoundException = new DirectoryNotFoundException(ex.Message);
            }
        }

        app.Run();
        
    }
    public static async Task SeedRoles(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        string[] roleNames = { "Admin", "User" };
        foreach (var roleName in roleNames)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }
    }
}