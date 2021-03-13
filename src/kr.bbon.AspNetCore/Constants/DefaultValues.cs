using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kr.bbon.AspNetCore
{
    public class DefaultValues
    {
        public const string RouteTemplate = "[area]/v{version:apiVersion}/[controller]";
        public const string ApiVersion = "1.0";
        public const string AreaName = "api";
    }
}
