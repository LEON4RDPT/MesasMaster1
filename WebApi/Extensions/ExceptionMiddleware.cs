﻿using System.Net;
using Domain.Exceptions.Mesa;
using Domain.Exceptions.Reserva;
using Domain.Exceptions.Shared;
using Domain.Exceptions.User;

namespace WebApi.Extensions;

public class CustomExceptionMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await next(httpContext);
        }
        catch (MesaAlreadyAtUseException ex)
        {
            httpContext.Response.StatusCode = (int)HttpStatusCode.Conflict;
            await httpContext.Response.WriteAsJsonAsync(new { message = ex.Message });
        }
        catch (InvalidDateException ex)
        {
            httpContext.Response.StatusCode = (int)HttpStatusCode.Conflict;
            await httpContext.Response.WriteAsJsonAsync(new { message = ex.Message });
        }
        catch (ReservaNotFoundException ex)
        {
            httpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
            await httpContext.Response.WriteAsJsonAsync(new { message = ex.Message });
        }
        catch (MesaNotFoundException ex)
        {
            httpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
            await httpContext.Response.WriteAsJsonAsync(new { message = ex.Message });
        }
        catch (InvalidMesaPosition ex)
        {
            httpContext.Response.StatusCode = (int)HttpStatusCode.Conflict;
            await httpContext.Response.WriteAsJsonAsync(new { message = ex.Message });
        }
        catch (EmailNotFoundException ex)
        {
            httpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
            await httpContext.Response.WriteAsJsonAsync(new { message = ex.Message });
        }
        catch (MissingAttributeException ex)
        {
            httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            await httpContext.Response.WriteAsJsonAsync(new { message = ex.Message });
        }
        catch (InvalidEmailException ex)
        {
            httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            await httpContext.Response.WriteAsJsonAsync(new { message = ex.Message });
        }
        catch (UserNotFoundException ex)
        {
            httpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
            await httpContext.Response.WriteAsJsonAsync(new { message = ex.Message });
        }
        catch (LoginUnauthorizedException ex)
        {
            httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            await httpContext.Response.WriteAsJsonAsync(new { message = ex.Message });
        }
        catch (EmailAlreadyInUseException ex)
        {
            httpContext.Response.StatusCode = (int)HttpStatusCode.Conflict;
            await httpContext.Response.WriteAsJsonAsync(new { message = ex.Message });
        }
        catch (MissingEnvironmentValue ex)
        {
            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            await httpContext.Response.WriteAsJsonAsync(new { message = ex.Message });
        }
        catch (ReservaAlreadyDeletedException ex)
        {
            httpContext.Response.StatusCode = (int)HttpStatusCode.Conflict;
            await httpContext.Response.WriteAsJsonAsync(new { message = ex.Message });
        }
    }
}