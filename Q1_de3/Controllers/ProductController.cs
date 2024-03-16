using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;
using Q1_de3.Models;
using System.Linq;

namespace Q1_de3.Controllers
{
  
    public class ProductController : ControllerBase
    {
        PE_PRN_23SumContext context = new PE_PRN_23SumContext();
        [HttpDelete("delete/{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                List<OrderDetail> order = context.OrderDetails.Where(x => x.ProductId == id).ToList();
                context.OrderDetails.RemoveRange(order);
                context.SaveChanges();

                var product = context.Products.Where(x => x.ProductId == id).FirstOrDefault();
                if (product != null)
                {
                    context.Products.Remove(product);
                    context.SaveChanges();
                    return Ok();
                }
                else
                {
                    return BadRequest("Ko tìm thấy product");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [EnableQuery]
        [HttpGet("api/Product/index")]
        [Produces("application/json", new string[] { "text/plain", "text/json" })]
        public IActionResult Index()
        {
            try
            {
                var products = context.Products
                                 .Include(x => x.Category)
                                 .Where(product => product.OrderDetails.Any(g => g.ProductId == product.ProductId))
                                 .Select(product => new
                                 {
                                     productId = product.ProductId,
                                     productName = product.ProductName,
                                     categoryId = product.CategoryId,
                                     categoryName = product.Category.CategoryName,
                                     quantityPerUnit = product.QuantityPerUnit,
                                     unitPrice = product.UnitPrice,
                                     unitsInStock = product.UnitsInStock,
                                     totalUnitOrdered = product.OrderDetails.Sum(x => x.Quantity) // Tính tổng số lượng
                                 });
                                //join c in context.Categories on p.CategoryId equals c.CategoryId
                                //select new
                                //{
                                //    productId = p.ProductId,
                                //    productName = p.ProductName,
                                //    categoryId = p.CategoryId,
                                //    categoryName = c.CategoryName,
                                //    quantityPerUnit = p.QuantityPerUnit,
                                //    unitPrice = p.UnitPrice,
                                //    unitsInStock = p.UnitsInStock,
                                //    totalUnitOrdered = p.UnitsOnOrder * p.UnitPrice,
                                //}).ToList();

                return Ok(products);
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("api/Product/List/{minprice}/{maxprice}")]
        public IActionResult List(int minprice , int maxprice)
        {
            try
            {
                var products = context.Products
                                 .Include(x => x.Category)
                                 .Where(product => product.OrderDetails.Any(g => g.ProductId == product.ProductId))
                                 .Where(product => product.UnitPrice >= minprice && product.UnitPrice <= maxprice)
                                 .Select(product => new
                                 {
                                     productId = product.ProductId,
                                     productName = product.ProductName,
                                     categoryId = product.CategoryId,
                                     categoryName = product.Category.CategoryName,
                                     quantityPerUnit = product.QuantityPerUnit,
                                     unitPrice = product.UnitPrice,
                                     unitsInStock = product.UnitsInStock,
                                     totalUnitOrdered = product.OrderDetails.Sum(x => x.Quantity) // Tính tổng số lượng
                                 });

                return Ok(products);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



    }
}
