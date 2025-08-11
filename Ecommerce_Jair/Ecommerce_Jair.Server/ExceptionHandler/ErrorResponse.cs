namespace Ecommerce_Jair.Server.ExceptionHandler
{
    public class ErrorResponse
    {
        public string? Title { get; set; }

        public string? ExceptionMessage { get; set; }
        public int? StatusCode { get; set; }

    }
}
