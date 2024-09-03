using System;
using System.ComponentModel;

namespace FileHelpers
{
    /// <summary>
    /// Indicates the value to assign to the field in the case of a NULL value.
    /// A default value if none supplied in the field itself.
    /// </summary>
    /// <remarks>
    /// You must specify a string and a converter that can be converted to the
    /// type or an object of the correct type to be directly assigned.
    /// <para/>
    /// See the <a href="http://www.filehelpers.net/mustread">complete attributes list</a> for more
    /// information and examples of each one.
    /// </remarks>

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public sealed class FieldNullValueAttribute : Attribute
    {
        /// <summary>Default value for a null value.</summary>
        public object NullValue { get; private set; }

        /// <summary>
        /// Defines the default in event of a null value.
        /// Object must be of the correct type
        /// </summary>
        /// <param name="nullValue">The value to assign the case of a NULL value.</param>
        public FieldNullValueAttribute(object nullValue)
        {
            NullValue = nullValue;
        }

        /// <summary>Indicates a type and a string to be converted to that type.</summary>
        /// <param name="type">The type of the null value.</param>
        /// <param name="nullValue">The string to be converted to the specified type.</param>
        public FieldNullValueAttribute(Type type, string nullValue)
            : this(TypeDescriptor.GetConverter(type).ConvertFromString(nullValue)) {}
    }
}