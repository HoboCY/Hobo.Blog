using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
using Blog.Service.Menus;
using Blog.Service.Roles;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.WebEncoders;

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

            services.AddAuthorization(options =>
            {
                var permissions = BlogPermissionsExtensions.GetPermissions();
                permissions.ForEach(p => options.AddPolicy(p, policy => policy.AddRequirements(new OperationAuthorizationRequirement { Name = p })));
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
            services.AddTransient<IRoleService, RoleService>();
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<IPostService, PostService>();
            services.AddTransient<IMenuService, MenuService>();
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
            })
                .ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var error = context.ModelState.BuildErrors();
                    return new BadRequestObjectResult(error);
                };
            })
                .AddRazorRuntimeCompilation();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseExceptionHandler(async context => await MiddlewareExtensions.HandleExceptionAsync(context));

            app.UseHsts();

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