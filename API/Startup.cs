using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Persistence;
using MediatR;
using Application.Activities;
using Domain;
using Microsoft.AspNetCore.Identity;
using FluentValidation.AspNetCore;
using API.Middleware;
using Application.Interfaces;
using Infrastructure.Security;
using Application.User;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using AutoMapper;
using Infrastructure.Photos;
using Application.Photos;

namespace API
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

            //  services.AddDbContext<DataContext>(options =>
            //     options.UseSqlServer(
            //         Configuration.GetConnectionString("DefaultConnection")));

           services.AddDbContext<DataContext>(options =>
             {
                 options.UseLazyLoadingProxies();        // For Lazy Loading Microsoft.EntityFrameworkCore.Proxies
                 options.UseSqlite(
                  Configuration.GetConnectionString("DefaultConnection"));
             });

            // //  services.AddDbContext<DataContext>(options =>
            // //  {
            // //      options.UseLazyLoadingProxies();        // For Lazy Loading Microsoft.EntityFrameworkCore.Proxies
            // //      options.UseSqlServer(
            // //       Configuration.GetConnectionString("Default"));
            // //  });


            services.AddCors(opt =>
           opt.AddPolicy("CorsPloicy", policy =>
           {
               policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:3000");
           })
            );

            services.AddMediatR(typeof(List.Handler).Assembly);
            services.AddAutoMapper(typeof(List.Handler));
            //  services.AddMediatR(typeof(Login.Handler).Assembly);
            // services.AddControllers().AddFluentValidation( cfg =>
            // {
            //     cfg.RegisterValidatorsFromAssemblyContaining<Create>();
            // });

            services.AddControllers(opt =>
            {
                var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                opt.Filters.Add(new AuthorizeFilter(policy));
            }).AddFluentValidation(cfg =>
           {
               cfg.RegisterValidatorsFromAssemblyContaining<Create>();
           });
            //    .AddNewtonsoftJson(options =>
            //        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            //    );

            // var builder = services.AddIdentityCore<AppUser>();
            // var identityBuilder = new IdentityBuilder(builder.UserType, builder.Services);
            // identityBuilder.AddEntityFrameworkStores<DataContext>();
            // identityBuilder.AddSignInManager<SignInManager<AppUser>>();


            var builder = services.AddIdentityCore<AppUser>();
            var identityBuilder = new IdentityBuilder(builder.UserType, builder.Services);
            identityBuilder.AddEntityFrameworkStores<DataContext>();
            identityBuilder.AddSignInManager<SignInManager<AppUser>>();

            services.AddAuthorization(opt =>
             {
                opt.AddPolicy("IsActivityHost", policy =>
                {
                    policy.Requirements.Add(new IsHostRequirement());
                });
             });
            services.AddTransient<IAuthorizationHandler, IsHostRequirementHandler>();



            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["TokenKey"]));
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
               .AddJwtBearer(opt =>
               {
                   opt.TokenValidationParameters = new TokenValidationParameters
                   {
                       ValidateIssuerSigningKey = true,
                       IssuerSigningKey = key,
                       ValidateAudience = false,
                       ValidateIssuer = false
                   };
               });

            services.AddScoped<IJwtGenerator, JwtGenerator>();
            services.AddScoped<IUserAccessor, UserAccessor>();
            services.AddScoped<IPhotoAccessor, PhotoAccessor>();
            
            services.Configure<CloudinarySettings>(Configuration.GetSection("Cloudinary"));

        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<ErrorHandlingMiddleware>();

            if (env.IsDevelopment())
            {
                //   app.UseDeveloperExceptionPage();
            }

            // app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors("CorsPloicy");

            app.UseAuthentication();
            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
