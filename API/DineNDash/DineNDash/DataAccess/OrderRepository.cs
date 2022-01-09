using Dapper;
using DineNDash.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DineNDash.DataAccess
{
    public class OrderRepository
    {
        readonly string _connectionString;

        public OrderRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DineAndDash");
        }
        // GET ALL ORDERS
        internal IEnumerable<Order> GetAll()
        {
            using var db = new SqlConnection(_connectionString);

            var sql = @"select *
                        from Orders o
	                        join Users u 
		                        on u.Id = o.UserId
                            join Payments p
                                on p.Id = o.PaymentId
	                       ";

            var results = db.Query<Order, User, Payment, Order>(sql, Map, splitOn: "Id");

            return results;
        }

        // Getting a single order by the Id passed into the params //
        internal Order GetById(Guid id)
        {
            // Sets up connection with out Database //
            using var db = new SqlConnection(_connectionString);

            // First we are joining the users and orders table. We are doing this by joining Users on the User id and setting that equal to the Order's User id //
            // Then Joinging the payments table. We are doing this by setting the id of the payment table to the order's payment Id of the orders table //
            // Then we are saying only select the order info where the order.id is equal to the id passed in the params //
            // Setting this response to the variable 'orderSql' //
            var orderSql = @"select *
                        from Orders o
	                        join Users u
		                        on u.Id = o.UserId
                            join Payments p
                                on p.Id = o.PaymentId
                            WHERE o.id = @id
	                       ";

            // Setting the response of this query to the 'order' variable //
            // Using this multi-mapping query with 3 input types to return a single input type of an enumerable of Order and splitting on the scaler variable of id //
            var order = db.Query<Order, User, Payment, Order>(orderSql, Map, new { id = id });

            //if (order == null) return null;

            // returning the first order in our query above //
            return order.FirstOrDefault();
        }

        // Creating a map as a param for the multi-mapping query above //
        Order Map(Order order, User user, Payment payment)
        {
            // setting the user of the order to user //
            order.User = user;
            // settin the payment of the order to payment //
            order.Payment = payment;
            // returning our order param //
            return order;
        }

        internal IEnumerable<Order> GetCompletedUserOrders(Guid userId, bool completed)
        {
            using var db = new SqlConnection(_connectionString);

            var sql = @"Select *
                From Orders
                WHERE userId = @id
                AND completed = @completedOrder";

            var orders = db.Query<Order>(sql, new { id = userId, completedOrder = completed });

            return orders;
        }
        internal OrderDetail GetUserCart(Guid userId)
        {
            using var db = new SqlConnection(_connectionString);

            var sql = @"Select *
                From Orders
                WHERE userId = @id
                AND completed = 0";

            var orders = db.QueryFirstOrDefault<OrderDetail>(sql, new { id = userId });

            return orders;
        }

        internal IEnumerable<Order> GetUserOrders(Guid userId)
        {
            using var db = new SqlConnection(_connectionString);

            var sql = @"Select *
                From Orders
                WHERE userId = @id";

            var orders = db.Query<Order>(sql, new { id = userId });

            return orders;
        }
        // ADD AN ORDER
        internal void Add(Order newOrder)
        {
            using var db = new SqlConnection(_connectionString);

            var sql = @"INSERT INTO [dbo].[Orders]
                                       (
                                         [UserId]
                                        ,[TotalCost]
                                        ,[Completed])
	                            output inserted.Id
                                 VALUES
		                            (@UserId, @TotalCost, @Completed)";

            var parameters = new
            {
                TotalCost = newOrder.TotalCost,
                Completed = newOrder.Completed,
                UserId = newOrder.User.Id,
            };

            var id = db.ExecuteScalar<Guid>(sql, parameters);
            newOrder.Id = id;
        }

        internal Order Update(Guid id, Order order)
        {
            using var db = new SqlConnection(_connectionString);

            var sql = @"update Orders set
                                        UserId = @userId, 
                                        TotalCost = @totalCost,
                                        PaymentId = @paymentId,
                                        Completed = @completed
                                       
                                     
                                     output inserted.Id
                                     Where Id = @id";

            order.Id = id;

            var parameters = new
            {
                Id = order.Id,
                UserId = order.User.Id,
                TotalCost = order.TotalCost,
                PaymentId = order.Payment.Id,
                Completed = order.Completed
            };

            var updatedOrder = db.QueryFirstOrDefault<Order>(sql, parameters);
            return updatedOrder;

        }

        internal object UpdateCompleted(Guid id, Order order)
        {
            using var db = new SqlConnection(_connectionString);

            var sql = @"update Orders set
                                     Completed = @completed
                                        output inserted.Id
                                     Where Id = @id";

            order.Id = id;

            var parameters = new
            {
                Completed = order.Completed
            };

            var updatedOrder = db.QueryFirstOrDefault<Order>(sql, parameters);
            return updatedOrder;
        }

        internal void Remove(Guid id)
        {
            using var db = new SqlConnection(_connectionString);

            var sql = @"Delete 
                        From Orders 
                        Where Id = @id";

            db.Execute(sql, new { id });
        }







    }
}