using Google.Protobuf;

namespace IO.Net
{
    public interface IUnCollatedMessage
    {
        IMessage Payload { get; }
    }

    public interface ICollatedMessage<T>
    {
        IMessage Payload { get; }
        void SetCollationId(int id);
    }
}