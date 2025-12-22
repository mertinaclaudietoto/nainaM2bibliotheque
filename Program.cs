using Client.Repositorys;
var builder = WebApplication.CreateBuilder(args);

// Connection string SQL Server
string connectionString = builder.Configuration.GetConnectionString("_connectionString");

// Ajouter le repository au conteneur DI
builder.Services.AddSingleton(new UserRepository(connectionString));
builder.Services.AddSingleton(new LivreRepository(connectionString));


// Add services to the container.
builder.Services.AddRazorPages();

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

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
