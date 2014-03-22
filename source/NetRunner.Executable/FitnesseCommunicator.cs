using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace NetRunner.Executable
{
    internal sealed class FitnesseCommunicator : IDisposable
    {
        private readonly Socket socket;

        public FitnesseCommunicator(string hostName, int port)
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(hostName, port);
        }

        public void SendDocument(string document)
        {
            Write(Protocol.FormatDocument(document));
        }

        public void Transmit(string message)
        {
            Write(message);
        }
        
        public void EstablishConnection(string request)
        {
            Trace.WriteLine("HTTP request: {0}");
            Transmit(request);

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

        public void Close()
        {
            socket.Close();
        }

        private int ReceiveInteger()
        {
            return Convert.ToInt32(ReadBytes(10));
        }
        public void Dispose()
        {
            if (socket != null)
                socket.Dispose();
        }


    }

    internal static class Protocol
    {
        public static string FormatInteger(int encodeInteger)
        {
            string numberPartOfString = "" + encodeInteger;
            return new String('0', 10 - numberPartOfString.Length) + numberPartOfString;
        }

        public static string FormatDocument(string document)
        {
            return FormatInteger(Encoding.UTF8.GetBytes(document).Length) + document;
        }

        public static string FormatRequest(string token)
        {
            return "GET /?responder=socketCatcher&ticket=" + token + " HTTP/1.1\r\n\r\n";
        }

        public static string FormatRequest(string pageName, bool usingDownloadedPaths, string suiteFilter)
        {
            string request = "GET /" + pageName + "?responder=fitClient";
            if (usingDownloadedPaths)
                request += "&includePaths=yes";
            if (suiteFilter != null)
                request += "&suiteFilter=" + suiteFilter;
            return request + " HTTP/1.1\r\n\r\n";
        }
    }
}
