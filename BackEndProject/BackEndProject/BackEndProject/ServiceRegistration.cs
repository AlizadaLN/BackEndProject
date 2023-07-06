using BackEndProject.DAL;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BackEndProject
{
    public static class ServiceRegistration
    {
        public static void ServicesRegister(this IServiceCollection services, IConfiguration _config)
        {
            services.AddControllersWithViews();
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(_config.GetConnectionString("DefaultConnection"));

            });

           
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(10);

            });

            services.AddHttpContextAccessor();
          //  services.AddScoped<IBasketService, BasketService>();
            //services.AddIdentity<AppUser, IdentityRole>(options =>
            //{
            //    options.Password.RequiredLength = 8;
            //    options.Password.RequireLowercase = true;
            //    options.Password.RequireUppercase = true;
            //    options.Password.RequireDigit = true;
            //    options.Password.RequireNonAlphanumeric = true;

            //    options.User.RequireUniqueEmail = true;
            //    options.Lockout.AllowedForNewUsers = true;
            //    options.Lockout.MaxFailedAccessAttempts = 3;
            //    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
            //})
            //    .AddEntityFrameworkStores<AppDbContext>()
            //    .AddDefaultTokenProviders()
            //    .AddErrorDescriber<CustomIdentityErrorDescriber>();

        }
    }
}
