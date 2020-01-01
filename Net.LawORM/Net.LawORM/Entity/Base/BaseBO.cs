using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Text;

namespace Net.LawORM.Entity.Base
{
    [Serializable]
    public abstract class BaseBO : IBaseBO, INotifyPropertyChanged
    {
        #region [ Private Fileds ]

        private List<String> _changeList = null;

        #endregion


        #region [ BaseBO Ctor ]

        /// <summary>
        /// BaseBO Ctor.
        /// </summary>
        protected BaseBO()
        {
            _changeList = new List<String>();
            this.PropertyChanged += BaseBO_PropertyChanged;
        }

        #endregion


        #region [ GetTableName method ]

        /// <summary>
        /// Gets Table Name Of BaseBO object.
        /// </summary>
        /// <returns>Returns Table Name Of BaseBO object.</returns>
        public virtual String GetTableName()
        {
            throw new NotImplementedException("Table Name is not implemented.");
        }

        #endregion


        #region [ GetIdColumn method ]

        /// <summary>
        ///  Gets Identity Column Name Of BaseBO object.
        /// </summary>
        /// <returns>Returns Identity Column Name Of BaseBO object.</returns>
        public virtual String GetIdColumn()
        {
            throw new NotImplementedException("Id Column is not implemented.");
        }

        #endregion


        #region [ GetColumnChangeList method ]

        /// <summary>
        /// Gets Column Name list with property changed.
        /// </summary>
        /// <returns>Returns Column Name list with property changed.</returns>
        internal List<String> GetColumnChangeList()
        {
            return _changeList;
        }

        #endregion


        #region [ Equals method ]

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj">object instance which inherits BaseBO object.</param>
        /// <returns></returns>
        public override Boolean Equals(Object obj)
        {
            if (Object.ReferenceEquals(obj.GetType(), this.GetType()))
            {
                BaseBO bo = (BaseBO)obj;
                if (bo.GetTableName().Equals(this.GetTableName()) & bo.GetIdColumn().Equals(this.GetIdColumn()))
                {
                    PropertyInfo propInf = bo.GetType().GetProperty(bo.GetIdColumn());
                    Object obj1 = propInf.GetValue(bo);
                    Object obj2 = propInf.GetValue(this);
                    return Object.Equals(obj1, obj2);
                }
                else
                    return false;
            }
            else
                return false;
        }

        #endregion


        #region [ GetHashCode method ]

        /// <summary>
        /// Gets HashCode of object.
        /// </summary>
        /// <returns>Returns HashCode int.</returns>
        public override Int32 GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion


        #region [ Serialize method ]

        /// <summary>
        /// Write Object serialization result to a file.
        /// </summary>
        /// <param name="fileName"> File Name for Serialization object writing. </param>
        /// <param name="append">if append is true Serialization continues with append.</param>
        public void Serialize(String fileName, Boolean append)
        {
            try
            {
                StringBuilder strBuilder = new StringBuilder();
                PropertyInfo[] pInfos = this.GetType().GetProperties();
                strBuilder.AppendLine(String.Format("Name : {0} ", this.GetType().Name));
                Object objVal;

                foreach (PropertyInfo inf in pInfos)
                {
                    if (inf.Name.Equals(this.GetIdColumn()) == false)
                    {
                        objVal = inf.GetValue(this, null);
                        strBuilder.AppendLine(String.Format("{0} : {1} ", inf.Name, objVal));
                    }
                }

                if (append == true)
                    strBuilder.AppendLine("/* ------------------------------------------------*/");

                FileMode fMode = append == true ? FileMode.Append : FileMode.Create;

                using (StreamWriter outfile = new StreamWriter(new FileStream(fileName, fMode)))
                {
                    outfile.Write(strBuilder.ToString());
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Write Object serialization result to a file.
        /// </summary>
        /// <param name="fileName"> File Name for Serialization object writing. </param>
        public void Serialize(String fileName)
        {
            try
            {
                Serialize(fileName, false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion


        #region [ BaseBO_PropertyChanged method ]

        private void BaseBO_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_changeList.Contains(e.PropertyName) == false)
                _changeList.Add(e.PropertyName);
        }

        #endregion


        #region [ PropertyChanged EventHandler ]

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion


        #region [ OnPropertyChanged method ]

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion


        #region [ ChangeSetCount Property ]

        /// <summary>
        /// Gets Count of Changed Property.
        /// </summary>
        public int ChangeSetCount
        {
            get
            {
                return _changeList.Count;
            }
        }

        #endregion


        #region [ IsPropertyChanged method ]

        /// <summary>
        /// Returns true if Value of Property with given name changes, return true; else returns false.
        /// </summary>
        /// <param name="propName">Property Name</param>
        /// <returns>Returns bool object.</returns>
        public bool IsPropertyChanged(string propName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(propName))
                    return false;

                return _changeList.Contains(propName);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

    }
}