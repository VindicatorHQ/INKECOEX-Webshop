using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebshopService.DTOs;
using WebshopService.DTOs.Responses;
using WebshopService.Repositories.Interface;

namespace WebshopService.Controllers.Admin;

[ApiController]
[Route("api/admin")]
[Authorize(Roles = "Admin")]
public class AnalyticsController(IAnalyticsRepository analyticsRepository) : ControllerBase
{
    [HttpGet("sales/statistics")]
    public async Task<ActionResult<SalesDataResponse>> GetSalesStatistics([FromQuery] int days = 30)
    {
        if (days is <= 0 or > 365)
        {
            return BadRequest(new Error("Het aantal dagen moet tussen 1 en 365 liggen.", "IWS400"));
        }
        
        var stats = await analyticsRepository.GetSalesStatisticsAsync(days);
        
        return Ok(stats);
    }
}