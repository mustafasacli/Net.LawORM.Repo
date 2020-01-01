using System;

namespace Net.LawORM.Entity.Base
{
    internal interface IBaseBO
    {
        /// <summary>
        /// Returns Table Name Of IBaseBO object.
        /// </summary>
        /// <returns>Returns Table Name Of IBaseBO object.</returns>
        String GetTableName();

        /// <summary>
        ///  Returns Identity Name Of IBaseBO object.
        /// </summary>
        /// <returns>Returns Identity Column Name Of IBaseBO object.</returns>
        String GetIdColumn();

    }
}