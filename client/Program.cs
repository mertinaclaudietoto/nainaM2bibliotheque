using Client.Repositorys;
var builder = WebApplication.CreateBuilder(args);

// Connection string SQL Server
string connectionString = builder.Configuration.GetConnectionString("_connectionString");

// Ajouter le repository au conteneur DI
builder.Services.AddSingleton(new UserRepository(connectionString));
builder.Services.AddSingleton(new LivreRepository(connectionString));


// Add services to the container.
builder.Services.AddRazorPages();
// 🔹 session 

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".Bibliophilia.Session";
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Ajout du HttpClientFactory
builder.Services.AddHttpClient();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();
// 🔹 Ajouter l’utilisation de la session
app.UseSession();


app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
