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

apiEndpoints.MapGet("/products/", DatabaseHelper.ReadAllProducts);

apiEndpoints.MapGet("/products/{id}", DatabaseHelper.ReadProduct);

apiEndpoints.MapDelete("/products/{productId}", DatabaseHelper.DeleteProduct);

app.Run();
