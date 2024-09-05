using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace FileHelpers.Core.Dynamic
{
    /// <summary>
    /// Exception with error information of the run time compilation.
    /// </summary>
    [Serializable]
    public sealed class DynamicCompilationException : FileHelpersException
    {
        /// <summary>
        /// Compilation exception happened while loading a dynamic class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="sourceCode">The source code that caused the error.</param>
        /// <param name="errors">The errors returned from the compiler.</param>
        internal DynamicCompilationException(string message, string sourceCode, IEnumerable<Diagnostic> errors)
            : base(message)
        {
            mSourceCode = sourceCode;
            mCompilerErrors = errors;
        }

        private readonly string mSourceCode;

        /// <summary>
        /// The source code that generated the exception.
        /// </summary>
        public string SourceCode
        {
            get { return mSourceCode; }
        }

        private readonly IEnumerable<Diagnostic> mCompilerErrors;

        /// <summary>
        /// The errors returned from the compiler.
        /// </summary>
        public IEnumerable<Diagnostic> CompilerErrors
        {
            get { return mCompilerErrors; }
        }
    }
}
