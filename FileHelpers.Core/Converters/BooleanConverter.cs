using System;

namespace FileHelpers.Core.Converters
{
    /// <summary>
    /// Convert an input value to a boolean,  allows for true false values
    /// </summary>
    internal sealed class BooleanConverter : ConverterBase
    {
        private readonly string mTrueString;
        private readonly string mFalseString;
        private readonly string mTrueStringLower;
        private readonly string mFalseStringLower;

        /// <summary>
        /// Simple boolean converter
        /// </summary>
        public BooleanConverter() { }

        /// <summary>
        /// Boolean converter with true false values
        /// </summary>
        /// <param name="trueStr">True string</param>
        /// <param name="falseStr">False string</param>
        public BooleanConverter(string trueStr, string falseStr)
        {
            mTrueString = trueStr;
            mFalseString = falseStr;
            mTrueStringLower = trueStr.ToLower();
            mFalseStringLower = falseStr.ToLower();
        }

        /// <summary>
        /// convert a string to a boolean value
        /// </summary>
        /// <param name="from">string to convert</param>
        /// <returns>boolean value</returns>
        public override object StringToField(string from)
        {
            object val;
            string testTo = from.ToLower();

            if (mTrueString == null)
            {
                testTo = testTo.Trim();
                val = testTo switch
                {
                    "true" or "1" or "y" or "t" => true,
                    "false" or "0" or "n" or "f" or "" => (object)false,
                    _ => throw new ConvertException(from,
                                                typeof(bool),
                                                "The string: " + from
                                                               + " can't be recognized as boolean using default true/false values."),
                };
            }
            else
            {
                //  Most of the time the strings should match exactly.  To improve performance
                //  we skip the trim if the exact match is true
                if (testTo == mTrueStringLower)
                    val = true;
                else if (testTo == mFalseStringLower)
                    val = false;
                else
                {
                    testTo = testTo.Trim();
                    if (testTo == mTrueStringLower)
                        val = true;
                    else if (testTo == mFalseStringLower)
                        val = false;
                    else
                    {
                        throw new ConvertException(from,
                            typeof(bool),
                            "The string: " + from
                                           + " can't be recognized as boolean using the true/false values: " + mTrueString + "/" +
                                           mFalseString);
                    }
                }
            }

            return val;
        }

        /// <summary>
        /// Convert to a true false string
        /// </summary>
        public override string FieldToString(object from)
        {
            bool b = Convert.ToBoolean(from);
            if (b)
            {
                if (mTrueString == null)
                    return "True";
                else
                    return mTrueString;
            }
            else if (mFalseString == null)
                return "False";
            else
                return mFalseString;
        }
    }
}