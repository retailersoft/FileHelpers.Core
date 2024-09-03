
using System;
using System.Data;
using Microsoft.Data.SqlClient;  // Updated namespace for .NET Core and later versions

namespace FileHelpers.DataLink
{
    /// <summary>
    /// This is a base class that implements the <see cref="DataStorage"/> for
    /// Microsoft SQL Server.
    /// </summary>
    [Obsolete("Datalink feature is outdated and will be rewritten, see https://www.filehelpers.net/mustread/")]
    public sealed class SqlServerStorage : DatabaseStorage
    {
        #region Constructors

        /// <summary>
        /// Create a new instance of the SqlServerStorage based on the record
        /// type provided.
        /// </summary>
        /// <param name="recordType">The type of the record class.</param>
        public SqlServerStorage(Type recordType)
            : this(recordType, string.Empty) { }

        /// <summary>
        /// Create a new instance of the SqlServerStorage based on the record
        /// type provided.
        /// </summary>
        /// <param name="recordType">The type of the record class.</param>
        /// <param name="connectionStr">
        /// The full connection string used to connect to the SQL server.
        /// </param>
        public SqlServerStorage(Type recordType, string connectionStr)
            : base(recordType)
        {
            ConnectionString = connectionStr;
        }

        /// <summary>
        /// Create a new instance of the SqlServerStorage based on the record
        /// type provided (uses Windows auth).
        /// </summary>
        /// <param name="recordType">The type of the record class.</param>
        /// <param name="server">The server name or IP of the SQL Server.</param>
        /// <param name="database">The database name on the server.</param>
        public SqlServerStorage(Type recordType, string server, string database)
            : this(recordType, server, database, string.Empty, string.Empty) { }

        /// <summary>
        /// Create a new instance of the SqlServerStorage based on the record
        /// type provided (uses SQL Server auth).
        /// </summary>
        /// <param name="recordType">The type of the record class.</param>
        /// <param name="server">The server name or IP of the SQL Server.</param>
        /// <param name="database">The database name on the server.</param>
        /// <param name="user">The SQL username to log in to the server.</param>
        /// <param name="pass">The password of the SQL username to log in to the server.</param>
        public SqlServerStorage(Type recordType, string server, string database, string user, string pass)
            : this(recordType, DataBaseHelper.SqlConnectionString(server, database, user, pass))
        {
            ServerName = server;
            DatabaseName = database;
            UserName = user;
            UserPass = pass;
        }

        #endregion

        #region Create Connection and Command

        /// <summary>Create a connection object to the database.</summary>
        /// <returns>SQL Server connection object.</returns>
        protected override sealed IDbConnection CreateConnection()
        {
            string conString = ConnectionString;

            if (string.IsNullOrEmpty(conString))
            {
                if (string.IsNullOrEmpty(ServerName))
                    throw new BadUsageException("The ServerName can't be null or empty.");

                if (string.IsNullOrEmpty(DatabaseName))
                    throw new BadUsageException("The DatabaseName can't be null or empty.");

                conString = DataBaseHelper.SqlConnectionString(ServerName, DatabaseName, UserName, UserPass);
            }

            return new SqlConnection(conString);
        }

        #endregion

        #region Properties

        private string _serverName = string.Empty;

        /// <summary> The server name or IP of the SQL Server. </summary>
        public string ServerName
        {
            get => _serverName;
            set
            {
                _serverName = value;
                ConnectionString = DataBaseHelper.SqlConnectionString(ServerName, DatabaseName, UserName, UserPass);
            }
        }

        private string _databaseName = string.Empty;

        /// <summary> The name of the database. </summary> 
        public string DatabaseName
        {
            get => _databaseName;
            set
            {
                _databaseName = value;
                ConnectionString = DataBaseHelper.SqlConnectionString(ServerName, DatabaseName, UserName, UserPass);
            }
        }

        private string _userName = string.Empty;

        /// <summary> The user name used to log in to the SQL Server (leave empty for Windows Auth).</summary>
        public string UserName
        {
            get => _userName;
            set
            {
                _userName = value;
                ConnectionString = DataBaseHelper.SqlConnectionString(ServerName, DatabaseName, UserName, UserPass);
            }
        }

        private string _userPass = string.Empty;

        /// <summary> The user password used to log in to the SQL Server (leave empty for Windows Auth).</summary>
        public string UserPass
        {
            get => _userPass;
            set
            {
                _userPass = value;
                ConnectionString = DataBaseHelper.SqlConnectionString(ServerName, DatabaseName, UserName, UserPass);
            }
        }

        #endregion

        #region ExecuteInBatch

        /// <summary>Determines if operations are executed in batches.</summary>
        protected override bool ExecuteInBatch => true;

        #endregion
    }
}
