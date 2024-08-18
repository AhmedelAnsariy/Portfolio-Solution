using Microsoft.EntityFrameworkCore;
using Portfolio.Repository.Data;

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





            var app = webApplicationbuilder.Build();


            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var _context = services.GetRequiredService<DataDbContext>();

            var loggerFactory = services.GetRequiredService<ILoggerFactory>();

            try
            {
                await _context.Database.MigrateAsync();
            }catch (Exception ex)
            {
                var loogger = loggerFactory.CreateLogger<Program>();
                loogger.LogError(ex, "Error in Update Database");
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
