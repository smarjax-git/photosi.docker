
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

using PhotoSi.Catalog.Data;

namespace photosi.catalog
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
            Console.WriteLine(builder.Configuration.GetConnectionString("CatalogDb"));
            Console.ResetColor();

            builder.Services.AddDbContext<CatalogDbContext>(
                        options => options.UseSqlServer(builder.Configuration.GetConnectionString("CatalogDb"))
                    );

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            //builder.Services.AddSwaggerGen();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "PhotoSi.Catalog", Version = "v1" });
            });

            // Set the JSON serializer options
            builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
            {
                options.SerializerOptions.PropertyNameCaseInsensitive = false;
                options.SerializerOptions.PropertyNamingPolicy = null;
                options.SerializerOptions.WriteIndented = true;
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                //app.UseSwaggerUI(c =>
                //{
                //    c.SwaggerEndpoint("/swagger/v1/swagger.json", "PhotoSi.Catalog V1");
                //    c.RoutePrefix = string.Empty;
                //});
            }


            //app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<CatalogDbContext>();

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