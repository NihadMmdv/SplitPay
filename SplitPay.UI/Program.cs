using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SplitPay.DAL.Data;
using SplitPay.DAL.Models;
using SplitPay.DAL.Repository.Interface;
using SplitPay.DAL.Repository;
using SplitPay.UI.Config;
using SplitPay.UI.Services.Interfaces;
using SplitPay.UI.Services;

namespace SplitPay.UI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
            builder.Services.AddScoped<IEmailService, EmailService>();
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

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
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
