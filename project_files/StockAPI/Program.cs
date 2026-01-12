using Helpers;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var apiEndpoints = app.MapGroup("/api/v1");

if (!File.Exists(DatabaseHelper.fileLocation))
{
    DatabaseHelper.InitializeDatabase();
    DatabaseHelper.AddCategories();
    DatabaseHelper.AddProducts();
}

apiEndpoints.MapPost("/products/", DatabaseHelper.AddProduct);

apiEndpoints.MapGet("/products/", DatabaseHelper.ReadAllProducts);

apiEndpoints.MapGet("/products/{id}", DatabaseHelper.ReadProduct);

apiEndpoints.MapPut("/products/{id}", DatabaseHelper.UpdateProduct);

apiEndpoints.MapDelete("/products/{id}", DatabaseHelper.DeleteProduct);

apiEndpoints.MapPost("/products/{id}/stock/add", (int id, StockInput body) => 
            {
            return DatabaseHelper.EditStock(id, body.amount);
            });

apiEndpoints.MapPost("/products/{id}/stock/remove", (int id, StockInput body) => 
            {
            return DatabaseHelper.EditStock(id, body.amount * -1);
            });

app.Run();
