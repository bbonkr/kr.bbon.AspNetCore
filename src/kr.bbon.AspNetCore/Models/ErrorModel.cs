using System;
using System.Collections.Generic;

namespace kr.bbon.AspNetCore.Models
{
    public class ErrorModel
    {
        public string Code { get; set; }

        public string Message { get; set; }

        [Obsolete("Use InnerErrors instead of this")]
        public ErrorModel InnerError { get; set; } = default;

        public IList<ErrorModel> InnerErrors { get; set; } = new List<ErrorModel>();
    }
}
