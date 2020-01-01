using System;

namespace Net.LawORM.Base
{
    public enum ConnectionTypes : byte
    {
        MariaDb,
        MySQL,
        Oledb,
        Odbc,
        SqlServer,
    };

    #region [ ConnectionTypeBuilder class ]

    public static class ConnectionTypeBuilder
    {
        public static ConnectionTypes GetConnectionType(String driverTypeName)
        {
            try
            {
                driverTypeName = driverTypeName.TrimEnd().TrimStart().ToLower();
                switch (driverTypeName)
                {
                    case "sqlexpress":
                    case "sqlserver":
                    default:
                        return ConnectionTypes.SqlServer;

                    case "mysql":
                        return ConnectionTypes.MySQL;

                    case "mariadb":
                    case "marıadb":
                        return ConnectionTypes.MariaDb;

                    case "oledb":
                        return ConnectionTypes.Oledb;

                    case "odbc":
                        return ConnectionTypes.Odbc;
                }
            }
            catch (Exception)
            {
                return ConnectionTypes.SqlServer;
            }
        }
    }

    #endregion
}