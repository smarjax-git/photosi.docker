using photosi.ws.users;
using photosi.ws.catalog;
using photosi.ws.orders;
using photosi.ws.pickuppoints;

namespace photosi.api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            Console.WriteLine("************ WS PATH ***********");
            Console.WriteLine(builder.Configuration.GetValue<string>("WS:OrdersWS"));
            Console.WriteLine(builder.Configuration.GetValue<string>("WS:UsersWS"));
            Console.WriteLine(builder.Configuration.GetValue<string>("WS:CatalogWS"));
            Console.WriteLine(builder.Configuration.GetValue<string>("WS:PickupPointsWS"));
            Console.WriteLine("----------------------------------");
            // Add services to the container.
            builder.Services.AddScoped<Iorders_ws>(s => 
                        new orders_ws(builder.Configuration.GetValue<string>("WS:OrdersWS"), new HttpClient()));

            builder.Services.AddScoped<photosi.ws.users.Iusers_ws>(s => 
                        new users_ws(builder.Configuration.GetValue<string>("WS:UsersWS"), new HttpClient()));

            builder.Services.AddScoped<Icatalog_ws>(s => 
                        new catalog_ws(builder.Configuration.GetValue<string>("WS:CatalogWS"), new HttpClient()));

            builder.Services.AddScoped<Ipickuppoints_ws>(s =>
                        new pickuppoints_ws(builder.Configuration.GetValue<string>("WS:PickupPointsWS"), new HttpClient()));

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            //{
                app.UseSwagger();
                app.UseSwaggerUI();
            //}

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
