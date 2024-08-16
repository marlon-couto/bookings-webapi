using BookingsWebApi.Exceptions;
using BookingsWebApi.Helpers;

namespace BookingsWebApi.Middlewares;

public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext ctx)
    {
        try
        {
            await _next(ctx);
        }
        catch (Exception exception)
        {
            await HandleExceptionAsync(ctx, exception);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext ctx, Exception exception)
    {
        ctx.Response.ContentType = "application/json";
        var res = new ControllerResponse { Result = EResult.Failed, Message = exception.Message };

        // Handle different exceptions.
        switch (exception)
        {
            case InvalidOperationException:
                ctx.Response.StatusCode = StatusCodes.Status400BadRequest;
                break;
            case InvalidDateException
            or InvalidEmailException
            or InvalidInputDataException
            or MaximumCapacityException:
                ctx.Response.StatusCode = StatusCodes.Status400BadRequest;
                break;
            case NotFoundException:
                ctx.Response.StatusCode = StatusCodes.Status404NotFound;
                break;
            case UnauthorizedException:
                ctx.Response.StatusCode = StatusCodes.Status401Unauthorized;
                break;
            case UnauthorizedAccessException:
                ctx.Response.StatusCode = StatusCodes.Status401Unauthorized;
                res.Message = "You must authorize first.";
                break;
            default:
                ctx.Response.StatusCode = StatusCodes.Status500InternalServerError;
#if RELEASE
                res.Message = "An error occurred";
#endif
                break;
        }

        res.StatusCode = ctx.Response.StatusCode;
        await ctx.Response.WriteAsJsonAsync(res);
    }
}
