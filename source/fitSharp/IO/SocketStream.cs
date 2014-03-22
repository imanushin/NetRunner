// Copyright © 2009 Syterra Software Inc. All rights reserved.
// The use and distribution terms for this software are covered by the Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file license.txt at the root of this distribution. By using this software in any fashion, you are agreeing
// to be bound by the terms of this license. You must not remove this notice, or any other, from this software.

using System.IO;
using System.Reflection;
using System.Text;

namespace fitSharp.IO
{
    public class SocketStream
    {
        private static readonly string localDir = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName;

        private readonly ISocketModel socket;

        public SocketStream(ISocketModel socket)
        {
            this.socket = socket;
        }

        public string ReadBytes(int bytesToRead)
        {
            var bytes = new byte[bytesToRead];
            int bytesReceived = 0;
            while (bytesReceived < bytesToRead)
            {
                bytesReceived += socket.Receive(bytes, bytesReceived, bytesToRead - bytesReceived);
            }
            var characters = new char[bytesToRead];
            int charCount = Encoding.UTF8.GetDecoder().GetChars(bytes, 0, bytesToRead, characters, 0);
            var result = new StringBuilder(charCount).Append(characters, 0, charCount).ToString();

#if DEBUG
            File.AppendAllText(System.IO.Path.Combine(localDir, "recieved.txt"), result);
#endif
            return result;
        }

        public void Write(string message)
        {
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
#if DEBUG
            File.AppendAllText(System.IO.Path.Combine(localDir, "sent.txt"), message);
#endif
            socket.Send(messageBytes);
        }
    }
}
