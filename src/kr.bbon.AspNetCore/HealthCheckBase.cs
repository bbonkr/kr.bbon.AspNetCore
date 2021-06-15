using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace kr.bbon.AspNetCore
{
    public abstract class HealthCheckBase : IHealthCheck
    {
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            return await CheckAsync(cancellationToken);
        }

        public abstract Task<HealthCheckResult> CheckAsync(CancellationToken cancellationToken = default);
    }
}
