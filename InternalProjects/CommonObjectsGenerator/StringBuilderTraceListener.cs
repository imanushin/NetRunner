using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using System;
using System.Text;

namespace CommonObjectsGenerator
{
    public sealed class StringBuilderTraceListener : TraceListener
    {
        private readonly StringBuilder stringBuilder = new StringBuilder();

        public override void Write(string message)
        {
            stringBuilder.Append(message);
        }

        public override void WriteLine(string message)
        {
            stringBuilder.AppendLine(message);
        }

        public override string ToString()
        {
            return stringBuilder.ToString();
        }
    }
}