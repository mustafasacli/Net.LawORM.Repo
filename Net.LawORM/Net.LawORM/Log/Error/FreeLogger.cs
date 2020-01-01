using Net.LawORM.Logic.Extensions;
using Net.LawORM.Logic.Util;
using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace Net.LawORM.Log.Error
{
    public sealed class FreeLogger
    {
        #region [ LogMethod method ]

        /// <summary>
        /// Logs Exception which handled at given method name and given class name.
        /// </summary>
        /// <param name="ex">handled exception</param>
        /// <param name="className">class name</param>
        /// <param name="methodName">method name</param>
        public static void LogMethod(Exception ex, string className, string methodName)
        {
            try
            {
                FreeLogger _log = new FreeLogger()
                {
                    LogCode = string.Format("{0}_{1}_ERR", className, methodName).Replace(' ', '_'),
                    LogType = LogTypes.Error,
                    Message = ex.Message,
                    StackTrace = ex.StackTrace,
                    Title = string.Format("An Exception handled at {0} method in {1} class.", methodName, className)
                };
                _log.Write();
            }
            catch (Exception)
            {
            }
        }

        #endregion


        #region [ LogException method ]

        /// <summary>
        /// Logs exception with given parameters.
        /// </summary>
        /// <param name="ex">Exception object</param>
        /// <param name="method">MethodBase which exception handled in.</param>
        /// <param name="userId">User Id</param>
        public static void LogException(Exception ex, MethodBase method, int userId)
        {
            try
            {
                FreeLogger.LogException(ex, string.Format("An Exception handled at {0} method.", method.Name), "MTHD_ERR", userId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion


        #region [ LogException method ]

        /// <summary>
        /// Logs exception with given parameters.
        /// </summary>
        /// <param name="ex">Exception object</param>
        /// <param name="method">MethodBase which exception handled in.</param>
        public static void LogException(Exception ex, MethodBase method)
        {
            try
            {
                FreeLogger.LogException(ex, string.Format("An Exception handled at {0} method.", method.Name), "MTHD_ERR");
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion


        #region [ LogException method ]

        /// <summary>
        /// Logs exception with given parameters.
        /// </summary>
        /// <param name="ex">Exception object</param>
        /// <param name="title">Exception Title.</param>
        /// <param name="errorcode">Error Code for Exception.</param>
        /// <param name="userId">User Id</param>
        public static void LogException(Exception ex, string title, string errorcode, int userId)
        {
            FreeLogger freeLogger = new FreeLogger()
            {
                Title = title,
                LogCode = errorcode,
                StackTrace = ex.StackTrace,
                Message = ex.Message,
                LogType = LogTypes.Error,
                UserId = userId
            };
            freeLogger.Write();
        }

        #endregion


        #region [ LogException method ]

        /// <summary>
        /// Logs exception with given parameters.
        /// </summary>
        /// <param name="ex">Exception object</param>
        /// <param name="title">Exception Title.</param>
        /// <param name="errorcode">Error Code for Exception.</param>
        public static void LogException(Exception ex, string title, string errorcode)
        {
            FreeLogger freeLogger = new FreeLogger()
            {
                Title = title,
                LogCode = errorcode,
                StackTrace = ex.StackTrace,
                Message = ex.Message,
                LogType = LogTypes.Error
            };
            freeLogger.Write();
        }

        #endregion


        #region [ Private readonly fields ]

        private readonly String filePath = "C:/FreeORM/log/log.txt";

        #endregion


        #region [ FreeLogger Ctors ]

        public FreeLogger()
        {
            LogTime = DateTime.Now;
            LogType = LogTypes.None;
        }

        public FreeLogger(Int32 userId)
        {
            UserId = userId;
            LogTime = DateTime.Now;
            LogType = LogTypes.None;
        }

        #endregion


        #region [ SaveType Property ]

        private SaveTypes SaveType
        {
            get
            {
                String saveType = ConfUtil.LogType;
                saveType = string.Format("{0}", saveType).Replace(" ", "").ToLower().Replace('ı', 'i');
                switch (saveType)
                {
                    case "db":
                    case "dbase":
                    case "database":
                        return SaveTypes.Database;

                    case "fl":
                    case "file":
                    default:
                        return SaveTypes.File;

                    case "cloud":
                    case "cld":
                        return SaveTypes.Cloud;
                }
            }
        }

        #endregion


        #region [ FilePath Property ]

        private String FilePath
        {
            get
            {
                string path = string.Empty;
                try
                {
                    path = ConfUtil.SaveFilePath;
                    path = string.Format("{0}", path).Replace(" ", "").Length == 0 ? filePath : path;
                }
                catch (Exception)
                {
                    path = filePath;
                }
                return path;
            }
        }

        #endregion


        #region [ Private Fields ]

        private String _LogCode;
        private DateTime _LogTime;
        private LogTypes _LogType = LogTypes.None;
        private String _Message;
        private String _StackTrace;
        private String _Title;
        private Int32 _UserId;

        #endregion


        #region [ Properties ]

        public String LogCode
        {
            get { return _LogCode; }
            set { _LogCode = value; }
        }

        public DateTime LogTime
        {
            get { return _LogTime; }
            internal set { _LogTime = value; }
        }

        public LogTypes LogType
        {
            get { return _LogType; }
            set { _LogType = value; }
        }

        public String Message
        {
            get { return _Message; }
            set { _Message = value; }
        }

        public String StackTrace
        {
            get { return _StackTrace; }
            set { _StackTrace = value; }
        }

        public String Title
        {
            get { return _Title; }
            set { _Title = value; }
        }

        public Int32 UserId
        {
            get { return _UserId; }
            set { _UserId = value; }
        }

        #endregion


        #region [ Write method ]

        public void Write()
        {
            try
            {
                switch (SaveType)
                {
                    case SaveTypes.File:
                        Write2File();
                        break;

                    default:
                    case SaveTypes.Database:
                        Write2Db();
                        break;

                    case SaveTypes.Cloud:
                        Write2Cloud();
                        break;
                }
            }
            catch (Exception)
            {
            }
        }

        #endregion


        #region [ Write2File method ]

        private void Write2File()
        {
            try
            {
                FileMode fMode = File.Exists(FilePath) ? FileMode.Append : FileMode.OpenOrCreate;

                StringBuilder strBuilder = new StringBuilder();
                strBuilder.AppendFormat("Title : {0}", _Title).AppendLine();
                strBuilder.AppendFormat("Log Code : {0}", _LogCode).AppendLine();
                strBuilder.AppendFormat("Log Type : {0}", _LogType).AppendLine();
                strBuilder.AppendFormat("Message : {0}", _Message).AppendLine();
                strBuilder.AppendFormat("Stack Trace : {0}", _StackTrace).AppendLine();
                strBuilder.AppendFormat("Log Time : {0}", _LogTime.ToString("yyyy-MM-dd HH:mm:ss")).AppendLine();
                strBuilder.AppendLine("/* ------------------------------------------------*/");
                using (StreamWriter outfile = new StreamWriter(new FileStream(FilePath, fMode)))
                {
                    outfile.Write(strBuilder.ToString());
                }
            }
            catch (Exception)
            {
            }
        }

        #endregion


        #region [ Write2Db method ]

        private void Write2Db()
        {
            try
            {
                using (LogDL lgDL = new LogDL())
                {
                    Log log = new Log()
                    {
                        Title = _Title,
                        LogCode = _LogCode,
                        LogTime = _LogTime,
                        OriginalMessage = _Message,
                        StackTrace = _StackTrace,
                        LogType = (int)_LogType
                    };

                    if (_UserId != 0)
                        log.UserId = _UserId;

                    lgDL.Insert(log);
                }
            }
            catch (Exception)
            {
            }
        }

        #endregion


        #region [ Write2Cloud method ]

        private void Write2Cloud()
        {
            try
            {
                using (LogDL lgDL = new LogDL("ext"))
                {
                    Log log = new Log()
                    {
                        Title = _Title,
                        LogCode = _LogCode,
                        LogTime = _LogTime,
                        OriginalMessage = _Message,
                        StackTrace = _StackTrace,
                        LogType = (int)_LogType
                    };

                    if (_UserId != 0)
                        log.UserId = _UserId;

                    lgDL.Insert(log);
                }
            }
            catch (Exception)
            {
            }
        }

        #endregion

    }
}