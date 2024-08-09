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
        catch (InvalidDateException e)
        {
            ctx.Response.StatusCode = StatusCodes.Status400BadRequest;
            var res = new ControllerResponse { Result = EResult.Failed, Message = e.Message };
            await ctx.Response.WriteAsJsonAsync(res);
        }
        catch (InvalidEmailException e)
        {
            ctx.Response.StatusCode = StatusCodes.Status400BadRequest;
            var res = new ControllerResponse { Result = EResult.Failed, Message = e.Message };
            await ctx.Response.WriteAsJsonAsync(res);
        }
        catch (InvalidInputDataException e)
        {
            ctx.Response.StatusCode = StatusCodes.Status400BadRequest;
            var res = new ControllerResponse { Result = EResult.Failed, Message = e.Message };
            await ctx.Response.WriteAsJsonAsync(res);
        }
        catch (MaximumCapacityException e)
        {
            ctx.Response.StatusCode = StatusCodes.Status400BadRequest;
            var res = new ControllerResponse { Result = EResult.Failed, Message = e.Message };
            await ctx.Response.WriteAsJsonAsync(res);
        }
        catch (NotFoundException e)
        {
            ctx.Response.StatusCode = StatusCodes.Status404NotFound;
            var res = new ControllerResponse { Result = EResult.Failed, Message = e.Message };
            await ctx.Response.WriteAsJsonAsync(res);
        }
        catch (UnauthorizedException e)
        {
            ctx.Response.StatusCode = StatusCodes.Status401Unauthorized;
            var res = new ControllerResponse { Result = EResult.Failed, Message = e.Message };
            await ctx.Response.WriteAsJsonAsync(res);
        }
        catch (UnauthorizedAccessException)
        {
            ctx.Response.StatusCode = StatusCodes.Status401Unauthorized;
            var res = new ControllerResponse { Result = EResult.Failed, Message = "You must authorize first." };
            await ctx.Response.WriteAsJsonAsync(res);
        }
        catch (Exception e)
        {
#if DEBUG
            Console.WriteLine($"Error => {e.Message}");
#endif
            ctx.Response.StatusCode = StatusCodes.Status500InternalServerError;
            var res = new ControllerResponse { Result = EResult.Failed, Message = "An error occurred." };
            await ctx.Response.WriteAsJsonAsync(res);
        }
    }
}