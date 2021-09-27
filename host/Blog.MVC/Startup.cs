using System;
using System.Collections.Generic;
using System.Text;
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
using Blog.Service.Users;
using Blog.Shared;
using Dapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

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
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.SaveToken = true;
                    options.RequireHttpsMetadata = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = true,
                        ValidateIssuer = true,
                        ValidAudience = Configuration["JwtOptions:Audience"],
                        ValidIssuer = Configuration["JwtOptions:Issuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtOptions:Key"]))
                    };
                });

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

            services.AddTransient<IUserService, UserService>();
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<IPostService, PostService>();
            services.AddTransient<IEmailService, EmailService>();

            services.AddCors(opt =>
            {
                opt.AddPolicy("default", builder =>
                {
                    builder.WithOrigins(Configuration["Cors:Origin"].Split(";"))
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });

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
            }).AddRazorRuntimeCompilation();
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

            app.UseCors("default");
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