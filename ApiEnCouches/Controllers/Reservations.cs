namespace ApiEnCouches.Controllers;

using ApiEnCouches.Service.DataTransferObject.Reservation;
using ApiEnCouches.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[Authorize]
[ApiController]
[Route("[controller]")]
public class Reservations : Controller
{
    private readonly IReservationService reservationService;

    public Reservations(IReservationService reservationService)
    {
        this.reservationService = reservationService;
    }

    private int GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim))
        {
            throw new UnauthorizedAccessException("Token invalide");
        }
        return int.Parse(userIdClaim);
    }

    [HttpGet(Routes.ReservationRoutes.GetAll)]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var userId = GetCurrentUserId();
            var reservations = await reservationService.GetByUserId(userId);
            return Ok(reservations);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet(Routes.ReservationRoutes.GetById)]
    public async Task<IActionResult> GetById(int reservationId)
    {
        try
        {
            var userId = GetCurrentUserId();
            var reservation = await reservationService.GetById(reservationId, userId);

            if (reservation == null)
                return NotFound(new { message = "Réservation non trouvée" });

            return Ok(reservation);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost(Routes.ReservationRoutes.Create)]
    public async Task<IActionResult> Create([FromBody] CreateReservationDTO createReservationDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var userId = GetCurrentUserId();
            var reservation = await reservationService.Create(createReservationDto, userId);
            return CreatedAtAction(nameof(GetById), new { reservationId = reservation.ReservationId }, reservation);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut(Routes.ReservationRoutes.Update)]
    public async Task<IActionResult> Update(int reservationId, [FromBody] UpdateReservationDTO updateReservationDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var userId = GetCurrentUserId();
            var reservation = await reservationService.Update(reservationId, updateReservationDto, userId);
            return Ok(reservation);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete(Routes.ReservationRoutes.Delete)]
    public async Task<IActionResult> Delete(int reservationId)
    {
        try
        {
            var userId = GetCurrentUserId();
            var result = await reservationService.Delete(reservationId, userId);

            if (result)
                return Ok(new { message = "Réservation supprimée avec succès" });
            else
                return NotFound(new { message = "Réservation non trouvée" });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
