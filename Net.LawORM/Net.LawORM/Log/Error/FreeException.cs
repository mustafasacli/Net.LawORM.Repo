using System;
using System.Runtime.Serialization;

namespace Net.LawORM.Log.Error
{
    public class FreeException : Exception
    {
        private FreeLogger _logEntry;

        public FreeException(Exception exc, FreeLogger log)
        {
            _logEntry = log;
            if (exc != null)
            {
                _logEntry.StackTrace = exc.StackTrace;
                _logEntry.Message = exc.Message;
                _logEntry.LogType = LogTypes.Error;
            }
            _logEntry.Write();
        }


        public FreeException()
        {
        }

        public FreeException(string message)
            : base(message)
        {
        }

        public FreeException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected FreeException(
          SerializationInfo info,
          StreamingContext context)
            : base(info, context)
        {
        }

    }
}