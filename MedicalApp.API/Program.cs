using MedicalApp.BL.MapperConfig;
using MedicalApp.DA.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using MedicalApp.BL.MapperConfig;
using Serilog;
using MedicalApp.DA.UnitOfWorks;
using MedicalApp.DA.Repositories.Custom;
using MedicalApp.DA.Interfaces;
using MedicalApp.BL.Interfaces;
using Microsoft.AspNetCore.Identity;
using MedicalApp.BL.Services;

namespace MedicalApp.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            #region Serilog Config
            Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();
            #endregion
            var builder = WebApplication.CreateBuilder(args);
            builder.Host.UseSerilog();

            # region DbContext Config
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseLazyLoadingProxies() 
                       .UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            
            #endregion
            #region Services Config
            builder.Services.AddControllers();
            builder.Services.AddOpenApi();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Medical App API",
                    Version = "v1",
                    Description = "API documentation for Medical App",
                    Contact = new OpenApiContact
                    {
                        Name = "Sara Yasser",
                        Email = "sarahyasser979@gmail.com"
                    }
                });
            });
            builder.Services.AddAutoMapper(cfg => cfg.AddProfile<MappConfig>());
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();
            builder.Services.AddScoped<IPatientRepository, PatientRepository>();
            builder.Services.AddScoped<AuthService>();
            #endregion
            #region Identity Config 
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();
            #endregion

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Medical App API v1");
                    c.RoutePrefix = "swagger";
                });
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
