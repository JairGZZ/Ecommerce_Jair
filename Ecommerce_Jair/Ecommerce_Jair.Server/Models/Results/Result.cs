namespace Ecommerce_Jair.Server.Models.Results
{
    public class Result
    {
        public bool Success { get; init; }
        public string? Error { get; init; }
      

        public static Result Ok() => new Result { Success = true };
        public static Result Fail(string error) => new Result { Success = false, Error = error };
    }

}
