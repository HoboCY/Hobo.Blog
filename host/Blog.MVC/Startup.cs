using System;
using Blog.Data;
using Blog.MVC.Mails;
using Blog.MVC.Options;
using Blog.Extensions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Tencent.COS.SDK;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Linq;
using Blog.Data.Repositories;
using Blog.Service;
using Blog.Service.Categories;
using Blog.Shared;
using MySqlConnector;

namespace Blog.MVC
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

            //services.Configure<WebEncoderOptions>(options =>
            //                                      {
            //                                          options.TextEncoderSettings =
            //                                              new TextEncoderSettings(UnicodeRanges.All);
            //                                      });

            //services.ConfigureApplicationCookie(options =>
            //{
            //    options.Cookie.Name = "996BUG-Cookie";
            //    options.Cookie.HttpOnly = false;    //客户端脚本是否可以访问Cookie
            //    options.ExpireTimeSpan = TimeSpan.FromDays(1);
            //    options.LoginPath = "/Account/Login";
            //    options.AccessDeniedPath = "/Account/AccessDenied";
            //    options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
            //    options.SlidingExpiration = true;
            //});

            services.AddScoped<IDateTimeResolver>(d => new DateTimeResolver(TimeSpan.FromHours(8).ToString()));

            services.Configure<BlogSettings>(Configuration.GetSection("BlogSettings"));
            services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));
            services.Configure<TencentCloudSettings>(Configuration.GetSection("TencentCloudSettings"));

            services.AddTransient<IDbHelper, DbHelper>();
            services.AddTransient<IRepository, Repository>();

            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<IPostService, PostService>();

            services.AddTransient<IEmailSender, EmailSender>();

            services.AddCos();

            //services.AddAntiforgery(options =>
            //                        {
            //                            options.Cookie.Name = BlogConsts.CsrfName;
            //                            options.FormFieldName = $"{BlogConsts.CsrfName}-INPUT";
            //                            options.HeaderName = "X-XSRF-TOKEN";
            //                        });

            services.AddControllersWithViews(options =>
            {
                //options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            });
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
                app.UseExceptionHandler("/error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
                app.UseStatusCodePagesWithReExecute("/error", "?statuscode={0}");
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