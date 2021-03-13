using System;

namespace kr.bbon.AspNetCore.Models
{
    public class ErrorModel
    {
        public string Code { get; set; }

        public string Message { get; set; }

        public ErrorModel InnerError { get; set; }
    }
}
