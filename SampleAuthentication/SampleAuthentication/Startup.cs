using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SampleAuthentication.Data;
using SampleAuthentication.Models;
using SampleAuthentication.Services;
using Microsoft.AspNetCore.Identity;
using Core;

namespace SampleAuthentication
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                // For more details on using the user secret store see https://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets<Startup>();
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            //services.AddDbContext<ApplicationDbContext>(options =>
            //    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            //services.AddIdentity<IdentityUser, IdentityRole>()
            //    .AddEntityFrameworkStores<ApplicationDbContext>()
            //    .AddDefaultTokenProviders();
            //services

            // Add application services.
            //var userStore = new UserStore<IdentityUser>();
            //var roleStore = new RoleStore();
            //var userPrincipalFactory = new ExampleUserPrincipalFactory();
            //services.AddTransient<IUserStore<IdentityUser>, UserStore<IdentityUser>>();
            //services.AddSingleton<IUserStore<IdentityUser>>(userStore);
            //services.AddSingleton<IRoleStore<IdentityRole>>(roleStore);
            //services.AddSingleton<IUserClaimsPrincipalFactory<IdentityUser>>(userPrincipalFactory);

            //var roleStore = new RoleStore();
            var userPrincipalFactory = new ExampleUserPrincipalFactory();
            services.AddSingleton<IUserStore<IdentityUser>, UserStore<IdentityUser>>();
            services.AddSingleton<IRoleStore<IdentityRole>, RoleStore<IdentityRole>>();
            services.AddSingleton<IUserClaimsPrincipalFactory<IdentityUser>>(userPrincipalFactory);

            services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@";                
            }).AddDefaultTokenProviders();

            services.AddMvc();

            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseDatabaseErrorPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationScheme = "Microsoft.AspNet.Identity.Application",
                LoginPath = new Microsoft.AspNetCore.Http.PathString("/login"),
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                SlidingExpiration = true
            });

            app.UseStaticFiles();

            app.UseIdentity();

            // Add external authentication middleware below. To configure them please see https://go.microsoft.com/fwlink/?LinkID=532715

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
