namespace Ecommerce_Jair.Server.Models.Results
{
    public class TResult<T>
    {
        public bool Success { get; init; }
        public string? Error { get; init; } = null!;
        //public string ErrorCode { get; init; } = null!;
        public T? Data { get; init; }

        public static TResult<T> Ok(T data) => new TResult<T> { Success = true, Data = data };
        public static TResult<T> Fail(string error) => new TResult<T> { Success = false, //ErrorCode = code,
        Error = error };
    }

}
