using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

public class GlobalExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is DuplicateUserException)
        {
            context.Result = new BadRequestObjectResult("A user with that name already exists.");
            context.ExceptionHandled = true;
        }
    }
}
