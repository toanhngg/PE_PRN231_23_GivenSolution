using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Q1_de4.Models;

namespace Q1_de4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        PRN_Sum22_B1Context context = new PRN_Sum22_B1Context();

        [HttpGet("GetAllOrder")]
        public IActionResult GetAllOrder()
        {
            try
            {
                var listOrder = context.Orders
                    .Include(x => x.OrderDetails)
                    .Include(x => x.Employee)
                    .Include(x => x.Customer)
                    .Select(x => new
                    {
                        orderId = x.OrderId,
                        customerId = x.CustomerId,
                        customerName = x.Customer.CompanyName,
                        employeeId = x.Employee.EmployeeId,
                        employeeName = x.Employee.FirstName + x.Employee.LastName,
                        employeeDepartmentId = x.Employee.Department.DepartmentId,
                        employeeDepartmentName = x.Employee.Department.DepartmentName,
                        orderDate = x.OrderDate,
                        requiredDate = x.RequiredDate,
                        freight = x.Freight,
                        shipName = x.ShipName,
                        shipAddress = x.ShipAddress,
                        shipRegion = x.ShipRegion,
                        shipPostalCode = x.ShipPostalCode,
                        shipCountry = x.ShipCountry,

                    }).ToList();
                return Ok(listOrder);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetOrderByDate/{from}/{to}")]
        public IActionResult GetOrderByDateFromTo(DateTime from, DateTime to)
        {
            try
            {
                if (from != null && to != null)
                {
                    if (from <= to)
                    {
                        var listOrder = context.Orders
                                           .Include(x => x.OrderDetails)
                                           .Include(x => x.Employee)
                                           .Include(x => x.Customer)
                                           .Where(x => x.OrderDate > from && x.OrderDate < to)
                                           .Select(x => new
                                           {
                                               orderId = x.OrderId,
                                               customerId = x.CustomerId,
                                               customerName = x.Customer.CompanyName,
                                               employeeId = x.Employee.EmployeeId,
                                               employeeName = x.Employee.FirstName + x.Employee.LastName,
                                               employeeDepartmentId = x.Employee.Department.DepartmentId,
                                               employeeDepartmentName = x.Employee.Department.DepartmentName,
                                               orderDate = x.OrderDate,
                                               requiredDate = x.RequiredDate,
                                               freight = x.Freight,
                                               shipName = x.ShipName,
                                               shipAddress = x.ShipAddress,
                                               shipRegion = x.ShipRegion,
                                               shipPostalCode = x.ShipPostalCode,
                                               shipCountry = x.ShipCountry
                                               }).ToList();
                        return Ok(listOrder);
                    }
                    return BadRequest("Vui lòng nhập ngày phù hợp");

                }
                else
                {
                    return BadRequest("Vui lòng nhập ngày");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
