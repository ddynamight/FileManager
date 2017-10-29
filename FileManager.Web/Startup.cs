using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FileManager.Model;
using FileManager.Web.Services;

namespace FileManager.Web
{
     public class Startup
     {
          public Startup(IConfiguration configuration)
          {
               Configuration = configuration;
          }

          public IConfiguration Configuration { get; }

          // This method gets called by the runtime. Use this method to add services to the container.
          public void ConfigureServices(IServiceCollection services)
          {
               services.AddDbContext<AppDbContext>(options =>
                   options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

               services.AddIdentity<AppUser, IdentityRole>()
                   .AddEntityFrameworkStores<AppDbContext>()
                   .AddDefaultTokenProviders();

               services.Configure<IdentityOptions>(options => {

                    //Password Settings
                    options.Password.RequireDigit = false;
                    options.Password.RequiredLength = 8;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireLowercase = false;

                    // Lockout settings
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                    options.Lockout.MaxFailedAccessAttempts = 10;
                    
                    // User settings
                    options.User.RequireUniqueEmail = true;
               });

               // Add application services.
               services.AddTransient<IEmailSender, EmailSender>();

               services.AddMvc();
          }

          // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
          public void Configure(IApplicationBuilder app, IHostingEnvironment env)
          {
               if (env.IsDevelopment())
               {
                    app.UseDeveloperExceptionPage();
                    app.UseBrowserLink();
                    app.UseDatabaseErrorPage();
               }
               else
               {
                    app.UseExceptionHandler("/Home/Error");
               }

               app.UseStaticFiles();
               app.UseAuthentication();
               app.UseMvc(routes =>
               {

                    routes.MapRoute(
                     name: "areas",
                      template: "{area:exists}/{controller=Home}/{action=Index}/{tag?}");

                    routes.MapRoute(
                     name: "default",
                     template: "{controller=Home}/{action=Index}/{tag?}");
               });
          }
     }
}
