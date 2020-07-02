using System;
using System.Collections.Generic;
using System.Linq;

namespace Apizr
{
    public struct Error
    {
        public Error(IEnumerable<string> messages)
            : this(messages.ToArray())
        {
        }

        public Error(params string[] messages)
        {
            Messages = messages;
            Date = DateTime.Now;
        }

        public IReadOnlyList<string> Messages { get; }

        public DateTime Date { get; }

        public static implicit operator Error(string message) =>
            new Error(message);

        public static implicit operator Error(string[] messages) =>
            new Error(messages);

        public static implicit operator Error(Error[] errors) =>
            errors.Aggregate(MergeErrors);

        public static Error MergeErrors(Error first, Error second) =>
            new Error(first.Messages.Concat(second.Messages));
    }
}
