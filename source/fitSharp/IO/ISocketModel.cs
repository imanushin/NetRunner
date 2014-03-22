namespace fitSharp.IO
{
    public interface ISocketModel
    {
        int Receive(byte[] buffer, int offset, int bytesToRead);
        void Send(byte[] buffer);
        void Close();
    }
}