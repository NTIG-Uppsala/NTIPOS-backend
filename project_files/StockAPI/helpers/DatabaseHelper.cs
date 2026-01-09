using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Helpers
{
    class Product
    {
        public int id;
        public string? name;
        public int categoryID;
        public float price;
        public int stock;
        public string? createdAt;
        public string? updatedAt;
    }

    class HalfProduct
    {
        public int id;
        public string? name;
        public int category;
        public float price;
        public int stock;
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
                file.Directory.Create();
                SQLiteConnection.CreateFile(fileLocation);

                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    string createCategoriesTableQuery = @"
                    CREATE TABLE IF NOT EXISTS categories(
                            id INTEGER PRIMARY KEY AUTOINCREMENT,
                            categoryName TEXT NOT NULL,
                            categoryColor TEXT NOT NULL
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
                new { Name = "Tobak", Color = "SaddleBrown"},
                new { Name = "Godis", Color = "LightPink"},
                new { Name = "Enkel mat", Color = "Orange"},
                new { Name = "Tidningar", Color = "LightBlue"},
            };

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (var tx = connection.BeginTransaction())
                using (var cmd = new SQLiteCommand(@"
                            INSERT INTO categories(CategoryName, CategoryColor)
                            VALUES (@categoryName, @categoryColor)", connection, tx))
                {
                    cmd.Parameters.Add(new SQLiteParameter("@categoryName"));
                    cmd.Parameters.Add(new SQLiteParameter("@categoryColor"));

                    foreach (var category in categories)
                    {
                        cmd.Parameters["@categoryName"].Value = category.Name;
                        cmd.Parameters["@categoryColor"].Value = category.Color;
                        cmd.ExecuteNonQuery();
                    }
                    tx.Commit();
                }
            }
        }

        public static void AddProducts()
        {
            var products = new[]
            {
                new Product{ name = "Marlboro Red (20-pack)", categoryID = 1, price = 89.00F, stock = 100 },
                new Product{ name = "Camel Blue (20-pack)", categoryID = 1, price = 85.00F, stock = 100 },
                new Product{ name = "L&M Filter (20-pack)", categoryID = 1, price = 79.00F, stock = 100 },
                new Product{ name = "Skruf Original Portion", categoryID = 1, price = 62.00F, stock = 100 },
                new Product{ name = "Göteborgs Rapé White Portion", categoryID = 1, price = 67.00F, stock = 100 },

                new Product{ name = "Marabou Mjölkchoklad 100 g", categoryID = 2, price = 25.00F, stock = 100 },
                new Product{ name = "Daim dubbel", categoryID = 2, price = 15.00F, stock = 100 },
                new Product{ name = "Kexchoklad", categoryID = 2, price = 12.00F, stock = 100 },
                new Product{ name = "Malaco Gott & Blandat 160 g", categoryID = 2, price = 28.00F, stock = 100 },

                new Product{ name = "Korv med bröd", categoryID = 3, price = 25.00F, stock = 100 },
                new Product{ name = "Varm toast (ost & skinka)", categoryID = 3, price = 30.00F, stock = 100 },
                new Product{ name = "Pirog (köttfärs)", categoryID = 3, price = 22.00F, stock = 100 },
                new Product{ name = "Färdig sallad (kyckling)", categoryID = 3, price = 49.00F, stock = 100 },
                new Product{ name = "Panini (mozzarella & pesto)", categoryID = 3, price = 45.00F, stock = 100 },

                new Product{ name = "Aftonbladet (dagens)", categoryID = 4, price = 28.00F, stock = 100 },
                new Product{ name = "Expressen (dagens)", categoryID = 4, price = 28.00F, stock = 100 },
                new Product{ name = "Illustrerad Vetenskap", categoryID = 4, price = 79.00F, stock = 100 },
                new Product{ name = "Kalle Anka & Co", categoryID = 4, price = 45.00F, stock = 100 },
                new Product{ name = "Allt om Mat", categoryID = 4, price = 69.00F, stock = 100 },

            };

            foreach (var product in products){AddProduct(product);}
        }
        public static string AddProduct(Product product)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (var cmd = new SQLiteCommand(@"
                            INSERT INTO products(Name, CategoryID, Price, Stock, CreatedAt, UpdatedAt)
                            VALUES (@name, @categoryId, @price, @stock, @createdAt, @updatedAt)
                            RETURNING id, createdAt, updatedAt", connection))
                {
                    cmd.Parameters.Add(new SQLiteParameter("@name"));
                    cmd.Parameters.Add(new SQLiteParameter("@categoryId"));
                    cmd.Parameters.Add(new SQLiteParameter("@price"));
                    cmd.Parameters.Add(new SQLiteParameter("@stock"));
                    cmd.Parameters.Add(new SQLiteParameter("@createdAt"));
                    cmd.Parameters.Add(new SQLiteParameter("@updatedAt"));

                    string timestamp = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ");

                    cmd.Parameters["@name"].Value = product.name;
                    cmd.Parameters["@categoryId"].Value = product.categoryID;
                    cmd.Parameters["@price"].Value = product.price;
                    cmd.Parameters["@stock"].Value = product.stock;
                    cmd.Parameters["@createdAt"].Value = timestamp;
                    cmd.Parameters["@updatedAt"].Value = timestamp;

                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        reader.Read();

                        product.id = reader.GetInt32(reader.GetOrdinal("id"));
                        product.createdAt = reader.GetString(reader.GetOrdinal("createdAt"));
                        product.updatedAt = reader.GetString(reader.GetOrdinal("updatedAt"));
                    }
                }

                string result = "fix";
                return result;
            }
        }

        public static object ReadProduct(int id)
        {
            object result;

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (var cmd = new SQLiteCommand($"SELECT * FROM products WHERE id = {id}", connection))
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    reader.Read();

                    result = new{
                        id = id,
                        name = reader.GetString(reader.GetOrdinal("name")),
                        category = reader.GetInt32(reader.GetOrdinal("categoryId")),
                        price = reader.GetFloat(reader.GetOrdinal("price")),
                        stock = reader.GetInt32(reader.GetOrdinal("stock"))
                    };
                }
            }
            return result;
        }

        public static string ReadData(string query)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (var cmd = new SQLiteCommand(query, connection))
                {
                    object result = cmd.ExecuteScalar();
                    return result == null ? "" : result.ToString();
                }
            }
        }
    }
}
