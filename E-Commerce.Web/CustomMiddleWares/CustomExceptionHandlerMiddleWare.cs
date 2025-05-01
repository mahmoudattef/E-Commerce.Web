//using Azure;
//using DomainLayer.Exceptions;
//using Shared.ErrorModels;
//using System.Net;
//using System.Text.Json;

//namespace E_Commerce.Web.CustomMiddleWares
//{
//    public class CustomExceptionHandlerMiddleWare
//    {
//        private readonly RequestDelegate _next;
//        private readonly ILogger<CustomExceptionHandlerMiddleWare> _logger;

//        public CustomExceptionHandlerMiddleWare(RequestDelegate Next, ILogger<CustomExceptionHandlerMiddleWare> logger)
//        {
//            _next = Next;
//            _logger = logger;
//        }
//        public async Task InvokeAsync(HttpContext httpContext)
//        {
//            try
//            {
//                await _next.Invoke(httpContext);
//                if (httpContext.Response.StatusCode == StatusCodes.Status404NotFound)
//                {
//                    var Response = new ErrorToReturn()
//                    {
//                        StatusCode = StatusCodes.Status404NotFound,
//                        ErrorMessage = $"End Point {httpContext.Request.Path} is Not Found"
//                    };
//                    await httpContext.Response.WriteAsJsonAsync(Response);
//                }
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Something Went Wrong");
//                var Response = new ErrorToReturn();

//                httpContext.Response.StatusCode = ex switch
//                {
//                    NotFoundException => StatusCodes.Status404NotFound,
//                    UnAuthorizedException => StatusCodes.Status404NotFound,
//                    BadRequestException badRequestException => GetBadRequestErrors(badRequestException, Response),
//                    _ => StatusCodes.Status500InternalServerError
//                };
//                //Set Status Code For Response
//                //httpContext.Response.StatusCode=(int) HttpStatusCode.InternalServerError; or
//                //httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

//                //Set Content Type For Response
//                httpContext.Response.ContentType = "application/json";

//                //Response Obj
//                var response = new ErrorToReturn()
//                {
//                    StatusCode = httpContext.Response.StatusCode,
//                    ErrorMessage = ex.Message
//                };

//                //Return obj As Json

//                //var ResponseToReturn =JsonSerializer.Serialize(Response);
//                // await httpContext.Response.WriteAsync(ResponseToReturn);  Or

//                await httpContext.Response.WriteAsJsonAsync(response);

//            }
//        }

//        private int GetBadRequestErrors(BadRequestException badRequestException, ErrorToReturn response)
//        {
//            response.ErrorMessage = badRequestException.Message;
//            return StatusCodes.Status400BadRequest;
//        }
//    }
//}
//using Azure;
//using DomainLayer.Exceptions;
//using Shared.ErrorModels;
//using System.Net;
//using System.Text.Json;

//namespace E_Commerce.Web.CustomMiddleWares
//{
//    public class CustomExceptionHandlerMiddleWare
//    {
//        private readonly RequestDelegate _next;
//        private readonly ILogger<CustomExceptionHandlerMiddleWare> _logger;

//        public CustomExceptionHandlerMiddleWare(RequestDelegate next, ILogger<CustomExceptionHandlerMiddleWare> logger)
//        {
//            _next = next;
//            _logger = logger;
//        }

//        public async Task InvokeAsync(HttpContext httpContext)
//        {
//            try
//            {
//                await _next.Invoke(httpContext);

//                if (httpContext.Response.StatusCode == StatusCodes.Status404NotFound)
//                {
//                    var response = new ErrorToReturn()
//                    {
//                        StatusCode = StatusCodes.Status404NotFound,
//                        ErrorMessage = $"End Point {httpContext.Request.Path} is Not Found"
//                    };

//                    await httpContext.Response.WriteAsJsonAsync(response);
//                }
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Something Went Wrong");

//                var response = new ErrorToReturn();

//                httpContext.Response.StatusCode = ex switch
//                {
//                    NotFoundException => StatusCodes.Status404NotFound,
//                    UnAuthorizedException => StatusCodes.Status404NotFound,
//                    BadRequestException badRequestException => GetBadRequestErrors(badRequestException, response),
//                    _ => StatusCodes.Status500InternalServerError
//                };

//                response.StatusCode = httpContext.Response.StatusCode;
//                if (string.IsNullOrWhiteSpace(response.ErrorMessage))
//                {
//                    response.ErrorMessage = ex.Message;
//                }

//                httpContext.Response.ContentType = "application/json";
//                await httpContext.Response.WriteAsJsonAsync(response);
//            }
//        }

//        private int GetBadRequestErrors(BadRequestException badRequestException, ErrorToReturn response)
//        {
//            response.ErrorMessage = badRequestException.Message;
//            return StatusCodes.Status400BadRequest;
//        }
//    }
//}

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

        public CustomExceptionHandlerMiddleWare(RequestDelegate next, ILogger<CustomExceptionHandlerMiddleWare> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);

                // Handle 404 after request pipeline ends
                if (httpContext.Response.StatusCode == StatusCodes.Status404NotFound && !httpContext.Response.HasStarted)
                {
                    var response = new ErrorToReturn
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        ErrorMessage = $"Endpoint {httpContext.Request.Path} not found"
                    };
                    httpContext.Response.ContentType = "application/json";
                    await httpContext.Response.WriteAsJsonAsync(response);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred");

                var response = new ErrorToReturn();

                httpContext.Response.StatusCode = ex switch
                {
                    NotFoundException => StatusCodes.Status404NotFound,
                    UnAuthorizedException => StatusCodes.Status401Unauthorized,
                    BadRequestException badRequest => GetBadRequestErrors(badRequest, response),
                    _ => StatusCodes.Status500InternalServerError
                };

                httpContext.Response.ContentType = "application/json";

                response.StatusCode = httpContext.Response.StatusCode;
                response.ErrorMessage = ex.Message;

                if (ex is BadRequestException badRequestEx && badRequestEx.Errors != null)
                {
                    response.Errors = badRequestEx.Errors;
                }

                await httpContext.Response.WriteAsJsonAsync(response);
            }
         }

        private int GetBadRequestErrors(BadRequestException badRequestException, ErrorToReturn response)
        {
            response.ErrorMessage = badRequestException.Message;
            response.Errors = badRequestException.Errors;
            return StatusCodes.Status400BadRequest;
        }
    }
}
