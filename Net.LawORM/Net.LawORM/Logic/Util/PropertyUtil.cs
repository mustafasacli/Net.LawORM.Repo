namespace Net.LawORM.Logic.Util
{
    using Net.LawORM.Base;
    using Net.LawORM.Logic.Extensions;
    using System;
    using System.Collections;
    using System.Text;

    internal class PropertyUtil
    {
        private Hashtable _hash;
        private string _connStr = string.Empty;

        public PropertyUtil()
        {
            _hash = new Hashtable();
        }

        public void Add(Object key, Object value)
        {
            try
            {
                if (key.ToStr().ToLower().Equals("conn-str") == false)
                    _hash.Add(key, value);
                else
                    _connStr = value.ToStr();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ConnectionTypes DriverType
        {
            get
            {
                try
                {
                    return ConnectionTypeBuilder.GetConnectionType(string.Format("{0}", GetValue("driver-type")));
                }
                catch (Exception)
                {
                    return ConnectionTypes.SqlServer;
                }
            }
        }


        public void Remove(Object key)
        {
            try
            {
                _hash.Remove(key);
                if (key.ToStr().ToLower().Equals("conn-str"))
                    _connStr = string.Empty;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Object GetValue(Object key)
        {
            try
            {
                if (_hash.Contains(key) == true)
                {
                    return _hash[key];
                }
                else
                    return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public String PropertyString
        {
            get
            {
                try
                {
                    if (_connStr.IsNullOrSpace() == true)
                    {
                        IDictionaryEnumerator dictEnumerator = _hash.GetEnumerator();
                        StringBuilder strBuilder = new StringBuilder();
                        while (dictEnumerator.MoveNext())
                        {
                            if (dictEnumerator.Key.ToString().Equals("driver-type") == false)
                                strBuilder.AppendFormat("{0}={1}; ", dictEnumerator.Key, dictEnumerator.Value);
                        }
                        String retstr = strBuilder.ToString();
                        retstr = retstr.TrimEnd();
                        return retstr;
                    }
                    else
                        return _connStr;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }


        public override String ToString()
        {
            return PropertyString;
        }
    }
}