using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace FileHelpers.Dynamic
{
    /// <summary>
    /// Exception with error information of the run time compilation.
    /// </summary>
    [Serializable]
    public sealed class DynamicCompilationException : FileHelpersException
    {
        /// <summary>
        /// Compilation exception happen loading a dynamic class
        /// </summary>
        /// <param name="message">Message for the error</param>
        /// <param name="sourceCode">Source code reference???</param>
        /// <param name="errors">Errors from compiler</param>
        internal DynamicCompilationException(string message, string sourceCode, IEnumerable<Diagnostic> errors)
            : base(message)
        {
            mSourceCode = sourceCode;
            mCompilerErrors = errors;
        }

        private readonly string mSourceCode;

        /// <summary>
        /// The source code that generates the Exception
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