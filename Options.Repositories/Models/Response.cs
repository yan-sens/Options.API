using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Options.Repositories.Models
{
    public class Response<T>
    {
        public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;

        public string ErrorMessage { get; set; } = string.Empty;

        public T? Data { get; set; }
    }
}
