using System;
using System.Text;

namespace FileHelpers.Core.Dynamic
{
    /// <summary>
    /// Create attributes in the specified language and return as text to paste
    /// into code.
    /// </summary>
    internal sealed class AttributesBuilder
    {
        /// <summary>
        /// Attribute text is created here.
        /// </summary>
        private readonly StringBuilder mSb = new(250);

        /// <summary>
        /// C# or Visual Basic.
        /// </summary>
        private readonly NetLanguage mLang;

        /// <summary>
        /// Create an attribute in the language selected.
        /// </summary>
        /// <param name="lang">The programming language (C# or VB).</param>
        public AttributesBuilder(NetLanguage lang)
        {
            mLang = lang;
        }

        /// <summary>
        /// Do we have any attributes? Do we have to start and close VB attributes?
        /// </summary>
        private bool mFirst = true;

        /// <summary>
        /// Add an attribute. C#: [att1] [att2], VB: &lt;att1, att2&gt;.
        /// </summary>
        /// <param name="attribute">The attribute to add.</param>
        public void AddAttribute(string attribute)
        {
            if (string.IsNullOrEmpty(attribute))
                return;

            string newLine = Environment.NewLine;
            if (mFirst)
            {
                switch (mLang)
                {
                    case NetLanguage.CSharp:
                        mSb.Append('[');
                        break;
                    case NetLanguage.VbNet:
                        mSb.Append('<');
                        break;
                }
                mFirst = false;
            }
            else
            {
                switch (mLang)
                {
                    case NetLanguage.VbNet:
                        mSb.Append(", _"); // New line continuation
                        mSb.Append(newLine);
                        mSb.Append(' ');
                        break;
                    case NetLanguage.CSharp:
                        mSb.Append('[');
                        break;
                }
            }

            mSb.Append(attribute);

            switch (mLang)
            {
                case NetLanguage.CSharp:
                    mSb.Append(']');
                    mSb.Append(newLine);
                    break;
                case NetLanguage.VbNet:
                    break;
            }
        }

        /// <summary>
        /// Return all attributes as text. If none were added, returns empty string.
        /// </summary>
        /// <returns>The generated attributes as code.</returns>
        public string GetAttributesCode()
        {
            if (mFirst == true)
                return string.Empty;

            switch (mLang)
            {
                case NetLanguage.VbNet:
                    mSb.Append("> _");
                    mSb.Append(Environment.NewLine);
                    break;
            }

            return mSb.ToString();
        }
    }
}
