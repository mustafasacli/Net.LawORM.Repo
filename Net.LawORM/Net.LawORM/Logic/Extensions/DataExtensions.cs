using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;

namespace Net.LawORM.Logic.Extensions
{
    /// <summary>
    /// Description of DataExtensions.
    /// </summary>
    public static class DataExtensions
    {

        #region [ DataTable To Generic List ]

        /// <summary>
        /// This method returns A List of T object.
        /// </summary>
        /// <typeparam name="T">T object type</typeparam>
        /// <param name="datatable">Datatble object</param>
        /// <param name="accordingToColumn">if it is true, returns a List with DataTable Columns else returns a List with PropertyInfo of Object.</param>
        /// <returns>Returns A List of T object.</returns>
        public static List<T> ToList<T>(this DataTable datatable, Boolean accordingToColumn) where T : new()
        {
            try
            {
                List<T> liste = new List<T>();
                Object obj;
                T item = new T();
                if (accordingToColumn == true)
                {
                    PropertyInfo propInfo = null;
                    foreach (DataRow row in datatable.Rows)
                    {
                        item = new T();
                        foreach (DataColumn col in datatable.Columns)
                        {
                            obj = row[col.ColumnName];
                            if (null != obj && obj != DBNull.Value)
                            {
                                propInfo = typeof(T).GetProperty(col.ColumnName);
                                propInfo.SetValue(item, obj);
                            }

                        }
                        liste.Add(item);
                    }
                }
                else
                {
                    PropertyInfo[] pInfos = typeof(T).GetProperties();
                    foreach (DataRow row in datatable.Rows)
                    {
                        item = new T();
                        for (int proCounter = 0; proCounter < pInfos.Length; proCounter++)
                        {
                            obj = row[pInfos[proCounter].Name];
                            if (obj.IsNullOrDbNull() == false)
                                pInfos[proCounter].SetValue(item, obj);
                        }
                        liste.Add(item);
                    }
                }
                return liste;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion


        #region [ GetPageOfDataTable method ]

        public static DataTable GetPageOfDataTable(this DataTable dt, Int32 pageNumber, Int32 rowCount)
        {
            try
            {
                if (dt == null)
                    throw new System.NullReferenceException("DataTable object can not be null.");

                if (pageNumber < 0)
                    throw new Exception("Page Number cannot be less than 0.");
                if (rowCount < 1)
                    throw new Exception("Row Count of Page cannot be less than 1.");

                DataTable retDt = new DataTable();
                foreach (DataColumn col in dt.Columns)
                {
                    retDt.Columns.Add(new DataColumn(col.ColumnName, col.DataType));
                }

                Int32 totalRow = dt.Rows.Count;
                Int32 rowNo = pageNumber * rowCount;
                if (totalRow > rowNo)
                {
                    Int32 rowForCounter = totalRow > (rowNo + rowCount) ? rowCount : (totalRow - rowNo);

                    for (Int32 i = 0; i < rowForCounter; i++)
                    {
                        DataRow row = dt.Rows[rowNo + i];
                        retDt.Rows.Add(row.ItemArray);
                    }
                }

                return retDt;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion


        #region [ Get Column Numbers Of DataTable ]

        /// <summary>
        /// Returns a DataTable with Selected column names.
        /// </summary>
        /// <param name="dt">DataTable object</param>
        /// <param name="columnList"> column names array </param>
        /// <returns>Returns a DataTable with Selected column names.</returns>
        public static DataTable GetColumnsOfDataTable(this DataTable dt, params String[] columnList)
        {
            try
            {
                DataTable ndt = new DataTable();
                DataColumn dtCol = null;
                foreach (string colName in columnList)
                {
                    dtCol = new DataColumn(dt.Columns[colName].ColumnName, dt.Columns[colName].DataType);
                    ndt.Columns.Add(dtCol);
                }
                List<Object> rowItems = null;
                foreach (DataRow row in dt.Rows)
                {
                    rowItems = new List<Object>();
                    foreach (String colName in columnList)
                    {
                        rowItems.Add(row[colName]);
                    }
                    ndt.Rows.Add(rowItems.ToArray());
                }
                return ndt;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion


        #region [ Get Columns Of DataTable ColumnNumbers ]

        /// <summary>
        /// Returns a DataTable with Selected column numbers.
        /// </summary>
        /// <param name="dt">DataTable object</param>
        /// <param name="columnList">column numbers array</param>
        /// <returns>Returns a DataTable with Selected column numbers.</returns>
        public static DataTable GetColumnsOfDataTable(this DataTable dt, params Int32[] columnList)
        {
            try
            {
                DataTable ndt = new DataTable();
                DataColumn dtCol = null;
                foreach (Int32 colNo in columnList)
                {
                    dtCol = new DataColumn(dt.Columns[colNo].ColumnName, dt.Columns[colNo].DataType);
                    ndt.Columns.Add(dtCol);
                }
                List<Object> rowItems = null;
                foreach (DataRow row in dt.Rows)
                {
                    rowItems = new List<Object>();
                    foreach (Int32 colNo in columnList)
                    {
                        rowItems.Add(row[colNo]);
                    }
                    ndt.Rows.Add(rowItems.ToArray());
                }
                return ndt;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion


        #region [ Get Object With Selected Column ]

        /// <summary>
        /// Returns a object with given parameters.
        /// </summary>
        /// <param name="dt">DataTable object</param>
        /// <param name="refColumn">Name of Reference Column</param>
        /// <param name="refValue">Value of Reference Column</param>
        /// <param name="destinationColumn">Name of Destination Column</param>
        /// <returns>Returns a object at destination column which contains reference value at reference column. Otherwise return null.</returns>
        public static Object GetObjectWithSelectedColumn(this DataTable dt, String refColumn, Object refValue, String destinationColumn)
        {
            try
            {
                Object retObj = null;
                foreach (DataRow row in dt.Rows)
                {
                    if (row[refColumn].ToString().Equals(refValue.ToString()))
                    {
                        retObj = row[destinationColumn];
                        break;
                    }
                }
                return retObj;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion


        #region [ Export As Excel With Include Columns ]

        public static void ExportAsExcelWithIncludeColumns(this DataTable dt, String fileName, params Object[] includeColumns)
        {
            try
            {
                if (includeColumns.IsNull())
                {
                    return;
                }
                else
                {
                    using (StreamWriter sWriter = new StreamWriter(new FileStream(fileName, FileMode.OpenOrCreate)))
                    {
                        sWriter.AutoFlush = true;

                        foreach (String col in includeColumns)
                        {
                            sWriter.Write("{0}\t", col);
                        }
                        sWriter.Write("\n");

                        foreach (DataRow rw in dt.Rows)
                        {
                            foreach (String col in includeColumns)
                            {
                                sWriter.Write("{0}\t", rw[col].ToStr().Replace("\n", " "));
                            }
                            sWriter.Write("\n");
                        }
                    }
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion


        #region [ Export As Excel With Exclude Columns ]

        public static void ExportAsExcelWithExcludeColumns(this DataTable dt, String fileName, params Object[] excludeColumns)
        {
            try
            {
                if (excludeColumns.IsNull())
                {
                    using (StreamWriter sWriter = new StreamWriter(new FileStream(fileName, FileMode.OpenOrCreate)))
                    {
                        sWriter.AutoFlush = true;

                        foreach (DataColumn col in dt.Columns)
                        {
                            sWriter.Write("{0}\t", col.ColumnName);
                        }
                        sWriter.Write("\n");

                        foreach (DataRow rw in dt.Rows)
                        {
                            foreach (DataColumn col in dt.Columns)
                            {
                                sWriter.Write("{0}\t", rw[col].ToStr().Replace("\n", " "));
                            }
                            sWriter.Write("\n");
                        }
                    }
                }
                else
                {

                    System.Collections.Generic.List<string> colList = new System.Collections.Generic.List<string>();

                    foreach (DataColumn col in dt.Columns)
                    {
                        colList.Add(col.ColumnName);
                    }

                    foreach (object obj in excludeColumns)
                    {
                        if (colList.Contains(obj.ToStr()) == true)
                        {
                            colList.Remove(obj.ToStr());
                        }
                    }


                    using (StreamWriter sWriter = new StreamWriter(new FileStream(fileName, FileMode.OpenOrCreate)))
                    {
                        sWriter.AutoFlush = true;

                        foreach (string col in colList)
                        {
                            sWriter.Write(string.Format("{0}\t", col));
                        }
                        sWriter.Write("\n");

                        foreach (DataRow rw in dt.Rows)
                        {
                            foreach (string col in colList)
                            {
                                sWriter.Write("{0}\t", rw[col].ToStr().Replace("\n", " "));
                            }
                            sWriter.Write("\n");
                        }
                    }
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion


        #region [ Copy method ]

        public static DataTable Copy(this DataTable dt)
        {
            try
            {
                DataTable _dataT = new DataTable();
                foreach (DataColumn col in dt.Columns)
                {
                    _dataT.Columns.Add(col.ColumnName, col.DataType);
                }

                DataRow dr;
                foreach (DataRow row in dt.Rows)
                {
                    dr = null;
                    dr = _dataT.NewRow();
                    foreach (DataColumn col in dt.Columns)
                    {
                        dr[col.ColumnName] = row[col.ColumnName];
                    }
                    _dataT.Rows.Add(dr);
                }

                return _dataT;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion


        #region [ GetSomeColumnsAsTable method ]

        public static DataTable GetSomeColumnsAsTable(this DataTable dt, string[] columnList)
        {
            try
            {
                DataTable dtNew = new DataTable();
                if (dt == null)
                    return dtNew;

                if (columnList == null)
                    return dtNew;

                if (columnList.Length == 0)
                    return dtNew;

                DataColumn _col;
                foreach (string col in columnList)
                {
                    _col = dt.Columns[col];
                    dtNew.Columns.Add(_col.ColumnName, _col.DataType);
                }

                DataRow dr = null;
                foreach (DataRow row in dt.Rows)
                {
                    dr = dtNew.NewRow();
                    foreach (string col in columnList)
                    {
                        dr[col] = row[col];
                    }
                    dtNew.Rows.Add(dr);
                }
                return dtNew;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion


        #region [ Search method ]

        public static DataTable Search(this DataTable dt, string refColumn, string value, bool isExactMatch)
        {
            DataTable nDt = new DataTable();
            try
            {

                foreach (DataColumn col in dt.Columns)
                {
                    nDt.Columns.Add(col.ColumnName, col.DataType);
                }
                DataRow dRow = null;
                if (isExactMatch)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        if (string.Format("{0}", row[refColumn]).Equals(value))
                        {
                            dRow = null;
                            dRow = nDt.NewRow();
                            foreach (DataColumn col in nDt.Columns)
                            {
                                dRow[col.ColumnName] = row[col.ColumnName];
                            }
                            nDt.Rows.Add(dRow);
                        }
                    }
                }
                else
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        if (string.Format("{0}", row[refColumn]).Contains(value))
                        {
                            dRow = null;
                            dRow = nDt.NewRow();
                            foreach (DataColumn col in nDt.Columns)
                            {
                                dRow[col.ColumnName] = row[col.ColumnName];
                            }
                            nDt.Rows.Add(dRow);
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nDt;
        }

        #endregion


        #region [ MergeSomeColumns method ]

        public static DataTable MergeSomeColumns(this DataTable dt, string newColumnName, string[] mergedColumnNames)
        {
            DataTable ndt = new DataTable();
            try
            {
                if (dt.IsNull())
                    return ndt;

                ndt = Copy(dt);

                if (mergedColumnNames.IsNull())
                {
                    return ndt;
                }

                if (mergedColumnNames.Length == 0)
                {
                    return ndt;
                }

                if (newColumnName.IsNullOrSpace())
                {
                    return ndt;
                }

                int ordinal = -1;
                foreach (string col in mergedColumnNames)
                {
                    if (ndt.Columns[col].Ordinal > ordinal)
                    {
                        ordinal = ndt.Columns[col].Ordinal;
                    }
                }

                List<string> colsList = new List<string>();
                colsList.Add(newColumnName);
                for (int colCounter = ordinal + 1; colCounter < ndt.Columns.Count; colCounter++)
                {
                    colsList.Add(ndt.Columns[colCounter].ColumnName);
                }

                ndt.Columns.Add(newColumnName, typeof(string));
                string strRow = null;
                for (int rowCounter = 0; rowCounter < ndt.Rows.Count; rowCounter++)
                {
                    strRow = string.Empty;
                    foreach (string col in mergedColumnNames)
                    {
                        if (string.Format("{0}", ndt.Rows[rowCounter][col]).Replace(" ", "").Length > 0)
                        {
                            strRow = string.Format("{0} {1}", strRow, ndt.Rows[rowCounter][col]);
                        }
                    }
                    strRow = strRow.TrimStart();
                    ndt.Rows[rowCounter][newColumnName] = strRow;
                }

                for (int colCounter = 0; colCounter < colsList.Count; colCounter++)
                {
                    ndt.Columns[colsList[colCounter]].SetOrdinal(ordinal + colCounter + 1);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ndt;
        }

        #endregion


        #region [ SetColumnsOrder method ]

        public static void SetColumnsOrder(this DataTable table, params String[] columnNames)
        {
            try
            {
                for (int columnIndex = 0; columnIndex < columnNames.Length; columnIndex++)
                {
                    table.Columns[columnNames[columnIndex]].SetOrdinal(columnIndex);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

    }
}