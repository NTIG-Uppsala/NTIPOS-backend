using Helpers;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var apiEndpoints = app.MapGroup("/api/v1");

if (!File.Exists(DatabaseHelper.fileLocation))
{
    DatabaseHelper.InitializeDatabase();
    DatabaseHelper.AddCategories();
    DatabaseHelper.AddProducts();
}

apiEndpoints.MapGet("/products/{id}", DatabaseHelper.ReadProduct);

app.UsePathBase("/admin");
app.UseDefaultFiles();
app.UseStaticFiles();
app.MapStaticAssets().ShortCircuit();

app.Run();
