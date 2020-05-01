using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using System.Threading.Tasks;
using UserAuthentication.Data;
using UserAuthentication.Interfaces;
using UserAuthentication.Models.ViewModels;
using UserAuthentication.Services.Users;

namespace UserAuthentication
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IConfiguration _configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            // Add base Db context
            services.AddDbContext<UserAuthenticationDb>(options =>
            {
                options.UseSqlServer(_configuration.GetConnectionString("UserAuthenticationDb"));
            });
            // Add identity (core), choose some very loose password settings
            IdentityBuilder builder = services.AddIdentityCore<ManagRUser>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 3;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
            });

            // Add the overrides to the profile service
            services.AddScoped<IIDAuthService, IDAuthService>();
            services.AddTransient<IProfileService, ManagRProfileService>();

            // Create a builder for Identity
            builder = new IdentityBuilder(builder.UserType, typeof(IdentityRole), builder.Services);
            builder.AddEntityFrameworkStores<UserAuthenticationDb>();
            builder.AddRoleValidator<RoleValidator<IdentityRole>>();
            builder.AddRoleManager<RoleManager<IdentityRole>>();
            builder.AddSignInManager<SignInManager<ManagRUser>>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(config =>
            {
                var secretBytes = Encoding.UTF8.GetBytes(Constants.Secret);
                var key = new SymmetricSecurityKey(secretBytes);

                config.Events = new JwtBearerEvents()
                {
                    // Always get token from request
                    OnMessageReceived = context =>
                    {
                        if (context.Request.Query.ContainsKey("access_token"))
                        {
                            context.Token = context.Request.Query["access_token"];
                        }

                        return Task.CompletedTask;
                    }
                };

                // Validation params
                config.TokenValidationParameters = new TokenValidationParameters()
                {
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = Constants.Issuer,
                    ValidAudience = Constants.Audience,
                    IssuerSigningKey = key
                };
            });

            // After identity has been added, add identity server using in memory resources and  
            // And a custom user and user profile service
            services.AddIdentityServer()
                .AddInMemoryApiResources(IdentityServerConfig.GetApis())
                .AddInMemoryClients(IdentityServerConfig.GetClients())
                .AddAspNetIdentity<ManagRUser>()
                .AddProfileService<ManagRProfileService>()
                .AddDeveloperSigningCredential();

            services.AddScoped<IUsersService, UsersService>();

            // Add cors to prevent CORS attacks
            services.AddCors(options =>
            {
                options.AddPolicy("ManagRAppServices",
                builder =>
                {
                    builder.WithOrigins("https://localhost:4200",
                                        "http://localhost:4200",
                                        "http://localhost:4200/register")
                                        .AllowAnyOrigin()
                                        .AllowAnyMethod()
                                        .AllowAnyHeader();
                });
            });

            services.AddHttpClient();
            
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseCors("ManagRAppServices");

            app.UseIdentityServer();

            app.UseEndpoints(endpoints =>
            {
                // default controller notation
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
