using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

namespace FileHelpers.Core.Core
{
    internal static class ComparerCache
    {
        private static CultureInfo mCulture;

        /// <summary>
        /// Create an invariant culture comparison operator
        /// </summary>
        /// <returns>Comparison operations</returns>
        internal static CompareInfo CreateComparer()
        {
            mCulture ??= CultureInfo.InvariantCulture; // new CultureInfo("en-us");

            return mCulture.CompareInfo;
        }
    }
}