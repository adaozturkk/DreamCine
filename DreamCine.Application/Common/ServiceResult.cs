namespace DreamCine.Application.Common
{
    public class ServiceResult<T> where T : class
    {
        public T? Data { get; set; }
        public bool IsSuccess { get; set; }
        public string? ErrorMessage { get; set; }
        public int StatusCode { get; set; }

        public static ServiceResult<T> Success(T data, int statusCode)
        {
            return new ServiceResult<T>()
            {
                Data = data,
                IsSuccess = true,
                StatusCode = statusCode
            };
        }

        public static ServiceResult<T> Failure(string message, int statusCode)
        {
            return new ServiceResult<T>()
            {
                IsSuccess = false,
                ErrorMessage = message,
                StatusCode = statusCode
            };
        }
    }
}