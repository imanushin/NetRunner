using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text;

namespace NetRunner.Executable
{
    internal sealed class FitnesseCommunicator : IDisposable
    {
        private readonly Socket socket;
        private bool isDisposed;

        public FitnesseCommunicator(string hostName, int port, string token)
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(hostName, port);

            Trace.WriteLine("FitnesseCommunicator open socket to host: {0}, port: {1}");

            EstablishConnection(token);
        }

        public void SendDocument(string document)
        {
            Write(FormatDocument(document));
        }

        public void SendCounts(TestCounts counts)
        {
            var builder = new StringBuilder();
            builder.Append(FormatInteger(0));
            builder.Append(FormatInteger(counts.SuccessCount));
            builder.Append(FormatInteger(counts.FailCount));
            builder.Append(FormatInteger(counts.IgnoreCount));
            builder.Append(FormatInteger(counts.ExceptionCount));

            Transmit(builder.ToString());
        }

        public void Transmit(string message)
        {
            Write(message);
        }

        private void EstablishConnection(string request)
        {
            Trace.WriteLine("HTTP request: {0}");
            Transmit(FormatRequest(request));

            Trace.WriteLine("Validating connection...");
            int statusSize = ReceiveInteger();
            if (statusSize == 0)
            {
                Trace.WriteLine("\t...ok\n");
            }
            else
            {
                String errorMessage = ReadBytes(statusSize);
                Trace.WriteLine("...failed because: " + errorMessage);
                Console.WriteLine("An error occured while connecting to client.");

                throw new InvalidOperationException("Communication error: " + errorMessage);
            }
        }

        private void Write(string message)
        {
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            socket.Send(messageBytes);
        }


        public string ReceiveDocument()
        {
            int documentLength = ReceiveInteger();

            if (documentLength == 0)
                return string.Empty;

            return ReadBytes(documentLength);
        }

        private string ReadBytes(int bytesToRead)
        {
            var bytes = new byte[bytesToRead];
            int bytesReceived = 0;
            while (bytesReceived < bytesToRead)
            {
                bytesReceived += socket.Receive(bytes, bytesReceived, bytesToRead - bytesReceived, SocketFlags.None);
            }
            var characters = new char[bytesToRead];
            int charCount = Encoding.UTF8.GetDecoder().GetChars(bytes, 0, bytesToRead, characters, 0);

            return new StringBuilder(charCount).Append(characters, 0, charCount).ToString();
        }

        private int ReceiveInteger()
        {
            return Convert.ToInt32(ReadBytes(10));
        }

        private static string FormatInteger(int encodeInteger)
        {
            string numberPartOfString = "" + encodeInteger;
            return new String('0', 10 - numberPartOfString.Length) + numberPartOfString;
        }

        private static string FormatDocument(string document)
        {
            return FormatInteger(Encoding.UTF8.GetBytes(document).Length) + document;
        }

        private static string FormatRequest(string token)
        {
            return "GET /?responder=socketCatcher&ticket=" + token + " HTTP/1.1\r\n\r\n";
        }

        ~FitnesseCommunicator()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (socket != null && !isDisposed)
            {
                socket.Dispose();
                isDisposed = true;
            }
                
        }
    }
}
