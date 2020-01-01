using Net.LawORM.Base;
using System;

namespace Net.LawORM.Data.Client
{
    public sealed class ConnectionFactory
    {

        #region [ CreateConnection Method ]

        /// <summary>
        /// Creates IConnection object with given parameters.
        /// </summary>
        /// <param name="connectionString">Connection String</param>
        /// <returns>Returns IConnection object.</returns>
        public static IConnection CreateConnection(ConnectionTypes connType)
        {
            IConnection conn = null;
            try
            {
                switch (connType)
                {
                    case ConnectionTypes.MariaDb:
                        conn = new ConnectionMariaDb();
                        break;

                    case ConnectionTypes.MySQL:
                        conn = new ConnectionMySQL();
                        break;

                    case ConnectionTypes.Oledb:
                        conn = new ConnectionOledb();
                        break;

                    case ConnectionTypes.Odbc:
                        conn = new ConnectionOdbc();
                        break;

                    case ConnectionTypes.SqlServer:
                        conn = new ConnectionSqlServer();
                        break;

                    default:
                        break;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return conn;
        }

        #endregion


        #region [ CreateConnection Method ]

        /// <summary>
        /// Creates IConnection object with given parameters.
        /// </summary>
        /// <param name="connType">Connection Type</param>
        /// <param name="connectionString">Connection String</param>
        /// <returns>Returns IConnection object.</returns>
        public static IConnection CreateConnection(ConnectionTypes connType, string connectionString)
        {
            IConnection conn = null;
            try
            {
                switch (connType)
                {
                    case ConnectionTypes.MariaDb:
                        conn = new ConnectionMariaDb(connectionString);
                        break;

                    case ConnectionTypes.MySQL:
                        conn = new ConnectionMySQL(connectionString);
                        break;

                    case ConnectionTypes.Oledb:
                        conn = new ConnectionOledb(connectionString);
                        break;

                    case ConnectionTypes.Odbc:
                        conn = new ConnectionOdbc(connectionString);
                        break;

                    case ConnectionTypes.SqlServer:
                        conn = new ConnectionSqlServer(connectionString);
                        break;
                        
                    default:
                        break;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return conn;
        }

        #endregion

    }
}