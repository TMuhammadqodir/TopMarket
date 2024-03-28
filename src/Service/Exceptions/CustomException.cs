namespace Service.Exceptions;

public class CustomException:Exception
{
    public int StatusCode { get; set; }
    public CustomException(int statusCode, string message) : base(message)
    {
        this.StatusCode = statusCode;
    }

    public CustomException(string message, Exception innerException) : base(message, innerException)
    {}
}
