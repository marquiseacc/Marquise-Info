

namespace Marquise_Web.Model.Utilities
{
    public class OperationResult<T>
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }

        public static OperationResult<T> Success(T data, string message = "عملیات با موفقیت انجام شد.")
        {
            return new OperationResult<T>
            {
                IsSuccess = true,
                Message = message,
                Data = data
            };
        }

        public static OperationResult<T> Failure(string message)
        {
            return new OperationResult<T>
            {
                IsSuccess = false,
                Message = message,
                Data = default
            };
        }
    }
}
