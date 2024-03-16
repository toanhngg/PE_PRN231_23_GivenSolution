using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Q1.Models;

namespace Q1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        PE_PRN_Fall2023B1Context context = new PE_PRN_Fall2023B1Context();

        [HttpGet("GetMovies")]
        [EnableQuery]
        public IActionResult GetMovies()
        {
            //var movies = (from m in context.Movies
            //             join d in context.Directors on m.DirectorId equals d.Id
            //             join s in context.Schedules on m.Id equals s.MovieId
            //             select new
            //             {
            //                 id = m.Id,
            //                 title = m.Title,
            //                 year = m.Year,
            //                 description = m.Description,
            //                 directorName = d.Name,
            //                 directorId = d.Id,

            //                 movieStars = (from ms in context.MovieStars
            //                               join s1 in context.Stars on ms.StarId equals s1.Id
            //                               where ms.MovieId == m.Id // Lọc theo Id của bộ phim
            //                               select s1.Name).ToList()
            //             }).Distinct().ToList();
            var movies = (from m in context.Movies
                          join d in context.Directors on m.DirectorId equals d.Id
                          join s in context.Schedules on m.Id equals s.MovieId
                          select new
                          {
                              id = m.Id,
                              title = m.Title,
                              year = m.Year,
                              description = m.Description,
                              directorName = d.Name,
                              directorId = d.Id,

                              movieStars = (from ms in context.MovieStars
                                            join s1 in context.Stars on ms.StarId equals s1.Id
                                            where ms.MovieId == m.Id // Lọc theo Id của bộ phim
                                            select s1.Name).Distinct().ToList()
                          }).GroupBy(m => m.id).Select(g => g.First()).ToList();

            return Ok(movies);
        }

        [HttpGet("GetMovieByStar")]
        public IActionResult GetMovieByStar([FromQuery] string star)
        {
            var movies = (from m in context.Movies
                          join d in context.Directors on m.DirectorId equals d.Id
                          join s in context.Schedules on m.Id equals s.MovieId
                          join ms in context.MovieStars on m.Id equals ms.MovieId
                          join s1 in context.Stars on ms.StarId equals s1.Id
                          group s1.Name by new
                          {
                              m.Id,
                              m.Title,
                              m.Year,
                              m.Description,
                              DirectorName = d.Name,
                          } into g
                          select new
                          {
                              id = g.Key.Id,
                              title = g.Key.Title,
                              year = g.Key.Year,
                              description = g.Key.Description,
                              directorName = g.Key.DirectorName,
                              directorId = g.Key.Id,
                              movieStars = g.Distinct().ToList()
                          }).ToList();

            return Ok(movies);
       


        }

        [HttpGet("List")]
        [EnableQuery]

        public IActionResult List()
        {
            try
            {
                var movies = (from sc in context.Schedules
                            join m in context.Movies on sc.MovieId equals m.Id 
                            join r in context.Rooms on sc.RoomId equals r.Id 
                            join ts in context.TimeSlots on sc.TimeSlotId equals ts.Id 
                            select new
                            {
                                Id = m.Id,
                                Title = m.Title,
                                Year = m.Year,
                                Description = m.Description,
                                Director = m.Director, // Thêm thông tin đạo diễn (nếu có)
                                StartDate = sc.StartDate,
                                EndDate = sc.EndDate,
                                Screening = new
                                {
                                    RoomId = r.Id,
                                    RoomName = r.Title,
                                    TimeSlotId = ts.Id,
                                    StartTime = ts.StartTime,
                                    EndTime = ts.EndTime,
                         
                                }
                            });


                return Ok(movies);
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
