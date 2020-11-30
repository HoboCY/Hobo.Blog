using System;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using AutoMapper;
using Blog.Data;
using Blog.Model;
using Blog.MVC.Mails;
using Blog.MVC.Options;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Tencent.COS.SDK;

namespace Blog.MVC
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public static readonly ILoggerFactory EfLoggerFactory
    = LoggerFactory.Create(builder => { builder.AddConsole(); });

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(HtmlEncoder.Create(UnicodeRanges.All));   //解决Html中文编码问题

            services.AddDbContext<BlogDbContext>(options =>
            {
                options.UseMySql(connectionString: Configuration.GetConnectionString("Default"),
                                 mySqlOptionsAction: x =>
                                                     {
                                                         x.MigrationsAssembly("Blog.Data");
                                                         x.EnableRetryOnFailure(3, TimeSpan.FromSeconds(10), null);
                                                     });
                options.UseLoggerFactory(EfLoggerFactory);
                options.UseLazyLoadingProxies();
            })
            .AddIdentity<ApplicationUser, ApplicationRole>()
            .AddEntityFrameworkStores<BlogDbContext>()
            .AddDefaultUI()
            .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                // Lockout Settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(2);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User Settings
                options.User.RequireUniqueEmail = true;

                // ClaimsIdentity Settings
                options.ClaimsIdentity.UserIdClaimType = "UserId";

                // SignIn Settings
                options.SignIn.RequireConfirmedEmail = true;
                options.SignIn.RequireConfirmedPhoneNumber = false;
            });

            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = "Hobo.Blog.Cookie";
                options.Cookie.HttpOnly = false;    //客户端脚本是否可以访问Cookie
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                options.LoginPath = "/Account/Login";
                options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
                options.SlidingExpiration = true;
            });

            services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));
            services.Configure<TencentCloudSettings>(Configuration.GetSection("TencentCloudSettings"));

            services.AddTransient<IEmailSender, EmailSender>();

            services.AddCos();

            services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);
            services.AddHttpContextAccessor();

            services.AddControllersWithViews()
                .AddRazorRuntimeCompilation();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Post}/{action=Index}/{id?}");
            });
        }
    }
}