using System;

using kr.bbon.AspNetCore;
using kr.bbon.AspNetCore.Mvc;
using kr.bbon.AspNetCore.Filters;

using Microsoft.AspNetCore.Mvc;
using SampleApp.Models;

namespace SampleApp.Controllers
{
    [ApiVersion(DefaultValues.ApiVersion)]
    [ApiController]
    [Route(DefaultValues.RouteTemplate)]
    [Area(DefaultValues.AreaName)]
    [ApiExceptionHandlerFilter]
    public class SamplesController : ApiControllerBase
    {
        [HttpPost]
        public IActionResult CreateSample([FromBody] CreateSampleModel payload)
        {
            var sample = new SampleModel
            {
                Id = Guid.NewGuid(),
                Name = payload.Name,
            };

            return Created($"{sample.Id}", sample);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public IActionResult UpdateSample([FromRoute] Guid id, [FromBody] UpdateSampleModel payload)
        {

            return Accepted(payload);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public IActionResult DeleteSample([FromRoute] Guid id)
        {

            return Accepted();
        }
    }
}
