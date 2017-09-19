using System;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using STS.Data;
using STS.Models;
using STS.Services;

namespace STS
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
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders()
            .AddIdentityServer(); // Needed!!!

            services.AddIdentityServer()
                .AddDeveloperSigningCredential() // for development only
                                                 // .AddSigningCredential("CN=sts") // Cert name is sts if that is in your certificate store.  Use AddSigningCredential with self signed cert instead of AddDeveloperSigningCredential 
                                                 // azure - upload pfx file via the portal settings
                .AddInMemoryIdentityResources(Config.GetIdentityResources())
                .AddInMemoryApiResources(Config.GetApiResources()) // Wires up IResourceStore
                .AddInMemoryClients(Config.GetClients())  // Wires up IClientStore                
                .AddAspNetIdentity<ApplicationUser>();

            services.Configure<PasswordHasherOptions>(options =>
            {
                options.IterationCount = 20000; // default is 10000
            });

            services.Configure<IdentityOptions>(options =>
            {
                options.Lockout.MaxFailedAccessAttempts = 5; // 5 is the default
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5); // 5 minutes is the default time as to how long the user is locked out
                options.Lockout.AllowedForNewUsers = true;
                //options.Password.RequiredLength = 10; // mpp - This doesn't seem to work.  Not sure why? In RegisterViewModel, it has hard-coded MinimumLength = 6
                //options.SignIn.RequireConfirmedEmail = true; // mpp - make this true after implementing EmailSender.  Changes needed in AccountController as well (see minute 53 from video from Brock Allen: https://vimeo.com/172009501)
            });

            services.AddAuthentication()
                .AddGoogle(options =>
                {
                    options.ClientId = Configuration["google:ClientId"];
                    options.ClientSecret = Configuration["google:ClientSecret"];
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
                // For secrets config, see: https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?tabs=visual-studio#security-app-secrets
                var builder = new ConfigurationBuilder();
                builder.AddUserSecrets<Startup>();

                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            //app.UseAuthentication();  Should not use when using app.UseIdentityServer()            

            app.UseIdentityServer();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
