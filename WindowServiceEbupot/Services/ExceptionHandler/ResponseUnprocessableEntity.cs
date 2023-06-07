using System.Collections.Generic;

namespace WindowsServiceEbupot.Services.ExceptionHandler
{
    public class ResponseUnprocessableEntity
    {
        public int StatusCode { get; set; }
        public List<ValidationModelError> ValidationErrors { get; set; }
        public string StackTrace { get; set; }
        public string Message { get; set; }
    }
}