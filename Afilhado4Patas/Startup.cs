﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Afilhado4Patas.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Afilhado4Patas.Areas.Identity.Services;
using Microsoft.Extensions.Logging;
using System.Data.SqlClient;
using System.IO;
using System.Globalization;
using Microsoft.AspNetCore.Localization;

namespace Afilhado4Patas
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddIdentity<Utilizadores, IdentityRole>(config =>
            {
                config.SignIn.RequireConfirmedEmail = true;
                config.Password.RequireDigit = true;
                config.Password.RequiredLength = 8;
                config.Password.RequireNonAlphanumeric = false;
                config.Password.RequireUppercase = false;
                config.Password.RequireLowercase = true;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();
            
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            // Automatically perform database migration
            try
            {
                services.BuildServiceProvider().GetService<ApplicationDbContext>().Database.Migrate();
            }
            catch (SqlException e) {
                // log information about exception
            }
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);            
            services.AddSingleton<EmailSender>();
            services.AddScoped<RazorView>();

            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.SupportedUICultures = new[] { new CultureInfo("pt-PT")};
                options.SupportedCultures = new[] { new CultureInfo("pt-PT") };
                options.DefaultRequestCulture = new RequestCulture("pt-PT");
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,ILoggerFactory loggerFactory)
        {
            string users = "\\Utilizadores";
            string path = env.WebRootPath + users;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string animais = "\\Animais";
            string pathAnimais = env.WebRootPath + animais;
            if (!Directory.Exists(pathAnimais))
            {
                Directory.CreateDirectory(pathAnimais);
            }

            string pedidosAdocao = "\\PedidosAdocao";
            string pathPedidosAdocao = env.WebRootPath + pedidosAdocao;
            if (!Directory.Exists(pathPedidosAdocao))
            {
                Directory.CreateDirectory(pathPedidosAdocao);
            }

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Guest/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Guest}/{action=Index}/{id?}");
            });
        }
        
        
    }
}
