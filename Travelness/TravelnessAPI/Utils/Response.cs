using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TravelnessAPI.Utils
{
    public class Response<T>
    {
        public bool Success { get; protected set; }
        public string Message { get; protected set; }
        public T Data { get; protected set; }

        public Response(bool success, string message)
        {
            Success = success;
            Message = message;
        }

        public Response(bool success, string message, T data)
        {
            Success = success;
            Message = message;
            Data = data;
        }

        public Response(T data)
        {
            Success = true;
            Message = string.Empty;
            Data = data;
        }

        public Response(string message)
        {
            Success = false;
            Message = message;
        }
    }
}
