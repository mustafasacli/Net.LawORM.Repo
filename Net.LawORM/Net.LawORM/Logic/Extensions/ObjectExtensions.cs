using System;

namespace Net.LawORM.Logic.Extensions
{
    public static class ObjectExtensions
    {
        public static Boolean IsNull(this Object o)
        {
            return o == null;
        }

        public static Boolean IsNullOrDbNull(this Object obj)
        {
            return (null == obj || obj == DBNull.Value);
        }

        public static String ToStr(this Object obj)
        {
            return obj.IsNullOrDbNull() == true ? String.Empty : obj.ToString();
        }

        public static Int32 ToInt(this Object obj)
        {
            try
            {
                return obj.ToStr().Str2Int();
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static String NTrim(this string str)
        {
            try
            {
                if (str == null)
                    return null;

                string str2 = str.Replace(" ", "");

                return str2;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static Int32 Str2Int(this String str)
        {
            try
            {
                return int.Parse(str);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static Int32 Char2Int(this char ch)
        {
            try
            {
                return Convert.ToInt32(ch);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static Boolean IsNullOrEmpty(this String str)
        {
            if (str == null)
            {
                return true;
            }
            else
            {
                return str.Length == 0;
            }
        }

        public static Boolean IsNullOrSpace(this String str)
        {
            if (str == null)
            {
                return true;
            }
            else
            {
                return str.Trim().Length == 0;
            }
        }

        public static Int32 FirstIndexOf(this string str, char ch)
        {
            try
            {
                if (str.IsNullOrEmpty())
                    return -1;

                if (ch.IsNull())
                    return -1;

                char[] chs = str.ToCharArray();
                int _index = -1;
                for (int charCounter = 0; charCounter < chs.Length; charCounter++)
                {
                    if (string.Format("{0}", ch).Equals(string.Format("{0}", chs[charCounter])))
                    {
                        _index = charCounter;
                        break;
                    }
                }
                return _index;
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}