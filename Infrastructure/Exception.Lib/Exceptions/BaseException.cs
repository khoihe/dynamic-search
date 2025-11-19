namespace Exception.Lib;

public class BaseException : System.Exception
{
    public string ErrorCode { get; private set; }

    public BaseException(string errorCode, string message)
        : base(message)
    {
        ErrorCode = errorCode;
    }
}