namespace ApiEnCouches.Controllers;

using ApiEnCouches.Service.DataTransferObject.User;
using ApiEnCouches.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
[ApiController]
[Route("[controller]")]
public class Users : Controller
{
    private readonly IUserService userService;

    public Users(IUserService userService)
    {
        this.userService = userService;
    }

    [HttpGet(Routes.UserRoutes.GetAll)]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var users = await userService.GetAll();
            return Ok(users);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet(Routes.UserRoutes.GetById)]
    public async Task<IActionResult> GetById(int userId)
    {
        try
        {
            var user = await userService.GetById(userId);
            if (user == null)
                return NotFound(new { message = "Utilisateur non trouvé" });

            return Ok(user);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost(Routes.UserRoutes.Create)]
    public async Task<IActionResult> Create([FromBody] CreateUserDTO createUserDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var user = await userService.Create(createUserDto);
            return CreatedAtAction(nameof(GetById), new { userId = user.UserId }, user);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut(Routes.UserRoutes.Update)]
    public async Task<IActionResult> Update(int userId, [FromBody] UpdateUserDTO updateUserDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var user = await userService.Update(userId, updateUserDto);
            return Ok(user);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete(Routes.UserRoutes.Delete)]
    public async Task<IActionResult> Delete(int userId)
    {
        try
        {
            var result = await userService.Delete(userId);
            if (result)
                return Ok(new { message = "Utilisateur supprimé avec succès" });
            else
                return NotFound(new { message = "Utilisateur non trouvé" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
