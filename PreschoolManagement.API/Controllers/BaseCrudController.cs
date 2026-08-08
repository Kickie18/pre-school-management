using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PreschoolManagement.Application.Common;
using PreschoolManagement.Application.Interfaces;

namespace PreschoolManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public abstract class BaseCrudController<TDto, TCreateDto, TUpdateDto> : ControllerBase
{
    private readonly ICrudService<TDto, TCreateDto, TUpdateDto> _service;

    protected BaseCrudController(ICrudService<TDto, TCreateDto, TUpdateDto> service)
    {
        _service = service;
    }

    [HttpGet]
    public virtual async Task<IActionResult> GetAllAsync([FromQuery] QueryParameters query, CancellationToken cancellationToken)
    {
        var data = await _service.GetAllAsync(query, cancellationToken);
        return Ok(ApiResponse<PagedResult<TDto>>.Ok(data));
    }

    [HttpGet("{id:guid}")]
    public virtual async Task<IActionResult> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var data = await _service.GetByIdAsync(id, cancellationToken);
        return data is null
            ? NotFound(ApiResponse<TDto>.Fail("Record not found"))
            : Ok(ApiResponse<TDto>.Ok(data));
    }

    [HttpPost]
    public virtual async Task<IActionResult> CreateAsync([FromBody] TCreateDto request, CancellationToken cancellationToken)
    {
        var data = await _service.CreateAsync(request, cancellationToken);
        return Ok(ApiResponse<TDto>.Ok(data, "Created successfully"));
    }

    [HttpPut("{id:guid}")]
    public virtual async Task<IActionResult> UpdateAsync(Guid id, [FromBody] TUpdateDto request, CancellationToken cancellationToken)
    {
        var data = await _service.UpdateAsync(id, request, cancellationToken);
        return data is null
            ? NotFound(ApiResponse<TDto>.Fail("Record not found"))
            : Ok(ApiResponse<TDto>.Ok(data, "Updated successfully"));
    }

    [HttpDelete("{id:guid}")]
    public virtual async Task<IActionResult> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var deleted = await _service.DeleteAsync(id, cancellationToken);
        return deleted
            ? Ok(ApiResponse<object>.Ok(new { id }, "Deleted successfully"))
            : NotFound(ApiResponse<object>.Fail("Record not found"));
    }
}
