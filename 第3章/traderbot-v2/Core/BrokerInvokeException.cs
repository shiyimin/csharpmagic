using System;

namespace TraderBot.Core
{
    public class BrokerInvokeException : Exception
    {
        public BrokerInvokeException() : base() { }

        public BrokerInvokeException(string message) : base(message) { }

        public BrokerInvokeException(string message, Exception inner) : base(message, inner) {}
    }
}