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

internal class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();

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

        // Add your existing services
        builder.Services.AddDbContext<AppDBContext>(options => options.UseSqlServer(
                builder.Configuration.GetConnectionString("DefaultConnection")
            ));

        builder.Services.AddScoped<ProductService>();
        builder.Services.AddScoped<UserService>();
        builder.Services.AddScoped<CartService>();
        // Replace the default UserClaimsPrincipalFactory with the custom one

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

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
                // Log the error if necessary
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