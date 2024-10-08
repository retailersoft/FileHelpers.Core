﻿namespace FileHelpers.Core.Enums
{
    /// <summary>
    /// Indicates the behavior when a data longer than a [<see cref="FixedLengthRecordAttribute"/>]
    /// Length is written to this field
    /// </summary>
    public enum OverflowMode
    {
        /// <summary>
        /// Discard overflowing characters at the end
        /// </summary>
        DiscardEnd,
        /// <summary>
        /// Throw an exception
        /// </summary>
        Error
    }
}
