using System.Net;

namespace Options.Repositories.Models
{
    public class Response<T> : Response
    {
        public T? Data { get; set; }
    }

    public class Response
    {
        public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;

        public string ErrorMessage { get; set; } = string.Empty;
    }
}
