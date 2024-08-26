using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Portfolio.API.Errors;
using Portfolio.API.Helper;
using Portfolio.API.Middlewares;
using Portfolio.Core.Identity;
using Portfolio.Core.Interfaces;
using Portfolio.Core.Services.Interfaces;
using Portfolio.Repository.Data;
using Portfolio.Repository.Repositories;
using Portfolio.Service;
using System.Text;

namespace Portfolio.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var webApplicationbuilder = WebApplication.CreateBuilder(args);



            webApplicationbuilder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            webApplicationbuilder.Services.AddEndpointsApiExplorer();
            webApplicationbuilder.Services.AddSwaggerGen();

            webApplicationbuilder.Services.AddDbContext<DataDbContext>(options =>
            {
                options.UseSqlServer(webApplicationbuilder.Configuration.GetConnectionString("DefaultConnection"));
            });


            webApplicationbuilder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            webApplicationbuilder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            webApplicationbuilder.Services.AddAutoMapper(typeof(MappingProfiles));
            webApplicationbuilder.Services.AddScoped<ITokenService, TokenService>();



            //webApplicationbuilder.Services.AddAuthentication(options =>
            //{
            //    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //})
            //                              .AddJwtBearer( options =>
            //                              {
            //                                  options.TokenValidationParameters = new TokenValidationParameters()
            //                                  {
            //                                      ValidateIssuer = true,
            //                                      ValidIssuer = webApplicationbuilder.Configuration["JWT:ValidIssure"],
            //                                      ValidateAudience = true,
            //                                      ValidAudience = webApplicationbuilder.Configuration["JWT:ValidAudience"],
            //                                      ValidateLifetime = true,
            //                                      ValidateIssuerSigningKey = true,
            //                                      IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(webApplicationbuilder.Configuration["JWT:Key"]))

            //                                  };


            //                              });




            //            webApplicationbuilder.Services.AddAuthentication(options =>
            //            {
            //                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //            })
            //.AddJwtBearer(options =>
            //{
            //    options.TokenValidationParameters = new TokenValidationParameters()
            //    {
            //        ValidateIssuer = true,
            //        ValidIssuer = webApplicationbuilder.Configuration["JWT:ValidIssuer"], // Fixed typo here
            //        ValidateAudience = true,
            //        ValidAudience = webApplicationbuilder.Configuration["JWT:ValidAudience"],
            //        ValidateLifetime = true,
            //        ValidateIssuerSigningKey = true,
            //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(webApplicationbuilder.Configuration["JWT:Key"]))
            //    };
            //});




            webApplicationbuilder.Services.AddAuthentication(optoins =>
            {
                optoins.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                optoins.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                                                             .AddJwtBearer(options =>
                                                             {
                                                                 options.TokenValidationParameters = new TokenValidationParameters()
                                                                 {
                                                                     ValidateIssuer = true,
                                                                     ValidIssuer = webApplicationbuilder.Configuration["JWT:ValidIssure"],
                                                                     ValidateAudience = true,
                                                                     ValidAudience = webApplicationbuilder.Configuration["JWT:ValidAudience"],
                                                                     ValidateLifetime = true,
                                                                     ValidateIssuerSigningKey = true,
                                                                     IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(webApplicationbuilder.Configuration["JWT:Key"]))

                                                                 };


                                                             });





















            webApplicationbuilder.Services.Configure<ApiBehaviorOptions>(options =>
             options.InvalidModelStateResponseFactory = (ActionContext context) =>
             {
             var errors = context.ModelState
            .Where(p => p.Value.Errors.Any())
            .SelectMany(p => p.Value.Errors)
            .Select(e => e.ErrorMessage)
            .ToArray();

        var validateErrorResponse = new ApiValidationErrorResponse
        {
            Errors = errors
        };

        return new BadRequestObjectResult(validateErrorResponse);
        });



            webApplicationbuilder.Services.AddIdentity<AppUser , IdentityRole>()
                                           .AddEntityFrameworkStores<DataDbContext>()
                                           .AddDefaultTokenProviders();





























            var app = webApplicationbuilder.Build();
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var _context = services.GetRequiredService<DataDbContext>();
            var userManager = services.GetRequiredService<UserManager<AppUser>>();


            var loggerFactory = services.GetRequiredService<ILoggerFactory>();

            try
            {
                await _context.Database.MigrateAsync();
                await DataDbContextSeed.SeedAsync(_context, userManager);
            }
            catch (Exception ex)
            {
                var loogger = loggerFactory.CreateLogger<Program>();
                loogger.LogError(ex, "Error in Update Database");
            }

            app.UseMiddleware<ExceptionMiddleware>(); // For Validation 

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            } 
            app.UseStatusCodePagesWithReExecute("/errors/{0}");  //For Not Found End Point 
            app.UseStaticFiles();
            app.UseHttpsRedirection();


            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
