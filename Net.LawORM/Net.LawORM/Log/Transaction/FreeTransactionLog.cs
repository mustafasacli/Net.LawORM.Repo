namespace Net.LawORM.Log.Transaction
{
    using Net.LawORM.Entity.Base;
    using Net.LawORM.Log.Error;
    using Net.LawORM.Logic.Util;
    using System;

    public sealed class FreeTransactionLog : BaseBO, IDisposable
    {
        private Int32 _UserId;
        private String _TransactionType;
        private Int32 _LogObject;
        private DateTime _LogTime;
        private String _ControlTag;
        private String _ControlName;
        private String _FormName;
        private Int32 _OBJID;

        public FreeTransactionLog(Int32 userId)
        {
            UserId = userId;
            LogTime = DateTime.Now;
            OnPropertyChanged("MachineName");
        }

        public Int32 OBJID
        {
            set { _OBJID = value; OnPropertyChanged("OBJID"); }
            get { return _OBJID; }
        }
        public String FormName
        {
            set { _FormName = value; OnPropertyChanged("FormName"); }
            get { return _FormName; }
        }
        public String ControlName
        {
            set { _ControlName = value; OnPropertyChanged("ControlName"); }
            get { return _ControlName; }
        }
        public String ControlTag
        {
            set { _ControlTag = value; OnPropertyChanged("ControlTag"); }
            get { return _ControlTag; }
        }
        public DateTime LogTime
        {
            private set { _LogTime = value; OnPropertyChanged("LogTime"); }
            get { return _LogTime; }
        }
        public Int32 LogObject
        {
            set { _LogObject = value; OnPropertyChanged("LogObject"); }
            get { return _LogObject; }
        }
        public String TransactionType
        {
            set { _TransactionType = value; OnPropertyChanged("TransactionType"); }
            get { return _TransactionType; }
        }
        public Int32 UserId
        {
            set { _UserId = value; OnPropertyChanged("UserId"); }
            get { return _UserId; }
        }

        public String MachineName
        {
            get
            {
                string _machineName = string.Empty;
                try
                {
                    _machineName = Environment.MachineName;
                }
                catch (Exception)
                {
                    _machineName = String.Empty;
                }
                if (_machineName.Length > 50)
                {
                    _machineName = _machineName.Substring(0, 50);
                }
                return _machineName;
            }
        }

        public override String GetIdColumn()
        {
            return "OBJID";
        }

        public override String GetTableName()
        {
            return "TransactionLog";
        }

        public Int32 Insert()
        {
            int InsertResult = -2;
            try
            {
                switch (SaveType)
                {
                    case SaveTypes.File:
                    case SaveTypes.Database:
                        using (TransactionLogDL transDL = new TransactionLogDL())
                        {
                            InsertResult = transDL.Insert(this);
                        }
                        break;

                    case SaveTypes.Cloud:
                        using (TransactionLogDL transDL = new TransactionLogDL("ext"))
                        {
                            InsertResult = transDL.Insert(this);
                        }
                        break;

                    default: break;
                }
            }
            catch (Exception)
            {
                InsertResult = -3;
            }
            return InsertResult;
        }


        private SaveTypes SaveType
        {
            get
            {
                String saveType = ConfUtil.SaveType;
                saveType = string.Format("{0}", saveType).Replace(" ", "").ToLower();
                switch (saveType)
                {
                    case "db":
                    case "dbase":
                    case "database":
                    default:
                        return SaveTypes.Database;

                    case "cloud":
                    case "cld":
                        return SaveTypes.Cloud;
                }
            }

        }

        public void Dispose()
        {
            System.GC.SuppressFinalize(this);
        }

    }
}