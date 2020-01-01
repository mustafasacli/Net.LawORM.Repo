namespace Net.LawORM.Logic.BaseDal
{
    using Net.LawORM.Base;
    using Net.LawORM.Data.Client;
    using Net.LawORM.Entity.Base;
    using Net.LawORM.Entity.QueryBuilding;
    using Net.LawORM.Logic.Extensions;
    using Net.LawORM.Logic.Util;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Reflection;

    public abstract class BaseDL : IBaseDL
    {
        #region [ private fields ]

        private IConnection Conn = null;
        private String _ConnStr;
        private PropertyUtil propUtil;
        private ConnectionTypes _driverType;
        private String _connName = "main";
        private QueryBuilder queryBuilder = null;

        #endregion


        #region [ BaseDL ctors ]

        /// <summary>
        /// protected ctor of BaseDL.
        /// </summary>
        protected BaseDL()
            : this("main")
        {
        }

        /// <summary>
        /// protected ctor of BaseDL.
        /// </summary>
        /// <param name="connectionName">connection property name</param>
        protected BaseDL(string connectionName)
        {
            _connName = connectionName;
            propUtil = ConfUtil.BuildPropertyNew(ConnectionName);
            _driverType = propUtil.DriverType;
            _ConnStr = propUtil.PropertyString;
            Conn = ConnectionFactory.CreateConnection(_driverType, _ConnStr);
        }

        #endregion


        #region [ ConnectionName Property ]

        /// <summary>
        /// Gets ConnectionName.
        /// </summary>
        public virtual String ConnectionName
        {
            get { return _connName; }
        }

        #endregion


        #region [ Insert method ]

        /// <summary>
        /// Gets exceution result of Insert Operation.
        /// </summary>
        /// <param name="baseBO">BaseBO object.</param>
        /// <returns>returns Int32</returns>
        public Int32 Insert(BaseBO baseBO)
        {
            try
            {
                queryBuilder =
                     CreateQueryBuilder(QueryTypes.Insert, baseBO);

                return ExecuteQuery(queryBuilder.QueryString, queryBuilder.Properties);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                queryBuilder = null;
            }
        }

        #endregion


        #region [ Insert And Get Id method ]

        /// <summary>
        /// Inserts and Gets Id of BaseBO object.
        /// </summary>
        /// <param name="baseBO">BaseBO object.</param>
        /// <returns>returns Int32</returns>
        public Int32 InsertAndGetId(BaseBO baseBO)
        {
            try
            {
                queryBuilder =
                     CreateQueryBuilder(QueryTypes.InsertAndGetId, baseBO);

                Object obj = ExecuteScalarAsQuery(queryBuilder.QueryString, queryBuilder.Properties);

                return obj.ToInt();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                queryBuilder = null;
            }
        }

        #endregion


        #region [ Update method ]

        /// <summary>
        /// Gets exceution result of Update Operation.
        /// </summary>
        /// <param name="baseBO">BaseBO object.</param>
        /// <returns>returns Int32</returns>
        public Int32 Update(BaseBO baseBO)
        {
            try
            {
                queryBuilder =
                    CreateQueryBuilder(QueryTypes.Update, baseBO);

                return ExecuteQuery(queryBuilder.QueryString, queryBuilder.Properties);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                queryBuilder = null;
            }
        }

        #endregion


        #region [ Delete method ]

        /// <summary>
        /// Gets exceution result of Delete Operation.
        /// </summary>
        /// <param name="baseBO">BaseBO object.</param>
        /// <returns>returns Int32</returns>
        public Int32 Delete(BaseBO baseBO)
        {
            try
            {
                queryBuilder = null;
                queryBuilder =
                    CreateQueryBuilder(QueryTypes.Delete, baseBO);

                return ExecuteQuery(queryBuilder.QueryString, queryBuilder.Properties);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        /* ------------------------------------------- */

        #region [ Get Table method ]

        /// <summary>
        /// Gets All Table Records of BaseBO object.
        /// </summary>
        /// <param name="baseBO">BaseBO object.</param>
        /// <returns>Returns a System.Data.DataTable</returns>
        public DataTable GetTable(BaseBO baseBO)
        {
            try
            {
                queryBuilder = CreateQueryBuilder(QueryTypes.Select, baseBO);

                return GetResultSetOfQuery(queryBuilder.QueryString).Tables[0];
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                queryBuilder = null;
            }
        }

        #endregion


        #region [ GetById method ]

        /// <summary>
        /// Gets One Record of BaseBO object if any object has BaseBO Id.
        /// </summary>
        /// <param name="baseBO">BaseBO object.</param>
        /// <returns>Returns a System.Data.DataTable</returns>
        public DataTable GetById(BaseBO baseBO)
        {
            try
            {
                queryBuilder =
                    CreateQueryBuilder(QueryTypes.SelectWhereId, baseBO);

                DataTable dT = GetResultSetOfQuery(
                    queryBuilder.QueryString,
                    queryBuilder.Properties).Tables[0];

                return dT;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                queryBuilder = null;
            }
        }

        #endregion


        #region [ GetObjectListByIds method ]

        public List<T> GetObjectListByIds<T>(List<int> idList) where T : new()
        {
            try
            {
                List<T> t_List = null;

                if (idList == null)
                    return t_List;

                t_List = new List<T>();
                if (idList.Count == 0)
                    return t_List;

                T t = (T)Activator.CreateInstance(typeof(T));

                string IdCol = string.Format("{0}", t.GetType().GetMethod("GetIdColumn").Invoke(t, null));
                if (IdCol.Trim().Length == 0)
                    throw new Exception("Id Column can not be empty.");

                string tableName = string.Format("{0}", t.GetType().GetMethod("GetTableName").Invoke(t, null));

                if (tableName.Replace(" ", "").Length == 0)
                    throw new Exception("TableName can not be empty.");

                string strIn = string.Empty;

                foreach (var item in idList)
                {
                    strIn = string.Format("{0}, {1}", strIn, item);
                }
                strIn = strIn.TrimStart(',').TrimStart();

                DataTable dtBO = GetResultSetOfQuery(string.Format("Select * From {0} Where {1} IN ({2})", tableName, IdCol, strIn)).Tables[0];

                t_List = dtBO.ToList<T>(true);

                return t_List;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion


        #region [ GetObjectById method ]

        public T GetObjectById<T>(int Id) where T : new()
        {
            try
            {
                T t = (T)Activator.CreateInstance(typeof(T));

                string IdCol = string.Format("{0}", t.GetType().GetMethod("GetIdColumn").Invoke(t, null));
                if (IdCol.Trim().Length == 0)
                    throw new Exception("Id Column can not be empty.");

                string tableName = string.Format("{0}", t.GetType().GetMethod("GetTableName").Invoke(t, null));
                if (tableName.Trim().Length == 0)
                    throw new Exception("TableName can not be empty.");

                DataTable dtBO = GetResultSetOfQuery(string.Format("Select * From {0} Where {1}={2}", tableName, IdCol, Id)).Tables[0];

                List<T> listT = dtBO.ToList<T>(true);

                return listT[0];
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion


        #region [ GetSpecialColumnsWithBO method ]

        public DataTable GetSpecialColumnsWithBO(string[] columnList, BaseBO baseBO)
        {
            try
            {
                if (columnList.IsNull())
                    throw new Exception("Column List can not be null.");

                if (columnList.Length == 0)
                    throw new Exception("Column List Length can not be Zero.");

                queryBuilder = CreateQueryBuilder(QueryTypes.SelectWhereChangeColumns, baseBO);
                string tmpQ = queryBuilder.QueryString;
                int _index = tmpQ.FirstIndexOf('*');
                string cols = string.Empty;
                foreach (var column in columnList)
                {
                    cols = string.Format("{0}, {1}", cols, column);
                }
                cols = cols.TrimStart(',').TrimStart();
                string query = string.Format("{0} {1} {2}", tmpQ.Substring(0, _index), cols, tmpQ.Substring(_index + 1, tmpQ.Length - _index - 1));
                return GetResultSetOfQuery(query, queryBuilder.Properties).Tables[0];
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                queryBuilder = null;
            }
        }

        #endregion


        #region [ GetChangeColumnList method ]

        /// <summary>
        /// Gets Columns which baseBo object changelist contains.
        /// Example: Column Change List Of BaseBO : Col1, Col2, Col3.
        /// Query = Select Col1, Col2, Col3 From TableOfBaseBO;
        /// </summary>
        /// <param name="baseBO">BaseBO object.</param>
        /// <returns>Returns a System.Data.DataTable</returns>
        public DataTable GetChangeColumnList(BaseBO baseBO)
        {
            try
            {
                queryBuilder = CreateQueryBuilder(QueryTypes.SelectWhereChangeColumns, baseBO);
                return GetResultSetOfQuery(queryBuilder.QueryString, queryBuilder.Properties).Tables[0];
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                queryBuilder = null;
            }
        }

        #endregion


        #region [ Get Where Change Column List nethod ]

        /// <summary>
        /// Gets Columns which baseBo object changelist contains.
        /// Example: Column Change List Of BaseBO : Col1, Col2, Col3.
        /// Query = Select *  From TableOfBaseBO Where Col1=Col1Value And Col2=Col2Value And Col3=Col3Value;
        /// </summary>
        /// <param name="baseBO">BaseBO object.</param>
        /// <returns>Returns a System.Data.DataTable</returns>
        public DataTable GetWhereChangeColumnList(BaseBO baseBO)
        {
            try
            {
                queryBuilder = CreateQueryBuilder(QueryTypes.SelectWhereChangeColumns, baseBO);
                return GetResultSetOfQuery(queryBuilder.QueryString, queryBuilder.Properties).Tables[0];
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                queryBuilder = null;
            }
        }

        #endregion

        /* ------------------------------------------------- */

        #region [ CreateQueryBuilder method. ]

        /// <summary>
        /// Create A QueryBuilder object with ConnectionType Of Connection and given querytype and AbstractBaseBo object.
        /// </summary>
        /// <param name="queryType">QueryType Of QueryBuilder</param>
        /// <param name="tableObject">Table class object.</param>
        /// <returns>Returns A QueryBuilder object with ConnectionType Of Connection and given querytype and AbstractBaseBo object.</returns>
        public QueryBuilder CreateQueryBuilder(QueryTypes queryType, BaseBO tableObject)
        {
            return new QueryBuilder(_driverType, queryType, tableObject);
        }

        #endregion

        /* ------------------------------------------------- */

        #region [ GetResultSet method ]

        internal DataSet GetResultSet(string queryOrProc, CommandType cmdType, Property property)
        {
            DataSet ds = new DataSet();
            try
            {
                Conn.Open();
                Conn.CreateTransaction();
                using (DbCommand Cmd = Conn.CreateCommand())
                {
                    Cmd.Transaction = Conn.Transaction;
                    Cmd.CommandText = queryOrProc;
                    Cmd.CommandType = cmdType;

                    if (null != property)
                    {
                        DbParameter dbParam;
                        foreach (FreeParameter prm in property.GetParameters())
                        {
                            dbParam = null;
                            dbParam = Cmd.CreateParameter();

                            dbParam.ParameterName = prm.Name;
                            dbParam.Value = prm.Value;
                            Cmd.Parameters.Add(dbParam);
                        }
                    }

                    using (DbDataAdapter adapter = Conn.CreateAdapter())
                    {
                        adapter.SelectCommand = Cmd;
                        adapter.Fill(ds);
                    }
                }
                Conn.CommitTransaction();
            }
            catch (Exception)
            {
                Conn.RollbackTransaction();
                throw;
            }
            finally
            {
                Conn.Close();
            }
            return ds;
        }

        #endregion

        /* ------------------------------------------------- */

        #region [ Get ResultSet of Query ]

        /// <summary>
        /// Returns A DataSet with given Query without any parameter
        /// </summary>
        /// <param name="query">Sql Query</param>
        /// <returns>Returns A DataSet with given Query without any parameter</returns>
        public DataSet GetResultSetOfQuery(String query)
        {
            DataSet ds = new DataSet();
            try
            {
                ds = GetResultSet(query, CommandType.Text, null);
            }
            catch (Exception)
            {
                throw;
            }
            return ds;
        }

        #endregion

        #region [ Get ResultSet Of Procedure ]

        /// <summary>
        /// Gets ResultSet with given Procedure without any parameter.
        /// </summary>
        /// <param name="query">Sql Procedure</param>
        /// <returns>Returns A DataSet with given Procedure without any parameter</returns>
        public DataSet GetResultSetOfProcedure(String procedure)
        {
            DataSet ds = new DataSet();
            try
            {
                ds = GetResultSet(procedure, CommandType.StoredProcedure, null);
            }
            catch (Exception)
            {
                throw;
            }
            return ds;
        }

        #endregion

        /* ------------------------------------------------- */

        #region [ Get ResultSet of Query with Parameters ]

        /// <summary>
        /// Returns A DataSet with given Query with parameter(s).
        /// </summary>
        /// <param name="query">Sql Query</param>
        /// <param name="parameters">Property which contains parameters</param>
        /// <returns>Returns A DataSet with given Query without any parameter</returns>
        public DataSet GetResultSetOfQuery(String query, Property parameters)
        {
            DataSet ds = new DataSet();
            try
            {
                ds = GetResultSet(query, CommandType.Text, parameters);
            }
            catch (Exception)
            {
                throw;
            }
            return ds;
        }

        #endregion

        #region [ Get ResultSet Of Procedure ]

        /// <summary>
        /// Returns A DataSet with given Procedure without any parameter
        /// </summary>
        /// <param name="query">Sql Procedure</param>
        /// <param name="parameters">Property parameters</param>
        /// <returns>Returns A DataSet with given Procedure without any parameter</returns>
        public DataSet GetResultSetOfProcedure(String procedure, Property parameters)
        {
            DataSet ds = new DataSet();
            try
            {
                ds = GetResultSet(procedure, CommandType.StoredProcedure, parameters);
            }
            catch (Exception)
            {
                throw;
            }
            return ds;
        }

        #endregion

        /* ------------------------------------------------- */

        #region [ Execute method ]

        internal Int32 Execute(string queryOrProc, CommandType cmdType, Property property)
        {
            Int32 retInt = -1;
            try
            {
                Conn.Open();
                Conn.CreateTransaction();
                using (DbCommand Cmd = Conn.CreateCommand())
                {
                    Cmd.Transaction = Conn.Transaction;
                    Cmd.CommandText = queryOrProc;
                    Cmd.CommandType = cmdType;

                    if (null != property)
                    {
                        DbParameter dbParam;
                        foreach (FreeParameter prm in property.GetParameters())
                        {
                            dbParam = null;
                            dbParam = Cmd.CreateParameter();

                            dbParam.ParameterName = prm.Name;
                            dbParam.Value = prm.Value;
                            Cmd.Parameters.Add(dbParam);
                        }
                    }

                    retInt = Cmd.ExecuteNonQuery();

                }

                Conn.CommitTransaction();
            }
            catch (Exception)
            {
                Conn.RollbackTransaction();
                throw;
            }
            finally
            {
                Conn.Close();
            }
            return retInt;
        }

        #endregion

        /* ------------------------------------------------- */

        #region [ Execute Query ]

        /// <summary>
        /// Returns execution value of query.
        /// </summary>
        /// <param name="query">Sql Query</param>
        /// <returns>Returns execution value of query.</returns>
        public Int32 ExecuteQuery(String query)
        {
            Int32 retInt = -1;

            try
            {
                retInt = Execute(query, CommandType.Text, null);
            }
            catch (Exception)
            {
                throw;
            }

            return retInt;
        }

        #endregion

        #region [ Execute Procedure ]

        /// <summary>
        /// Returns execution value of Procedure.
        /// </summary>
        /// <param name="query">Sql Procedure</param>
        /// <returns>Returns execution value of Procedure.</returns>
        public Int32 ExecuteProcedure(String procedure)
        {
            Int32 retInt = -1;

            try
            {
                retInt = Execute(procedure, CommandType.StoredProcedure, null);
            }
            catch (Exception)
            {
                throw;
            }

            return retInt;
        }

        #endregion

        /* ------------------------------------------------- */

        #region [ Execute Query with Parameters ]

        /// <summary>
        /// Returns execution value of query.
        /// </summary>
        /// <param name="query">Sql Query</param>
        /// <param name="parameters">Property contains parameters.</param>
        /// <returns>Returns execution value of query with parameters.</returns>
        public Int32 ExecuteQuery(String query, Property parameters)
        {
            Int32 retInt = -1;

            try
            {
                retInt = Execute(query, CommandType.Text, parameters);
            }
            catch (Exception)
            {
                throw;
            }

            return retInt;
        }

        #endregion

        #region [ Execute Procedure ]

        /// <summary>
        /// Returns execution value of Procedure.
        /// </summary>
        /// <param name="query">Sql Procedure</param>
        /// <param name="parameters">Property contains parameters.</param>
        /// <returns>Returns execution value of Procedure with parameters.</returns>
        public Int32 ExecuteProcedure(String procedure, Property parameters)
        {
            Int32 retInt = -1;

            try
            {
                retInt = Execute(procedure, CommandType.StoredProcedure, parameters);
            }
            catch (Exception)
            {
                throw;
            }

            return retInt;
        }

        #endregion

        /* ------------------------------------------------- */

        #region [ ExecuteScalar method ]

        internal Object ExecuteScalar(string queryOrProc, CommandType cmdType, Property property)
        {
            Object retObj = null;
            try
            {
                Conn.Open();
                Conn.CreateTransaction();
                using (DbCommand Cmd = Conn.CreateCommand())
                {
                    Cmd.Transaction = Conn.Transaction;
                    Cmd.CommandText = queryOrProc;
                    Cmd.CommandType = cmdType;

                    if (null != property)
                    {
                        DbParameter dbParam;
                        foreach (FreeParameter prm in property.GetParameters())
                        {
                            dbParam = null;
                            dbParam = Cmd.CreateParameter();

                            dbParam.ParameterName = prm.Name;
                            dbParam.Value = prm.Value;
                            Cmd.Parameters.Add(dbParam);
                        }
                    }

                    retObj = Cmd.ExecuteScalar();

                }

                Conn.CommitTransaction();
            }
            catch (Exception)
            {
                Conn.RollbackTransaction();
                throw;
            }
            finally
            {
                Conn.Close();
            }
            return retObj;
        }

        #endregion

        /* ------------------------------------------------- */

        #region [ Execute Scalar Query ]

        /// <summary>
        /// Returns scalar execution value of query.
        /// </summary>
        /// <param name="query">Sql Query</param>
        /// <returns>Returns scalar execution value of query.</returns>
        public Object ExecuteScalarAsQuery(String query)
        {
            Object retObj = null;

            try
            {
                retObj = ExecuteScalar(query, CommandType.Text, null);
            }
            catch (Exception)
            {
                throw;
            }

            return retObj;
        }

        #endregion

        #region [ Execute Scalar As Procedure ]

        /// <summary>
        /// Returns scalar execution value of Procedure.
        /// </summary>
        /// <param name="query">Sql Procedure</param>
        /// <returns>Returns scalar execution value of Procedure.</returns>
        public Object ExecuteScalarAsProcedure(String procedure)
        {
            Object retObj = null;

            try
            {
                retObj = ExecuteScalar(procedure, CommandType.StoredProcedure, null);
            }
            catch (Exception)
            {
                throw;
            }

            return retObj;
        }

        #endregion

        /* ------------------------------------------------- */

        #region [ Execute Scalar Query with Parameters ]

        /// <summary>
        /// Returns scalar execution value of query with parameters.
        /// </summary>
        /// <param name="query">Sql Query</param>
        /// <param name="parameters">Prperty contains parameters.</param>
        /// <returns>Returns scalar execution value of query with parameters.</returns>
        public Object ExecuteScalarAsQuery(String query, Property parameters)
        {
            Object retObj = null;

            try
            {
                retObj = ExecuteScalar(query, CommandType.Text, parameters);
            }
            catch (Exception)
            {
                throw;
            }

            return retObj;
        }

        #endregion

        #region [ Execute Scalar Query with Parameters ]

        /// <summary>
        /// Returns scalar execution value of Procedure with parameters.
        /// </summary>
        /// <param name="query">Sql Procedure</param>
        /// <param name="parameters">Property contains parameters.</param>
        /// <returns>Returns scalar execution value of Procedure with parameters.</returns>
        public Object ExecuteScalarAsProcedure(String procedure, Property parameters)
        {
            Object retObj = null;

            try
            {
                retObj = ExecuteScalar(procedure, CommandType.StoredProcedure, parameters);
            }
            catch (Exception)
            {
                throw;
            }

            return retObj;
        }

        #endregion

        /* ------------------------------------------------- */

        #region [ GetTableAsList method ]

        public List<T> GetTableAsList<T>() where T : new()
        {
            try
            {
                T t = (T)Activator.CreateInstance(typeof(T), null);
                MethodInfo mthdInf = t.GetType().GetMethod("GetTableName");
                Object objStr = mthdInf.Invoke(t, null);
                objStr = objStr == null ? String.Empty : objStr.ToString();
                return GetResultSetOfQuery(String.Format("Select * From {0};", objStr)).Tables[0].ToList<T>(true);
            }
            catch (Exception)
            {
                throw;
            }
        }


        #endregion


        #region [ Dispose method ]

        /// <summary>
        /// Disposes DbManager object with sub objects.
        /// </summary>
        public void Dispose()
        {
            if (Conn != null)
            {
                Conn.Dispose();
            }
            GC.SuppressFinalize(this);
        }

        #endregion


        #region [ BaseDL Destructor ]

        ~BaseDL()
        {
            Dispose();
        }

        #endregion


        #region [ DriverType Property ]

        /// <summary>
        /// Gets DriverType of Base DataLayer.
        /// </summary>
        public ConnectionTypes DriverType
        {
            get { return _driverType; }
        }

        #endregion

    }
}