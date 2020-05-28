using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace heitech.configXt.Core.Exceptions
{
    public class NotAllowedException : Exception
    {
        public IEnumerable<string> NotMetClaims { get; set; }
        public CommandTypes Type { get; set; }
        public NotAllowedException(CommandTypes type, List<string> notMetClaims, string message)
            : this(message)
        {
            Type = type;
            NotMetClaims = notMetClaims;
        }

        public NotAllowedException(string message) : base(message)
        {
        }

        public NotAllowedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NotAllowedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}