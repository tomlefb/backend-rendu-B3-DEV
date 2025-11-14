namespace ApiEnCouches.Controllers;

using ApiEnCouches.Service.DataTransferObject.MeetingRoom;
using ApiEnCouches.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
[ApiController]
[Route("[controller]")]
public class MeetingRooms : Controller
{
    private readonly IMeetingRoomService meetingRoomService;

    public MeetingRooms(IMeetingRoomService meetingRoomService)
    {
        this.meetingRoomService = meetingRoomService;
    }

    [HttpGet(Routes.MeetingRoomRoutes.GetAll)]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var rooms = await meetingRoomService.GetAll();
            return Ok(rooms);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet(Routes.MeetingRoomRoutes.GetById)]
    public async Task<IActionResult> GetById(int roomId)
    {
        try
        {
            var room = await meetingRoomService.GetById(roomId);
            if (room == null)
                return NotFound(new { message = "Salle de réunion non trouvée" });

            return Ok(room);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost(Routes.MeetingRoomRoutes.Create)]
    public async Task<IActionResult> Create([FromBody] CreateMeetingRoomDTO createRoomDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var room = await meetingRoomService.Create(createRoomDto);
            return CreatedAtAction(nameof(GetById), new { roomId = room.RoomId }, room);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete(Routes.MeetingRoomRoutes.Delete)]
    public async Task<IActionResult> Delete(int roomId)
    {
        try
        {
            var result = await meetingRoomService.Delete(roomId);
            if (result)
                return Ok(new { message = "Salle de réunion supprimée avec succès" });
            else
                return NotFound(new { message = "Salle de réunion non trouvée" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet(Routes.MeetingRoomRoutes.Availability)]
    public async Task<IActionResult> GetAvailability(int roomId, [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
    {
        try
        {
            var availability = await meetingRoomService.GetAvailability(roomId, startDate, endDate);
            return Ok(availability);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
