using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Tencent.COS.SDK;
using Blog.Data.Repositories;
using Blog.Data.TypeHandlers;
using Blog.Extensions;
using Blog.Permissions;
using Blog.Service.Categories;
using Blog.Service.Mails;
using Blog.Service.Posts;
using Blog.Service.Users;
using Blog.Shared;
using Dapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Blog.Permissions.AuthorizationHandlers;
using Microsoft.AspNetCore.Authorization.Infrastructure;

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

            services.AddAuthorization(options =>
            {
                options.AddPolicy(BlogPermissions.Posts.Create, policy => policy.AddRequirements(new OperationAuthorizationRequirement { Name = BlogPermissions.Posts.Create }));
                options.AddPolicy(BlogPermissions.Posts.Update, policy => policy.AddRequirements(new OperationAuthorizationRequirement { Name = BlogPermissions.Posts.Update }));
                options.AddPolicy(BlogPermissions.Posts.Delete, policy => policy.AddRequirements(new OperationAuthorizationRequirement { Name = BlogPermissions.Posts.Delete }));
                options.AddPolicy(BlogPermissions.Posts.Recycle, policy => policy.AddRequirements(new OperationAuthorizationRequirement { Name = BlogPermissions.Posts.Recycle }));
                options.AddPolicy(BlogPermissions.Posts.Restore, policy => policy.AddRequirements(new OperationAuthorizationRequirement { Name = BlogPermissions.Posts.Restore }));
            });

            services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();

            SqlMapper.AddTypeHandler(typeof(List<int>), new JsonTypeHandler());
            SqlMapper.AddTypeHandler(typeof(List<string>), new JsonTypeHandler());
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

            services.AddControllersWithViews(options =>
            {
                options.SuppressAsyncSuffixInActionNames = false;
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
                app.UseExceptionHandler(async context => await MiddlewareExtensions.HandleExceptionAsync(context));
                app.UseHsts();
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