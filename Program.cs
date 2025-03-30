namespace ExempleEnvoiDonneesVue
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            //connecxion SQL  Server
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Membre}/{action=Index}/{id_livre?}");
            app.MapControllerRoute(
                name: "book_Create",
                pattern: "{controller=Livre}/{action=Create}/{id?}");
            app.Run();
        }
    }
}
