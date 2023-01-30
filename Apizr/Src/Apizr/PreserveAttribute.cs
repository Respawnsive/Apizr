using System;
using System.ComponentModel;

namespace Apizr
{
    /// <inheritdoc />
    [AttributeUsage(AttributeTargets.All)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public sealed class PreserveAttribute : Attribute
    {
        public bool AllMembers;
        public bool Conditional;

        /// <inheritdoc />
        public PreserveAttribute(bool allMembers, bool conditional)
        {
            AllMembers = allMembers;
            Conditional = conditional;
        }

        /// <inheritdoc />
        public PreserveAttribute() : this(true, false)
        {
        }
    }
}
