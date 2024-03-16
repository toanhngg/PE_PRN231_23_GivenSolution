using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Q1_de4.DTO;
using Q1_de4.Models;

namespace Q1_de4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        PRN_Sum22_B1Context context = new PRN_Sum22_B1Context();

        [HttpPost("Delete/{CustomerId}")]
        public IActionResult DeleteCus(string CustomerId)
        {
            try
            {
                bool isNumeric = int.TryParse(CustomerId, out _);

                //var customer = context.Customers.Where(x => x.CustomerId == CustomerId).FirstOrDefault();   
                //if(customer != null)
                //{
                //int orderDetailDeleteCount = 0;
                //int orderDeleteCount = 0;
                //int customerDeleteCount = 0;
                //List<OrderDetail> orderDetails = context.Orders
                //                                .Where(order => order.CustomerId == CustomerId)
                //                                .SelectMany(order => order.OrderDetails)
                //                                .ToList();
                //List<Order> order = context.Orders.Where(x => x.CustomerId == CustomerId).ToList();
                //orderDetailDeleteCount = orderDetails.Count();
                //orderDeleteCount = order.Count();
                //customerDeleteCount = 1;
                //ResponseDTO responseDTO = new ResponseDTO()
                //{
                //    customerDeleteCount = customerDeleteCount,
                //    orderDeleteCount = orderDetailDeleteCount,
                //    orderDetailDeleteCount = (int)orderDetailDeleteCount
                //};
                if (!isNumeric)
                {
                    var customerCounts = context.Customers
                          .Where(x => x.CustomerId == CustomerId)
                          .Select(customer => new
                          {
                              customerDeleteCount = 1, // Vì bạn chỉ đang kiểm tra sự tồn tại của khách hàng này, nên số lượng là 1 hoặc 0.
                              orderDeleteCount = context.Orders.Count(order => order.CustomerId == CustomerId),
                              orderDetailDeleteCount = context.Orders
                                  .Where(order => order.CustomerId == CustomerId)
                                  .SelectMany(order => order.OrderDetails)
                                  .Count()
                          })
                          .FirstOrDefault();
                    var customer = context.Customers
                        .Include(c => c.Orders)
                         .ThenInclude(o => o.OrderDetails)
                        .FirstOrDefault(x => x.CustomerId == CustomerId);

                    if (customer != null)
                    {
                        // Xóa chi tiết đơn hàng của khách hàng
                        context.OrderDetails.RemoveRange(customer.Orders.SelectMany(o => o.OrderDetails));

                        // Xóa các đơn hàng của khách hàng
                        context.Orders.RemoveRange(customer.Orders);

                        // Xóa khách hàng
                        context.Customers.Remove(customer);

                        // Lưu các thay đổi vào cơ sở dữ liệu
                        context.SaveChanges();
                        return Ok(customerCounts);


                    }
                    else
                    {
                        return NotFound();
                    }

                    //context.SaveChanges();
                    // string jsonResponse = JsonConvert.SerializeObject( responseDTO );
                }
                else
                {
                    return StatusCode(409, "There was an unknown error when performing data deletion");

                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
