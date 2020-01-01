namespace Net.LawORM.Logic.BaseDal
{
    using Net.LawORM.Base;
    using System;
    using System.Data;

    internal interface IConnectionOperations
    {
        DataSet GetResultSetOfQuery(String query);

        DataSet GetResultSetOfQuery(String query, Property parameters);

        DataSet GetResultSetOfProcedure(String procedure);

        DataSet GetResultSetOfProcedure(String procedure, Property parameters);

        /* -------------------------------------------------------------- */

        Int32 ExecuteQuery(String query);

        Int32 ExecuteQuery(String query, Property parameters);

        Int32 ExecuteProcedure(String procedure);

        Int32 ExecuteProcedure(String procedure, Property parameters);

        /* -------------------------------------------------------------- */

        Object ExecuteScalarAsQuery(String query);

        Object ExecuteScalarAsQuery(String query, Property parameters);

        Object ExecuteScalarAsProcedure(String procedure);

        Object ExecuteScalarAsProcedure(String procedure, Property parameters);
    }
}