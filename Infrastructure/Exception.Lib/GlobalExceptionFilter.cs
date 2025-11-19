namespace Exception.Lib;

public class GlobalExceptionFilter : IExceptionFilter
{
    private readonly ILoggerAdapter<GlobalExceptionFilter> _logger;

    public GlobalExceptionFilter(ILoggerAdapter<GlobalExceptionFilter> logger)
    {
        _logger = logger;
    }

    public void OnException(ExceptionContext context)
    {
        var exception = context.Exception;

        context.ExceptionHandled = true;

        _logger.LogError(exception.ToString(), exception.Message.ToString());

        switch (exception)
        {
            case EntityNotFoundException notFoundValidation:
                context.Result = new NotFoundObjectResult(new { IsSuccess = false, ErrorCode = notFoundValidation.ErrorCode, Message = notFoundValidation.Message });
                context.ExceptionHandled = true;
                break;
            case BaseException baseException:
                context.Result = new BadRequestObjectResult(new { IsSuccess = false, ErrorCode = baseException.ErrorCode, Message = baseException.Message });
                context.ExceptionHandled = true;
                break;
            default:
                context.Result = new BadRequestObjectResult(new { IsSuccess = false, ErrorCode = ErrorCodes.UNKNOWN });
                break;
        }
    }
}