using Net.LawORM.Logic.BaseDal;
using System;

namespace Net.LawORM.Log.Error
{
    internal class LogDL : BaseDL
    {
        public LogDL()
            : base()
        { }

        public LogDL(String logName)
            : base(logName)
        {
        }
    }
}