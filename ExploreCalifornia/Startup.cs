using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExploreCalifornia.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ExploreCalifornia
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(Microsoft.Extensions.DependencyInjection.IServiceCollection services)
        {
            //services.AddRazorPages();

            services.AddTransient<FormattingService>();

            services.AddTransient<FeatureToggles>(x => new FeatureToggles
            {
                DeveloperExceptions = Configuration.GetValue<bool>("FeatureToggles:DeveloperExceptions")
            });

            services.AddDbContext<BlogDataContext>(options =>
            {
                var connectionString = Configuration.GetConnectionString("BlogDataContext");
                options.UseSqlServer(connectionString);
            });
       


            services.AddDbContext<IdentityDataContext>(options =>
            {
                var connectionString = Configuration.GetConnectionString("IdentityDataContext");
                options.UseSqlServer(connectionString);
            });

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<IdentityDataContext>();

            services.AddMvc(options=> options.EnableEndpointRouting=false);
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseExceptionHandler("/error.html");
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.Use(async (context, next) =>
            {
                if (context.Request.Path.Value.Contains("invalid"))
                    throw new Exception("Error");

                await next();
            });
            app.UseAuthentication();
            app.UseRouting();
            app.UseMvc(routes =>
            {
                
                routes.MapRoute("Default", "{controller=Home}/{action=Index}/{id?}");

            });
            
            app.UseFileServer();
            //app.UseStaticFiles();

            //app.UseRouting();

            //app.UseAuthorization();

            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapRazorPages();
            //});
        }
    }
}
