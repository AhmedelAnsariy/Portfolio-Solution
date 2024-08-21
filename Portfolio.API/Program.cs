using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Portfolio.API.Errors;
using Portfolio.API.Helper;
using Portfolio.API.Middlewares;
using Portfolio.Core.Interfaces;
using Portfolio.Repository.Data;
using Portfolio.Repository.Repositories;

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




            var app = webApplicationbuilder.Build();


            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var _context = services.GetRequiredService<DataDbContext>();

            var loggerFactory = services.GetRequiredService<ILoggerFactory>();

            try
            {
                await _context.Database.MigrateAsync();
                await DataDbContextSeed.SeedAsync(_context);
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

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
