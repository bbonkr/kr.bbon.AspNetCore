using kr.bbon.AspNetCore;
using kr.bbon.AspNetCore.Models;
using kr.bbon.AspNetCore.Mvc;
using kr.bbon.Core;
using kr.bbon.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace SampleNet6App.Controllers;

/// <summary>
/// AppOptions
/// </summary>
[ApiController]
[ApiVersion(DefaultValues.ApiVersion)]
[Route(DefaultValues.RouteTemplate)]
[Area(DefaultValues.AreaName)]
[Produces("application/json")]
[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponseModel<ErrorModel>))]
[ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiResponseModel<ErrorModel>))]
[ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponseModel<ErrorModel>))]
public class SettingsController : ApiControllerBase
{
    private readonly ILogger logger;
    private readonly AppOptions appOptions;

    public SettingsController(IOptionsMonitor<AppOptions> appOptionsMonitor, ILogger<SettingsController> logger)
    {
        this.appOptions = appOptionsMonitor.CurrentValue;
        this.logger = logger;
    }

    /// <summary>
    /// Get AppOptions value
    /// </summary>
    /// <param name="status">status you want to return</param>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<ApiResponseModel<AppOptions>> Get(int status = 200)
    => status switch
    {
        StatusCodes.Status400BadRequest => throw new ApiException(status, $"You requested Bad request"),
        StatusCodes.Status404NotFound => throw new ApiException(status, $"You requested not found"),
        StatusCodes.Status500InternalServerError => throw new ApiException(status, $"You requested internal server error"),
        _ => StatusCode(status, appOptions)
    };
        
    
}