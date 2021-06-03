using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dicount.API.Entities
{
   
    public record Coupon(int Id, string ProductName, string Description, int Amount);

    //public class Coupon
    //{
    //    public int Id { get; set; }

    //    public string ProductName { get; set; }
    //    public string Description { get; set; }
    //    public int Amount { get; set; }
    //}
}
