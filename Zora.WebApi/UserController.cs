using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Zora.Core.Features.UserServices;
using Zora.Core.Models;

namespace Zora.WebApi;

[ApiController]
[Route("api/[controller]")]
public class UserController(IUserReadService userReadService, IUserWriteService userWriteService)
    : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<User>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IReadOnlyList<User>>> GetUsersAsync(
        [FromQuery] string? name,
        CancellationToken cancellationToken
    )
    {
        return await userReadService.GetAllAsync(cancellationToken, name);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status405MethodNotAllowed)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<User>> CreateUserAsync(
        [FromBody] CreateUser createUser,
        CancellationToken cancellationToken
    )
    {
        if (!EmailIsValid(createUser.Email))
        {
            return BadRequest("Invalid email format.");
        }

        return await userWriteService.CreateAsync(createUser, cancellationToken);
    }

    [HttpPut("{userId}")]
    public async Task<ActionResult<User>> UpdateUserAsync(
        [FromRoute] long userId,
        [FromBody] UpdateUser updateUser,
        CancellationToken cancellationToken
    )
    {
        if (updateUser.Email != null && !EmailIsValid(updateUser.Email))
        {
            return BadRequest("Invalid email format.");
        }

        var updatedUser = await userWriteService.UpdateAsync(userId, updateUser, cancellationToken);

        if (updatedUser == null)
        {
            return NotFound();
        }

        return updatedUser;
    }

    [HttpDelete("{userId}")]
    public async Task<ActionResult> DeleteUserAsync(
        [FromRoute] long userId,
        CancellationToken cancellationToken
    )
    {
        await userWriteService.DeleteAsync(userId, cancellationToken);
        return NoContent();
    }

    [HttpPost("{userId}/join-tour/{tourId}")]
    public async Task<IActionResult> JoinTour(
        [FromRoute] long userId,
        [FromRoute] long tourId,
        CancellationToken cancellationToken
    )
    {
        var success = await userWriteService.JoinTourAsync(userId, tourId, cancellationToken);
        return success ? Ok() : BadRequest("Cannot join tour.");
    }

    [HttpPost("{userId}/leave-tour/{tourId}")]
    public async Task<IActionResult> LeaveTour(
        [FromRoute] long userId,
        [FromRoute] long tourId,
        CancellationToken cancellationToken
    )
    {
        var success = await userWriteService.LeaveTourAsync(userId, tourId, cancellationToken);
        return success ? Ok() : BadRequest("Cannot leave tour.");
    }

    [HttpPost("{userId}/tours/{tourId}/equipment/{equipmentId}/check")]
    public async Task<IActionResult> CheckItem(
        [FromRoute] long userId,
        [FromRoute] long tourId,
        [FromRoute] long equipmentId,
        [FromQuery] bool isChecked,
        CancellationToken cancellationToken
    )
    {
        await userWriteService.UpdateCheckListItemAsync(
            userId,
            tourId,
            equipmentId,
            isChecked,
            cancellationToken
        );
        return Ok(new { success = true });
    }

    private static bool EmailIsValid(string email)
    {
        return System.Text.RegularExpressions.Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
    }

    [HttpGet("{userId}/tours/{tourId}/is-joined")]
    public async Task<IActionResult> IsUserJoinedTour(
        [FromRoute] long userId,
        [FromRoute] long tourId,
        CancellationToken cancellationToken
    )
    {
        var isJoined = await userReadService.IsUserJoinedTourAsync(
            userId,
            tourId,
            cancellationToken
        );
        return Ok(new { signedUp = isJoined });
    }
}
