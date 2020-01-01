using Net.LawORM.Base;
using System;

namespace Net.LawORM.Entity.QueryBuilding
{
    internal interface IQueryBuilder
    {
        /// <summary>
        /// Gets Property contains parameter(s).
        /// </summary>
        Property Properties { get; }

        /// <summary>
        /// Gets Sql Query.
        /// </summary>
        String QueryString { get; }
    }
}