using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 🔹 Ajouter MVC
builder.Services.AddControllersWithViews();

// 🔹 Connexion SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("_connectionString")
    )
);

// 🔹 Injection du service (IC)
builder.Services.AddScoped<LivreService>();
builder.Services.AddScoped<GenreService>();
builder.Services.AddScoped<AuteurService>();
builder.Services.AddScoped<LivreDetailsService>();
builder.Services.AddScoped<LivreEmpruntService>();
builder.Services.AddScoped<UsersService>();


// 🔹 session 

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});


var app = builder.Build();

// 🔹 Pipeline HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}



app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
