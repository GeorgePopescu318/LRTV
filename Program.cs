using LRTV.ContextModels;
using LRTV.Helpers;
using LRTV.Interfaces;
using LRTV.Services;
using Microsoft.EntityFrameworkCore;

namespace LRTV
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
          //  builder.Services.AddDbContext<NewsContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("NewsDB")));
            builder.Services.AddDbContext<PlayersContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("PlayersDB")));
           // builder.Services.AddDbContext<TeamsContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("TeamsDB")));
            builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));
            builder.Services.AddScoped<IPhotoService, PhotoService>();

            var app = builder.Build();
                
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
