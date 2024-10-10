using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SplitPay.DAL.Data;
using SplitPay.DAL.Models;

namespace SplitPay.UI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<CustomDBContext>(
                opts => opts.UseSqlServer(builder.Configuration.GetConnectionString("SqlConStr")));
            builder.Services.AddIdentity<AppUser, AppRole>(opts =>
            {

                opts.Password.RequireDigit = true;
                opts.Password.RequireLowercase = true;
                opts.Password.RequireNonAlphanumeric = false;
                opts.Password.RequireUppercase = false;
                opts.Password.RequiredLength = 8;

            }).AddEntityFrameworkStores<CustomDBContext>().AddDefaultTokenProviders();
            builder.Services.Configure<DataProtectionTokenProviderOptions>(opts =>
            {
                opts.TokenLifespan = TimeSpan.FromHours(2);
            });
            builder.Services.ConfigureApplicationCookie(config =>
            {

                CookieBuilder cookieBuilder = new CookieBuilder()
                {
                    Name = "SplitPayUI",
                    Path = "/",

                };
                config.Cookie = cookieBuilder;
                config.ExpireTimeSpan = TimeSpan.FromDays(2);
                config.LoginPath = "/Account/LogIn";

            });

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

            app.Run();
        }
    }
}
