
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using PhotoSi.PickupPoint.Data;

namespace photosi.pickuppoints
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddAuthorization();

            builder.Services.AddControllers(opt =>
            {
                opt.OutputFormatters.RemoveType<HttpNoContentOutputFormatter>();
                opt.OutputFormatters.RemoveType<StringOutputFormatter>();
            });

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(builder.Configuration.GetConnectionString("PickupPointsDb"));
            Console.ResetColor();

            builder.Services.AddDbContext<PickUpPointDbContext>(o => {
                o.UseSqlServer(builder.Configuration.GetConnectionString("PickupPointsDb"));
            });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();


            app.MapControllers();

            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<PickUpPointDbContext>();

                try
                {
                    db.Database.EnsureCreated();
                }
                catch
                {
                }
            }

            app.Run();
        }
    }
}
