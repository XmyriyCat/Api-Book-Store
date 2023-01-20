using System.Net;
using System.Text.Json;
using BLL.Errors;
using FluentValidation;
using DLL.Errors;
using Google.Apis.Auth;

namespace ApiBookStore.MiddlewareHandlers
{
    public class GlobalErrorHandler
    {
        private readonly RequestDelegate _next;

        public GlobalErrorHandler(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                switch (error)
                {
                    case ValidationException:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;
                    case DbEntityNotFoundException:
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;
                    case InvalidUserLoginError:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;
                    case UserLoginIsNotFound:
                        response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        break;
                    case CreateIdentityUserException:
                        response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        break;
                    case WrongUserPasswordError:
                        response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        break;
                    case InvalidJwtException:
                        response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        break;
                    default:
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }

                var result = JsonSerializer.Serialize(new { message = error.Message });
                await response.WriteAsync(result);
            }
        }
    }
}
