using Microsoft.EntityFrameworkCore;
using ST10448420_CMCsystem.Data;

namespace ST10448420_CMCsystem
{
    public class Program
    {
        public static void Main(string[] args)//this would of been a ASYNC if i was using Identity framework for authentication
        {
            var builder = WebApplication.CreateBuilder(args);
            // Add Database Connection
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<AppDBContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            // Add services to the container.
            builder.Services.AddControllersWithViews();

            //add session support
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                //could defo be shorter but for clarity i made it longer
                options.IdleTimeout = TimeSpan.FromMinutes(60);//why? 60mins of inactivity:ensures users stay logged in during extended periods of use but are logged out after inactivity.
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });


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
            app.UseSession();//added this line to enable session middleware, must be before UseAuthorization(according to video)
            app.UseAuthorization();
           

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
