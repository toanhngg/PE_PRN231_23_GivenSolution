using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Q1.DTO;
using Q1.Models;

namespace Q1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        PE_PRN_Fall2023B1Context context = new PE_PRN_Fall2023B1Context();
        [HttpPost("AddSchedule")]
        public IActionResult AddSchedule([FromBody] ScheduleDTO sc )
        {
            try
            {
                var movie = context.Movies.Where(x => x.Id == sc.MovieId).FirstOrDefault();
                var room = context.Rooms.Where(x => x.Id == sc.RoomId).FirstOrDefault();
                var timeslot = context.TimeSlots.Where(x => x.Id != sc.TimeSlotId).FirstOrDefault();
                if (movie != null && room != null && timeslot != null)
                {
                    if (sc.StartDate < sc.EndDate)
                    {
                        Schedule schedule = new Schedule()
                        {
                            MovieId = sc.MovieId,
                            RoomId = sc.RoomId,
                            TimeSlotId = sc.TimeSlotId,
                            StartDate = sc.StartDate,
                            EndDate = sc.EndDate,
                            Note = sc.Note,
                        };
                        context.Schedules.Add(schedule);
                        context.SaveChanges();

                        return Ok();
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
                else
                {
                    return NotFound();
                }
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
