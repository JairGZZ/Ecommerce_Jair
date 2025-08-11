using Microsoft.AspNetCore.Diagnostics;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Ecommerce_Jair.Server.ExceptionHandler
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            var response = new ErrorResponse();

            switch (exception)
            {
                case NotImplementedException:
                    response.Title = "Algo Salio Mal";
                    response.StatusCode = StatusCodes.Status500InternalServerError;
                    response.ExceptionMessage = exception.Message;
                    break;
                case UnauthorizedAccessException:
                    response.Title = "No esta Autorizado";
                    response.StatusCode = StatusCodes.Status401Unauthorized;
                    response.ExceptionMessage = exception.Message;
                    break;
                   
                default :
                    response.Title = "Error del Servidor";
                    response.StatusCode = StatusCodes.Status500InternalServerError;
                    response.ExceptionMessage = "Ocurrio un error inesperado";
                    break;

            }
          

            await httpContext.Response.WriteAsJsonAsync(response,cancellationToken);


            return true;

        }
    }

}
