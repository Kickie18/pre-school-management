using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PreschoolManagement.Application.Common;
using PreschoolManagement.Application.Interfaces;

namespace PreschoolManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DashboardController : ControllerBase
{
    private readonly IReportService _reportService;

    public DashboardController(IReportService reportService)
    {
        _reportService = reportService;
    }

    [HttpGet("summary")]
    public async Task<IActionResult> GetSummaryAsync(CancellationToken cancellationToken)
    {
        var summary = await _reportService.GetDashboardSummaryAsync(cancellationToken);
        return Ok(ApiResponse<object>.Ok(summary));
    }
}
