using Microsoft.AspNetCore.Http;

namespace Zora.WebApi;

using Microsoft.AspNetCore.Mvc;
using Zora.Core.Features.StoryServices;
using Zora.Core.Models;

[ApiController]
[Route("api/story")]
public class StoryController(IStoryReadService readService, IStoryWriteService writeService)
    : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<Story>>> GetAll(CancellationToken cancellationToken) =>
        Ok(await readService.GetAllAsync(cancellationToken));

    [HttpGet("tour/{tourId:long}")]
    public async Task<ActionResult<List<Story>>> GetByTour(
        long tourId,
        CancellationToken cancellationToken
    ) => Ok(await readService.GetByTourIdAsync(tourId, cancellationToken));

    [HttpGet("user/{userId:long}")]
    public async Task<ActionResult<List<Story>>> GetByUser(
        long userId,
        CancellationToken cancellationToken
    ) => Ok(await readService.GetByUserIdAsync(userId, cancellationToken));

    [HttpPost]
    public async Task<ActionResult<Story>> Create(
        [FromForm] long userId,
        [FromForm] long tourId,
        [FromForm] string content,
        [FromForm] List<IFormFile> images,
        CancellationToken cancellationToken
    )
    {
        var dto = new CreateStory
        {
            UserId = userId,
            TourId = tourId,
            Content = content,
        };
        var result = await writeService.CreateAsync(dto, images, cancellationToken);
        return Ok(result);
    }

    [HttpPut("{storyId:long}")]
    public async Task<ActionResult<Story>> Update(
        long storyId,
        [FromForm] long userId,
        [FromForm] long tourId,
        [FromForm] string content,
        [FromForm] List<string> keepImagePaths,
        [FromForm] List<IFormFile> newImages,
        CancellationToken cancellationToken
    )
    {
        var dto = new CreateStory
        {
            UserId = userId,
            TourId = tourId,
            Content = content,
        };
        var updated = await writeService.UpdateAsync(
            storyId,
            dto,
            keepImagePaths,
            newImages,
            cancellationToken
        );
        return updated is null ? NotFound() : Ok(updated);
    }

    [HttpDelete("{storyId:long}")]
    public async Task<ActionResult> Delete(long storyId, CancellationToken cancellationToken)
    {
        var deleted = await writeService.DeleteAsync(storyId, cancellationToken);
        return deleted ? NoContent() : NotFound();
    }
}
