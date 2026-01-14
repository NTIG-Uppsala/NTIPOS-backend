using System.Data.SQLite;

namespace Helpers
{
    class StockInput
    {
        public int amount {get; set;}
    }

    class Product
    {
        public int id {get; set;}
        public string? name {get; set;}
        public int category {get; set;}
        public float price {get; set;}
        public int stock {get; set;}
        public string? createdAt {get; set;}
        public string? updatedAt {get; set;}
    }

    class Category
    {
        public int id {get; set;}
        public string? name {get; set;}
        public string? color {get; set;}
        public string? createdAt {get; set;}
        public string? updatedAt {get; set;}
    }

    class DatabaseHelper
    {
        public static readonly string fileLocation = "databases/products.db";
        public static readonly string connectionString = "Data Source=" + fileLocation + ";Version=3;";

        public static void InitializeDatabase()
        {
            if (!File.Exists(fileLocation))
            {
                FileInfo file = new FileInfo(fileLocation);
                file.Directory?.Create();
                SQLiteConnection.CreateFile(fileLocation);

                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    string createCategoriesTableQuery = @"
                    CREATE TABLE IF NOT EXISTS categories(
                            id INTEGER PRIMARY KEY AUTOINCREMENT,
                            categoryName TEXT NOT NULL,
                            categoryColor TEXT NOT NULL,
                            createdAt STRING NOT NULL,
                            updatedAt STRING NOT NULL
                    );";

                    string createProductsTableQuery = @"
                    CREATE TABLE IF NOT EXISTS products(
                            id INTEGER PRIMARY KEY AUTOINCREMENT,
                            name TEXT NOT NULL,
                            categoryId INTEGER NOT NULL,
                            price FLOAT NOT NULL,
                            stock INTEGER NOT NULL,
                            createdAt STRING NOT NULL,
                            updatedAt STRING NOT NULL,
                            FOREIGN KEY (categoryId) REFERENCES categories(id)
                    );";

                    using (var command = new SQLiteCommand(connection))
                    {
                        command.CommandText = createProductsTableQuery;
                        command.ExecuteNonQuery();

                        command.CommandText = createCategoriesTableQuery;
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        public static void AddCategories()
        {
            var categories = new[]
            {
                new Category{ name = "Tobak", color = "8b4513"},
                new Category{ name = "Godis", color = "ffb6c1"},
                new Category{ name = "Enkel mat", color = "ffa500"},
                new Category{ name = "Tidningar", color = "add8e6"},
            };

                    foreach (var category in categories){AddCategory(category);}
        }

        public static object AddCategory(Category category)
        {
            object result = new {};

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (var cmd = new SQLiteCommand(@"
                            INSERT INTO categories(CategoryName, CategoryColor, CreatedAt, UpdatedAt)
                            VALUES (@categoryName, @categoryColor, @createdAt, @updatedAt)
                            RETURNING id", connection))
                {
                    cmd.Parameters.Add(new SQLiteParameter("@categoryName"));
                    cmd.Parameters.Add(new SQLiteParameter("@categoryColor"));
                    cmd.Parameters.Add(new SQLiteParameter("@createdAt"));
                    cmd.Parameters.Add(new SQLiteParameter("@updatedAt"));

                    string timestamp = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ");

                    cmd.Parameters["@categoryName"].Value = category.name;
                    cmd.Parameters["@categoryColor"].Value = category.color;
                    cmd.Parameters["@createdAt"].Value = timestamp;
                    cmd.Parameters["@updatedAt"].Value = timestamp;

                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            result = new{
                                id = reader.GetInt32(reader.GetOrdinal("id")),
                                name = category.name, 
                                color = category.color,
                                createdAt = timestamp,
                                updatedAt = timestamp,
                            };
                        }
                    }
                }

                return Results.Created("", result);
            }
        }

        public static void AddProducts()
        {
            var products = new[]
            {
                new Product{ name = "Marlboro Red (20-pack)", category = 1, price = 89.00F, stock = 100 },
                new Product{ name = "Camel Blue (20-pack)", category = 1, price = 85.00F, stock = 100 },
                new Product{ name = "L&M Filter (20-pack)", category = 1, price = 79.00F, stock = 100 },
                new Product{ name = "Skruf Original Portion", category = 1, price = 62.00F, stock = 100 },
                new Product{ name = "Göteborgs Rapé White Portion", category = 1, price = 67.00F, stock = 100 },

                new Product{ name = "Marabou Mjölkchoklad 100 g", category = 2, price = 25.00F, stock = 100 },
                new Product{ name = "Daim dubbel", category = 2, price = 15.00F, stock = 100 },
                new Product{ name = "Kexchoklad", category = 2, price = 12.00F, stock = 100 },
                new Product{ name = "Malaco Gott & Blandat 160 g", category = 2, price = 28.00F, stock = 100 },

                new Product{ name = "Korv med bröd", category = 3, price = 25.00F, stock = 100 },
                new Product{ name = "Varm toast (ost & skinka)", category = 3, price = 30.00F, stock = 100 },
                new Product{ name = "Pirog (köttfärs)", category = 3, price = 22.00F, stock = 100 },
                new Product{ name = "Färdig sallad (kyckling)", category = 3, price = 49.00F, stock = 100 },
                new Product{ name = "Panini (mozzarella & pesto)", category = 3, price = 45.00F, stock = 100 },

                new Product{ name = "Aftonbladet (dagens)", category = 4, price = 28.00F, stock = 100 },
                new Product{ name = "Expressen (dagens)", category = 4, price = 28.00F, stock = 100 },
                new Product{ name = "Illustrerad Vetenskap", category = 4, price = 79.00F, stock = 100 },
                new Product{ name = "Kalle Anka & Co", category = 4, price = 45.00F, stock = 100 },
                new Product{ name = "Allt om Mat", category = 4, price = 69.00F, stock = 100 },

            };

            foreach (var product in products){AddProduct(product);}
        }

        public static object AddProduct(Product product)
        {
            object result = new {};

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (var cmd = new SQLiteCommand(@"
                            INSERT INTO products(Name, CategoryID, Price, Stock, CreatedAt, UpdatedAt)
                            VALUES (@name, @categoryId, @price, @stock, @createdAt, @updatedAt)
                            RETURNING id", connection))
                {
                    cmd.Parameters.Add(new SQLiteParameter("@name"));
                    cmd.Parameters.Add(new SQLiteParameter("@categoryId"));
                    cmd.Parameters.Add(new SQLiteParameter("@price"));
                    cmd.Parameters.Add(new SQLiteParameter("@stock"));
                    cmd.Parameters.Add(new SQLiteParameter("@createdAt"));
                    cmd.Parameters.Add(new SQLiteParameter("@updatedAt"));

                    string timestamp = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ");

                    cmd.Parameters["@name"].Value = product.name;
                    cmd.Parameters["@categoryId"].Value = product.category;
                    cmd.Parameters["@price"].Value = product.price;
                    cmd.Parameters["@stock"].Value = product.stock;
                    cmd.Parameters["@createdAt"].Value = timestamp;
                    cmd.Parameters["@updatedAt"].Value = timestamp;

                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            result = new{
                                id = reader.GetInt32(reader.GetOrdinal("id")),
                                name = product.name, 
                                category = product.category, 
                                price = product.price,
                                stock = product.stock,
                                createdAt = timestamp,
                                updatedAt = timestamp,
                            };
                        }
                    }
                }

                return Results.Created("", result);
            }
        }

        public static object ReadProduct(int id)
        {
            object result = new {};

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (var cmd = new SQLiteCommand($"SELECT * FROM products WHERE id = {id}", connection))
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        result = new{
                            id = id,
                            name = reader.GetString(reader.GetOrdinal("name")),
                            category = reader.GetInt32(reader.GetOrdinal("categoryId")),
                            price = reader.GetFloat(reader.GetOrdinal("price")),
                            stock = reader.GetInt32(reader.GetOrdinal("stock"))
                        };
                    }
                    else return Results.StatusCode(204);
                }
            }
            return result;
        }

        public static List<object> ReadAllProducts()
        {
            List<object> result = new List<object>();

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (var cmd = new SQLiteCommand($"SELECT * FROM products", connection))
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        object product = new{
                            id = reader.GetInt32(reader.GetOrdinal("id")),
                            name = reader.GetString(reader.GetOrdinal("name")),
                            category = reader.GetInt32(reader.GetOrdinal("categoryId")),
                            price = reader.GetFloat(reader.GetOrdinal("price")),
                            stock = reader.GetInt32(reader.GetOrdinal("stock"))
                        };

                        result.Add(product);
                    };
                }
            }

            return result;
        }

        public static object UpdateProduct(Product product, int id)
        {
            object result = new {};
            string timestamp = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ");

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (var cmd = new SQLiteCommand($@"
                            UPDATE products 
                            SET name = '{product.name}', 
                                categoryId = '{product.category}', 
                                price = '{product.price}',
                                stock = '{product.stock}',
                                updatedAt = '{timestamp}'
                            WHERE id = {id}
                            RETURNING *", connection))
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            result = new{
                                id = id,
                                name = reader.GetString(reader.GetOrdinal("name")),
                                category = reader.GetInt32(reader.GetOrdinal("categoryId")),
                                price = reader.GetFloat(reader.GetOrdinal("price")),
                                stock = reader.GetInt32(reader.GetOrdinal("stock"))
                            };
                        }
                        else return Results.StatusCode(204);
                    }
            }

            return result;
        }

        public static IResult DeleteProduct(int id)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (var cmd = new SQLiteCommand($"DELETE FROM products WHERE id = {id}", connection))
                    cmd.ExecuteNonQuery();
            }

            return Results.StatusCode(204);
        }

        public static object EditStock(int id, int amount)
        {
            object result = new {};
            string timestamp = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ");

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (var cmd = new SQLiteCommand($@"
                            UPDATE products 
                            SET stock = stock + '{amount}' ,
                            updatedAt = '{timestamp}'
                            WHERE id = {id}
                            RETURNING stock", connection))
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            result = new{
                                id = id,
                                stock = reader.GetInt32(reader.GetOrdinal("stock"))
                            };
                        }
                        else return Results.StatusCode(204);
                    }
            }

            return result;
        }

        public static object ReadCategory(int id)
        {
            object result = new {};

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (var cmd = new SQLiteCommand($"SELECT * FROM categories WHERE id = {id}", connection))
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        result = new{
                            id = id,
                            name = reader.GetString(reader.GetOrdinal("categoryName")),
                            color = reader.GetString(reader.GetOrdinal("categoryColor")),
                        };
                    }
                    else return Results.StatusCode(204);
                }
            }
            return result;
        }

        public static List<object> ReadAllCategories()
        {
            List<object> result = new List<object>();

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (var cmd = new SQLiteCommand($"SELECT * FROM categories", connection))
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        object category = new{
                            id = reader.GetInt32(reader.GetOrdinal("id")),
                            name = reader.GetString(reader.GetOrdinal("categoryName")),
                            color = reader.GetString(reader.GetOrdinal("categoryColor")),
                        };

                        result.Add(category);
                    };
                }
            }

            return result;
        }
//
        public static object UpdateCategory(Category category, int id)
        {
            object result = new {};
            string timestamp = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ");

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (var cmd = new SQLiteCommand($@"
                            UPDATE categories 
                            SET categoryName = '{category.name}', 
                                categoryColor = '{category.color}',
                                updatedAt = '{timestamp}'
                            WHERE id = {id}
                            RETURNING *", connection))
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            result = new{
                                id = id,
                                name = reader.GetString(reader.GetOrdinal("categoryName")),
                                color = reader.GetString(reader.GetOrdinal("categoryColor")),
                            };
                        }
                        else return Results.StatusCode(204);
                    }
            }

            return result;
        }

        public static IResult DeleteCategory(int id)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (var cmd = new SQLiteCommand($"DELETE FROM categories WHERE id = {id}", connection))
                    cmd.ExecuteNonQuery();
            }

            return Results.StatusCode(204);
        }
    }
}
