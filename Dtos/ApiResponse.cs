using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductApi.dtos
{
    public class ApiResponse<T>
    {
        public string Status { get; set; } = "success";
        public string? Message { get; set; }
        public T? Data { get; set; }

        public ApiResponse() {}

        public ApiResponse(T data, string? message = null)
        {
            Data = data;
            Message = message;
        }

        public static ApiResponse<T> Success(T data, string? message = null) =>
            new ApiResponse<T>(data, message);

        public static ApiResponse<T> Fail(string message) =>
            new ApiResponse<T> { Status = "fail", Message = message };

        // ✅ Tambahkan overload ini:
        public static ApiResponse<T> Fail(T data, string message) =>
            new ApiResponse<T> { Status = "fail", Message = message, Data = data };
    }


}