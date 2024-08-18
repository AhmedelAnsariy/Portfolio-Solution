using Microsoft.EntityFrameworkCore;
using Portfolio.Repository.Data;

namespace Portfolio.API
{
    public class Program
    {
        public static void Main(string[] args)
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
