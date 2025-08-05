using Ecommerce_Jair.Server.Models.Results;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_Jair.Server.Extensions
{

    //extension para que el controlador envie los errores como respuestas http

    //public static class ResultExtensions
    //{
    //    public static IActionResult ToActionResult<T>(this TResult<T> result)
    //    {
    //        if (result.Success)
    //            return new OkObjectResult(result.Data);

    //        return result.ErrorCode switch
    //        {
    //            "USER_ALREADY_EXISTS" => new ConflictObjectResult(result.Error),
    //            "PASSWORD_MISMATCH" => new BadRequestObjectResult(result.Error),
    //            _ => new ObjectResult("Error inesperado") { StatusCode = 500 }
    //        };
    //    }
    //}
}
