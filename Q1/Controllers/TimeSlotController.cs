using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Q1.Models;

namespace Q1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimeslotController : ControllerBase
    {
        PE_PRN_Fall2023B1Context   context = new PE_PRN_Fall2023B1Context();
        [HttpGet("List")]
        public IActionResult List()
        {
            try
            {
                var timeslot = context.TimeSlots.ToList();
                return Ok(timeslot);
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
