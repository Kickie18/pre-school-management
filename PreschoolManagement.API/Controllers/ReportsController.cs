using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PreschoolManagement.Application.Common;
using PreschoolManagement.Application.DTOs;
using PreschoolManagement.Application.Interfaces;

namespace PreschoolManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "SuperAdmin,PreschoolAdmin,Teacher")]
public class ReportsController : ControllerBase
{
    private readonly IReportService _reportService;

    public ReportsController(IReportService reportService)
    {
        _reportService = reportService;
    }

    [HttpPost("attendance")]
    public async Task<IActionResult> AttendanceAsync(ReportFilterDto filter, CancellationToken cancellationToken)
    {
        return Ok(ApiResponse<object>.Ok(await _reportService.AttendanceReportAsync(filter, cancellationToken)));
    }

    [HttpPost("students")]
    public async Task<IActionResult> StudentsAsync(ReportFilterDto filter, CancellationToken cancellationToken)
    {
        return Ok(ApiResponse<object>.Ok(await _reportService.StudentReportAsync(filter, cancellationToken)));
    }

    [HttpPost("fee-collection")]
    public async Task<IActionResult> FeeCollectionAsync(ReportFilterDto filter, CancellationToken cancellationToken)
    {
        return Ok(ApiResponse<object>.Ok(await _reportService.FeeCollectionReportAsync(filter, cancellationToken)));
    }

    [HttpPost("payment-due")]
    public async Task<IActionResult> PaymentDueAsync(ReportFilterDto filter, CancellationToken cancellationToken)
    {
        return Ok(ApiResponse<object>.Ok(await _reportService.PaymentDueReportAsync(filter, cancellationToken)));
    }

    [HttpPost("teachers")]
    public async Task<IActionResult> TeachersAsync(ReportFilterDto filter, CancellationToken cancellationToken)
    {
        return Ok(ApiResponse<object>.Ok(await _reportService.TeacherReportAsync(filter, cancellationToken)));
    }

    [HttpPost("inventory")]
    public async Task<IActionResult> InventoryAsync(ReportFilterDto filter, CancellationToken cancellationToken)
    {
        return Ok(ApiResponse<object>.Ok(await _reportService.InventoryReportAsync(filter, cancellationToken)));
    }

    [HttpPost("daily-checkinout")]
    public async Task<IActionResult> DailyCheckInOutAsync(ReportFilterDto filter, CancellationToken cancellationToken)
    {
        return Ok(ApiResponse<object>.Ok(await _reportService.DailyCheckInOutReportAsync(filter, cancellationToken)));
    }
}
