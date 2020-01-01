namespace Net.LawORM.Logic.BaseDal
{
    using Net.LawORM.Base;
    using Net.LawORM.Entity.Base;
    using Net.LawORM.Entity.QueryBuilding;
    using System;

    internal interface IBaseDL : IConnectionOperations, IDisposable
    {

        /// <summary>
        /// Inserts BaseBO object and returns execution value.
        /// </summary>
        /// <param name="baseBO">BaseBO object.</param>
        /// <returns>Returns Execution value of Insert query.</returns>
        Int32 Insert(BaseBO baseBO);

        /// <summary>
        /// Inserts BaseBO object and returns Identity value.
        /// </summary>
        /// <param name="baseBO">BaseBO object.</param>
        /// <returns>Returns Identity value</returns>
        Int32 InsertAndGetId(BaseBO baseBO);

        /// <summary>
        /// Updates BaseBO object and returns execution value of delete query.
        /// </summary>
        /// <param name="baseBO">BaseBO object.</param>
        /// <returns>returns execution value of delete query.</returns>
        Int32 Delete(BaseBO baseBO);

        /// <summary>
        /// Updates BaseBO object and returns execution value of update query.
        /// </summary>
        /// <param name="baseBO">BaseBO object.</param>
        /// <returns>returns execution value of update query.</returns>
        Int32 Update(BaseBO baseBO);


        /// <summary>
        /// Returns QueryBuilder object with with given parameters.
        /// </summary>
        /// <param name="queryType"> Query type for QueryBuilder </param>
        /// <param name="tableObject"> BaseBO object </param>
        /// <returns> QueryBuilder object</returns>
        QueryBuilder CreateQueryBuilder(QueryTypes queryType, BaseBO tableObject);

        /// <summary>
        /// Returns Driver Type of Data Layer.
        /// </summary>
        ConnectionTypes DriverType { get; }
    }
}