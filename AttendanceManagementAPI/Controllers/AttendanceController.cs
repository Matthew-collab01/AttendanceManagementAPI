using attedanceModels;
using attendanceAppService;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Dynamic;
using System.Security.Principal;

namespace AttendanceManagementAPI.Controllers
{
    [Route("api/Attendances")]
    [ApiController]
    public class AttendanceController : ControllerBase
    {
        private readonly attBL _appService;

        public AttendanceController(attBL appService)
        {
            _appService = appService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<attModels>> GetAllAttendances()
        {
            var records = _appService.GetAllAttendances();
            return Ok(records);
        }

        [HttpGet("{ident:guid}")]
        public ActionResult<attModels> GetAttendance(Guid ident)
        {
            var record = _appService.GetAttendance(ident);
            
            if (record == null)
                return NotFound($"No record found for id: {ident}");

            return Ok(record);
        }

        [HttpPost]
        public IActionResult CreateAttendance([FromBody] Models.AttendanceRequest request)
        {
            if (request == null)
            {
                return BadRequest("Student name is required.");
            }

            var att = new attedanceModels.attModels
            {
                ident = Guid.NewGuid(),
                studname = request.name,
                Present = request.present,
                Absent = request.absent,

            };

            _appService.AddStudent(
                request.name, 
                request.present, 
                request.absent
                );

            return CreatedAtAction(
                nameof(GetAttendance),
                new { ident = att.ident },
                att);
        }
        
        [HttpPatch("{ident:guid}")]
        public IActionResult UpdateAttendance(Guid ident, [FromBody] Models.AttendanceRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.name))
                return BadRequest("Student name is required.");

            var success = _appService.UpdateStudentById(
                ident, 
                request.name, 
                request.present, 
                request.absent
                );

            if (!success)
                return NotFound($"No record found for id: {ident}");

            return NoContent();
        }

        [HttpDelete("{ident:guid}")]
        public IActionResult DeleteAttendance(Guid ident)
        {
            var success = _appService.DeleteStudentById(ident);
            if (!success)
                return NotFound($"No record found for id: {ident}");

            return NoContent();
        }
    }

}