
namespace IO.Net
{
    public enum ErrorCode
    {
        Unkown = 0,
    }


    public class Error
    {
        public ErrorCode Code { get; private set; }
        public string Message { get; private set; }
        public string CollationId { get; private set; }

        public Error(string message)
        {
            Code = ErrorCode.Unkown;
            Message = message;
        }
    }
}