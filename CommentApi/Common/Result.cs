namespace CommentApi.Common
{
    public class Result<T>
    {
        public bool IsSuccess { get; init; }
        public bool IsFailure => !IsSuccess;
        public T? Data { get; init; }
        public string Message { get; init; } = string.Empty;
        public List<string> Errors { get; init; } = [];
        public ExceptionType ExceptionType { get; set; } = ExceptionType.None;

        public static Result<T> Success(T data, string message = "Success")
        {
            return new Result<T>
            {
                IsSuccess = true,
                Data = data,
                Message = message
            };
        }

        public static Result<T> Failure(string message = "Failed", List<string>? errors = null, ExceptionType exceptionType = ExceptionType.None)
        {
            return new Result<T>
            {
                IsSuccess = false,
                Message = message,
                Errors = errors ?? [],
                ExceptionType = exceptionType
            };
        }
    }

    public class Result : Result<object>
    {
        public static Result Success(string message = "Success")
        {
            return new Result
            {
                IsSuccess = true,
                Message = message
            };
        }

        public new static Result Failure(string message = "Failed", List<string>? errors = null, ExceptionType exceptionType = ExceptionType.None)
        {
            return new Result
            {
                IsSuccess = false,
                Message = message,
                Errors = errors ?? [],
                ExceptionType = exceptionType
            };
        }
    }
}
