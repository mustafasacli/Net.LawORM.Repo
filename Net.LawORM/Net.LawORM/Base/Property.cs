using System;
using System.Collections.Generic;

namespace Net.LawORM.Base
{
    public sealed class Property
    {
        private List<FreeParameter> _Parameters = null;

        public static Property CreateFromStringSplit(string resource, char keyChar, char valueChar)
        {
            Property p = new Property();
            try
            {
                string[] keys = resource.Split(keyChar);
                string[] vals;
                foreach (string item in keys)
                {

                    if (string.IsNullOrWhiteSpace(item) == true)
                        continue;

                    vals = item.Split(valueChar);

                    if (vals == null)
                        continue;

                    if (vals.Length == 2)
                    {
                        p.Put(vals[0].ToString(), vals[1]);
                        continue;
                    }

                    if (vals.Length == 1)
                    {
                        p.Put(vals[0].ToString(), string.Empty);
                        continue;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return p;
        }

        private Property()
        {
            _Parameters = new List<FreeParameter>();
        }

        public static Property Instance
        {
            get
            { return new Property(); }
        }

        public Int32 Puts(params FreeParameter[] parameters)
        {
            if (parameters == null)
            {
                return -1;
            }
            else
            {
                Int32 intRet = 0;
                FreeParameter frPrm;
                foreach (FreeParameter prm in parameters)
                {
                    if (_Parameters.Contains(prm) == true)
                    {
                        frPrm = _Parameters[_Parameters.IndexOf(prm)];
                        frPrm.Value = prm.Value;
                        _Parameters[_Parameters.IndexOf(prm)] = frPrm;
                    }
                    else
                    {
                        _Parameters.Add(prm);
                        intRet++;
                    }
                }
                return intRet;
            }
        }

        public Boolean Put(FreeParameter freeParam)
        {
            Int32 index = _Parameters.IndexOf(freeParam);
            if (index == -1)
            {
                _Parameters.Add(freeParam);
                return true;
            }
            else
            {
                FreeParameter frmPrm = _Parameters[index];
                frmPrm.Value = frmPrm.Value;
                _Parameters[index] = frmPrm;
                return false;
            }
        }

        public Boolean Put(String name, Object value)
        {
            return Put(new FreeParameter() { Name = name, Value = value });
        }

        public Boolean Remove(String name)
        {
            return Remove(new FreeParameter() { Name = name });
        }

        public Boolean Remove(FreeParameter freeParam)
        {
            Int32 index = _Parameters.IndexOf(freeParam);
            if (index == -1)
            {
                return false;
            }
            else
            {
                return _Parameters.Remove(freeParam);
            }
        }

        public List<FreeParameter> GetParameters()
        {
            return _Parameters;
        }
    }

    public struct FreeParameter
    {
        public String Name { get; set; }

        public Object Value { get; set; }

        public override Boolean Equals(object obj)
        {
            if (obj == null)
                return false;

            if (Object.ReferenceEquals(obj.GetType(), typeof(FreeParameter)))
            {
                FreeParameter prm = (FreeParameter)obj;
                return this.Name.Equals(prm.Name);
            }
            return false;
        }

        public override Int32 GetHashCode()
        {
            return base.GetHashCode();
        }

        public override String ToString()
        {
            return this.Name;
        }
    }
}