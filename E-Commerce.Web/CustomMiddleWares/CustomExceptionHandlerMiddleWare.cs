using DomainLayer.Exceptions;
using Shared.ErrorModels;
using System.Net;
using System.Text.Json;

namespace E_Commerce.Web.CustomMiddleWares
{
    public class CustomExceptionHandlerMiddleWare
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CustomExceptionHandlerMiddleWare> _logger;

        public CustomExceptionHandlerMiddleWare(RequestDelegate Next,ILogger<CustomExceptionHandlerMiddleWare> logger )
        {
            _next = Next;
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next.Invoke(httpContext);
                if (httpContext.Response.StatusCode == StatusCodes.Status404NotFound)
                {
                    var Response = new ErrorToReturn()
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        ErrorMessage = $"End Point {httpContext.Request.Path} is Not Found"
                    };
                    await httpContext.Response.WriteAsJsonAsync( Response );
                }
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Something Went Wrong");

                httpContext.Response.StatusCode = ex switch
                {
                    NotFoundException => StatusCodes.Status404NotFound,

                    _ => StatusCodes.Status500InternalServerError
                };
                //Set Status Code For Response
                //httpContext.Response.StatusCode=(int) HttpStatusCode.InternalServerError; or
                //httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

                //Set Content Type For Response
                httpContext.Response.ContentType = "application/json";

                //Response Obj
                var Response = new ErrorToReturn()
                {
                    StatusCode = httpContext.Response.StatusCode,
                    ErrorMessage = ex.Message
                };

                //Return obj As Json

                //var ResponseToReturn =JsonSerializer.Serialize(Response);
                // await httpContext.Response.WriteAsync(ResponseToReturn);  Or

                await httpContext.Response.WriteAsJsonAsync( Response );




            }
        }
    }
}
