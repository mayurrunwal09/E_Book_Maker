using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Book.Common.Model
{
    public class APIResponse<T>
    {

        public APIResponse(HttpStatusCode statusCode, APIStatus status, T data, string message = null!, string title = null!)
        {
            StatusCode = statusCode;
            Status = status.ToString();
            Data = data;
            Message = message;
            Title = title;
        }

        /// <summary>
        /// Status code
        /// </summary>
        public HttpStatusCode StatusCode { get; set; }
        /// <summary>
        /// Status of the API
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// Message for the API
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// Data for the API
        /// </summary>
        public T Data { get; set; }
        /// <summary>
        /// Title of the API
        /// </summary>
        public string Title { get; set; }

    }
    public enum APIStatus
    {
        Success,
        Failure,
        Warning,
        Exists,
        NotFound,
        OK
    }
}
