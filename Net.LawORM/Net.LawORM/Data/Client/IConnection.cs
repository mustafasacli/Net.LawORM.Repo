using Net.LawORM.Base;
using System;
using System.Data;
using System.Data.Common;

namespace Net.LawORM.Data.Client
{
    public interface IConnection : IDisposable
    {

        /// <summary>
        /// Gets Connection Type Of IConnection.
        /// </summary>
        ConnectionTypes ConnectionType { get; }


        /// <summary>
        /// Opens Database connection.
        /// </summary>
        void Open();


        /// <summary>
        /// Closes Database connection.
        /// </summary>
        void Close();


        /// <summary>
        /// Gets Connection State of database Connection.
        /// </summary>
        ConnectionState ConnectionState { get; }


        /// <summary>
        /// Creates Transaction Object.
        /// </summary>
        void CreateTransaction();


        /// <summary>
        /// Creates Transaction Object.
        /// </summary>
        void CreateTransaction(IsolationLevel isolationLevel);


        /// <summary>
        /// Gets Transaction object.
        /// </summary>
        DbTransaction Transaction { get; }


        /// <summary>
        /// Commits Transaction.
        /// </summary>
        void CommitTransaction();


        /// <summary>
        /// Rollbacks Transaction.
        /// </summary>
        void RollbackTransaction();


        /// <summary>
        /// Creates DbDataAdapter object.
        /// </summary>
        /// <returns>Returns System.Data.Common.DbDataAdapter object.</returns>
        DbDataAdapter CreateAdapter();


        /// <summary>
        /// Creates DbCommand object.
        /// </summary>
        /// <returns>Returns System.Data.Common.DbCommand object.</returns>
        DbCommand CreateCommand();


        /// <summary>
        /// Creates DbConnectionStringBuilder object.
        /// </summary>
        /// <returns>Returns System.Data.Common.DbConnectionStringBuilder object.</returns>        
        DbConnectionStringBuilder ConnectionStringBuilder { get; }

        /// <summary>
        /// Gets, Sets Connection String Of IConnection
        /// </summary>
        String ConnectionString { get; set; }

        /// <summary>
        /// Gets Database name.
        /// </summary>
        String Database { get; }

    }
}
