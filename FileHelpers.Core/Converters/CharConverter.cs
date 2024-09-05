using System;

namespace FileHelpers.Core.Converters
{
    /// <summary>
    /// Allow characters to be converted to upper and lower case automatically.
    /// </summary>
    internal sealed class CharConverter : ConverterBase
    {
        /// <summary>
        /// whether we upper or lower case the character on input
        /// </summary>
        private enum CharFormat
        {
            /// <summary>
            /// Don't change the case
            /// </summary>
            NoChange = 0,

            /// <summary>
            /// Change to lower case
            /// </summary>
            Lower,

            /// <summary>
            /// change to upper case
            /// </summary>
            Upper,
        }

        /// <summary>
        /// default to not upper or lower case
        /// </summary>
        private readonly CharFormat mFormat = CharFormat.NoChange;

        /// <summary>
        /// Create a single character converter that does not upper or lower case result
        /// </summary>
        public CharConverter()
            : this("") // default,  no upper or lower case conversion
        { }

        /// <summary>
        /// Single character converter that optionally makes it upper (X) or lower case (x)
        /// </summary>
        /// <param name="format"> empty string for no upper or lower,  x for lower case,  X for Upper case</param>
        public CharConverter(string format)
        {
            mFormat = format.Trim() switch
            {
                "x" or "lower" => CharFormat.Lower,
                "X" or "upper" => CharFormat.Upper,
                "" => CharFormat.NoChange,
                _ => throw new BadUsageException(
                                        "The format of the Char Converter must be \"\", \"x\" or \"lower\" for lower case, \"X\" or \"upper\" for upper case"),
            };
        }

        /// <summary>
        /// Extract the first character with optional upper or lower case
        /// </summary>
        /// <param name="from">String contents</param>
        /// <returns>Character (may be upper or lower case)</returns>
        public override object StringToField(string from)
        {
            if (string.IsNullOrEmpty(from))
                return char.MinValue;

            try
            {
                return mFormat switch
                {
                    CharFormat.NoChange => from[0],
                    CharFormat.Lower => char.ToLower(from[0]),
                    CharFormat.Upper => (object)char.ToUpper(from[0]),
                    _ => throw new ConvertException(from,
                                                typeof(char),
                                                "Unknown char convert flag " + mFormat.ToString()),
                };
            }
            catch
            {
                throw new ConvertException(from, typeof(char), "Upper or lower case of input string failed");
            }
        }

        /// <summary>
        /// Convert from a character to a string for output
        /// </summary>
        /// <param name="from">Character to convert from</param>
        /// <returns>String containing the character</returns>
        public override string FieldToString(object from)
        {
            return mFormat switch
            {
                CharFormat.NoChange => Convert.ToChar(from).ToString(),
                CharFormat.Lower => char.ToLower(Convert.ToChar(from)).ToString(),
                CharFormat.Upper => char.ToUpper(Convert.ToChar(from)).ToString(),
                _ => throw new ConvertException("", typeof(char), "Unknown char convert flag " + mFormat.ToString()),
            };
        }
    }
}