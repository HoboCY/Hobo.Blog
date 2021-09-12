using System;
using System.Collections.Generic;
using Blog.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Tencent.COS.SDK;
using Blog.Data.Repositories;
using Blog.Data.TypeHandlers;
using Blog.Service;
using Blog.Service.Categories;
using Blog.Service.Mails;
using Blog.Service.Posts;
using Blog.Shared;
using Dapper;

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

            SqlMapper.AddTypeHandler(typeof(List<int>), new JsonTypeHandler());
            services.AddScoped<IDateTimeResolver>(d => new DateTimeResolver(TimeSpan.FromHours(8).ToString()));

            services.Configure<BlogSettings>(Configuration.GetSection("BlogSettings"));
            services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));
            services.Configure<TencentCloudSettings>(Configuration.GetSection("TencentCloudSettings"));

            services.AddTransient<IRepository, Repository>();

            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<IPostService, PostService>();
            services.AddTransient<IEmailService, EmailService>();

            services.AddCos();

            //services.AddAntiforgery(options =>
            //                        {
            //                            options.Cookie.Name = BlogConsts.CsrfName;
            //                            options.FormFieldName = $"{BlogConsts.CsrfName}-INPUT";
            //                            options.HeaderName = "X-XSRF-TOKEN";
            //                        });

            services.AddControllersWithViews(options =>
            {
                options.SuppressAsyncSuffixInActionNames = false;
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