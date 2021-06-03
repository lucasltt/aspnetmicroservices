using Dicount.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;
using Microsoft.Extensions.Configuration;
using Dapper;

namespace Dicount.API.Repositories
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly IConfiguration _configuration;

        public DiscountRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> CreateDiscountAsync(Coupon coupon)
        {
            using NpgsqlConnection connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            int rowsAffected = await connection.ExecuteAsync("INSERT INTO Coupon(ProductName, Description, Amount) VALUES(@ProductName, @Description, @Amount)", 
                                                            new { ProductName = coupon.ProductName, Description = coupon.Description, Amount = coupon.Amount });

            if (rowsAffected == 0)
                return false;

            return true;
        }

        public async Task<bool> DeleteDiscountAsync(string productName)
        {
            using NpgsqlConnection connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            int rowsAffected = await connection.ExecuteAsync("DELETE FROM Coupon WHERE ProductName = @ProductName",
                                                            new { ProductName = productName });

            if (rowsAffected == 0)
                return false;

            return true;
        }

        public async Task<Coupon> GetDiscountAsync(string productName)
        {
            using NpgsqlConnection connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            Coupon coupon = await connection.QueryFirstOrDefaultAsync<Coupon>("SELECT * FROM Coupon WHERE ProductName = @ProductName", new { ProductName = productName });

            if (coupon == null)
                return new Coupon (0, "No Discount", "No Discount", 0);

            return coupon;
        }

        public async Task<bool> UpdateDiscountAsync(Coupon coupon)
        {
            using NpgsqlConnection connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            int rowsAffected = await connection.ExecuteAsync("UPDATE Coupon SET ProductName = @ProductName, Description = @Description, Amount = @Amount where Id = @Id)",
                                                            new { ProductName = coupon.ProductName, Description = coupon.Description, Amount = coupon.Amount, Id = coupon.Id });

            if (rowsAffected == 0)
                return false;

            return true;
        }
    }
}
