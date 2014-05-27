using System;
using System.Diagnostics;
using System.Globalization;
using System.Net.Sockets;
using System.Text;
using NetRunner.Executable.Common;

namespace NetRunner.Executable
{
    internal sealed class FitnesseCommunicator : IDisposable
    {
        public const int IntengerFitnessSize = 10;
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
            builder.Append(0.ToFinnessIntegerString());
            builder.Append(counts.SuccessCount.ToFinnessIntegerString());
            builder.Append(counts.FailCount.ToFinnessIntegerString());
            builder.Append(counts.IgnoreCount.ToFinnessIntegerString());
            builder.Append(counts.ExceptionCount.ToFinnessIntegerString());

            Write(builder.ToString());
        }

        private void EstablishConnection(string request)
        {
            Trace.WriteLine("HTTP request: {0}");
            Write(FormatRequest(request));

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
                Trace.WriteLine("An error occured while connecting to client.");

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
            return Convert.ToInt32(ReadBytes(IntengerFitnessSize));
        }

        private static string FormatDocument(string document)
        {
            return Encoding.UTF8.GetBytes(document).Length.ToFinnessIntegerString() + document;
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
                try
                {
                    socket.Dispose();
                }
                catch (Exception ex)
                {
                    Trace.TraceError("Unable to close socket: {0}", ex);
                }

                isDisposed = true;
            }
                
        }
    }
}
