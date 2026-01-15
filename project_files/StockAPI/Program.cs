using Helpers;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var apiEndpoints = app.MapGroup("/api/v2");
var products = apiEndpoints.MapGroup("/products");
var categories = apiEndpoints.MapGroup("/categories");

if (!File.Exists(DatabaseHelper.fileLocation))
{
    DatabaseHelper.InitializeDatabase();
    DatabaseHelper.AddCategories();
    DatabaseHelper.AddProducts();
}

products.MapPost("/", DatabaseHelper.AddProduct);

products.MapGet("/", DatabaseHelper.ReadAllProducts);

products.MapGet("/{id}", DatabaseHelper.ReadProduct);

products.MapPut("/{id}", DatabaseHelper.UpdateProduct);

products.MapDelete("/{id}", DatabaseHelper.DeleteProduct);

products.MapPost("/{id}/stock/add", (int id, StockInput body) =>
{
    return DatabaseHelper.EditStock(id, body.amount);
});

products.MapPost("/{id}/stock/remove", (int id, StockInput body) =>
{
    return DatabaseHelper.EditStock(id, body.amount * -1);
});

categories.MapPost("/", DatabaseHelper.AddCategory);

categories.MapGet("/", DatabaseHelper.ReadAllCategories);

categories.MapGet("/{id}", DatabaseHelper.ReadCategory);

categories.MapPut("/{id}", DatabaseHelper.UpdateCategory);

categories.MapDelete("/{id}", DatabaseHelper.DeleteCategory);

app.UseDefaultFiles();
app.UseStaticFiles();
app.MapStaticAssets().ShortCircuit();


app.Run();
