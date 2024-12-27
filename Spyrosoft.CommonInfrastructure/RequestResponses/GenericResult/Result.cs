using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spyrosoft.CommonInfrastructure.RequestResponses.GenericResult
{
    public sealed class Result<T>
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }

        public static Result<T> Success(string? message = null, T? data = default)
        {
            return new Result<T> { IsSuccess = true, Message = message, Data = data };
        }

        public static Result<T> Failure(string message)
        {
            return new Result<T> { IsSuccess = false, Message = message };
        }
    }

    public sealed class Result
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
        public object? Data { get; set; }

        public static Result Success(string? message = null, object? data = default)
        {
            return new Result { IsSuccess = true, Message = message, Data = data };
        }

        public static Result Failure(string message)
        {
            return new Result { IsSuccess = false, Message = message };
        }
    }
}
