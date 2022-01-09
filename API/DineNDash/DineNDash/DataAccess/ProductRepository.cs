using Dapper;
using DineNDash.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DineNDash.DataAccess
{
    public class ProductRepository
    {
        readonly string _connectionString = "Server=localhost;Database=DineAndDash;Trusted_Connection=True;";
        internal object GetAll()
        {
            using var db = new SqlConnection(_connectionString);

            var products = db.Query<Product>(@"Select * From Products");

            return products;
        }

        // Gets a single product by their Id //


        internal Product GetById(Guid id)
        {
            // establishsd connections to our database //
            using var db = new SqlConnection(_connectionString);

            // Selecting all info of the product that has the matching Id given in the params and setting that response to a variable //
            var sql = @"Select *
                        From Products
                        Where Id = @id";

            // slecting the first product from that response, and setting the two parameters needed for this Query. Setting this to a variable //
            var singleProduct = db.QueryFirstOrDefault<Product>(sql, new { id });

            // returning that variable. AKA our product //
            return singleProduct;
        }
        internal IEnumerable<Product> GetByType(ProductType type)
        {
            using var db = new SqlConnection(_connectionString);

            var sql = @" Select *
                         From Products
                         Where Type = @type";
            var products = db.Query<Product>(sql, new { type });
            return products;

        }

        internal IEnumerable<Product> GetForSale(string sale)
        {
            using var db = new SqlConnection(_connectionString);

            var sql = @"Select *
                        From Products
                        Where forSale = @sale";

            var productsForSale = db.Query<Product>(sql, new { sale });

            return productsForSale;

        }

        internal IEnumerable<Product> GetForRent(string rent)
        {
            using var db = new SqlConnection(_connectionString);

            var sql = @"Select *
                        From Products
                        Where forRent = @rent";

            var productsForRent = db.Query<Product>(sql, new { rent });

            return productsForRent;
        }

        internal IEnumerable<Product> GetAvailable(string status)
        {
            using var db = new SqlConnection(_connectionString);

            var sql = @"Select *
                        From Products 
                        Where status = @status";

            var availableProducts = db.Query<Product>(sql, new { status });
            return availableProducts;
        }

        internal void Add(Product product)
        {
            using var db = new SqlConnection(_connectionString);

            var sql = @"INSERT INTO [dbo].[Products]
                                       (
                                        [type]
                                       ,[productName]
                                       ,[productDescription]
                                       ,[price]
                                       ,[quantity]
                                       ,[forSale]
                                       ,[image]
                                       ,[forRent])
	                            output inserted.Id
                                 VALUES
		                            (@type, @productName, @productDescription, @price, @quantity, @forSale, @image, @forRent)";

            var id = db.ExecuteScalar<Guid>(sql, product);
            product.Id = id;
        }

        internal object Update(Guid id, Product product)
        {
            using var db = new SqlConnection(_connectionString);

            var sql = @"UPDATE [dbo].[Products]
                           SET Type = @type,
                               ProductName = @productName,
                               ProductDescription = @productDescription,
                               Price = @price,
                               Quantity = @quantity,
                               ForSale = @forSale,
                               ForRent = @forRent,
                               Image = @image,
                               Status = @status
                           Output inserted.*
                           WHERE Id = @id";
            product.Id = id;
            var updatedProduct = db.QuerySingleOrDefault<Product>(sql, product);

            return updatedProduct;
        }
        internal void Remove(Guid id)
        {
            using var db = new SqlConnection(_connectionString);

            var sql = @"Delete 
                        From [dbo].[Products]
                        Where Id = @id";

            db.Execute(sql, new { id });
        }

    }
}
