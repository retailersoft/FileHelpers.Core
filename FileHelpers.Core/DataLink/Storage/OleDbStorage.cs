using System;
using System.Data;

namespace FileHelpers.Core.DataLink
{
	using System.Data.OleDb;

    /// <summary>
    /// This is a base class that implements the <see cref="DataStorage"/> for
    /// Microsoft Access Files.
    /// </summary>
    [Obsolete("Datalink feature is outdated and will be rewritten, see https://www.filehelpers.net/mustread/")]
    public sealed class OleDbStorage : DatabaseStorage
    {
	#region "  Constructors  "

        /// <summary>
        /// Create a new OleDbStorage based in the record type and in the connection string.
        /// </summary>
        /// <param name="recordType">The Type of the records.</param>
        /// <param name="oleDbConnString">The connection string used to create the OleDbConnection.</param>
        public OleDbStorage(Type recordType, string oleDbConnString)
            : base(recordType)
        {
            ConnectionString = oleDbConnString;
        }

	#endregion

	#region "  Create Connection and Command  "

        /// <summary>Must create an abstract connection object.</summary>
        /// <returns>An Abstract Connection Object.</returns>
        protected override sealed IDbConnection CreateConnection()
        {
            if (ConnectionString == null ||
                ConnectionString == string.Empty)
                throw new BadUsageException("The OleDb Connection string can't be null or empty.");
            return new OleDbConnection(ConnectionString);
        }

	#endregion
    }
}
