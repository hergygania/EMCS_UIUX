using System.Collections.Generic;

namespace WindowsServiceEbupot.Services.ExceptionHandler
{
    public class ResponseBadRequest
    {
        public int StatusCode { get; set; }
        public bool IsModelValidatonError { get; set; }
        public bool IsCustomErrorObject { get; set; }
        public List<ValidationModelError> Errors { get; set; }
        public string ReferenceErrorCode { get; set; }
        public string ReferenceDocumentLink { get; set; }
        public string StackTrace { get; set; }
        public string Message { get; set; }
    }
}