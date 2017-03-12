using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISettingsProvider_Indexer
{
    class AccessSettingsProvider : BaseDbSettingsProvider
    {
        //Access 64位必须将项目改成64位才能连接！！
        private string connstr = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=db.accdb";


        protected override IDbConnection CreateDbConnection()
        {
            return new OleDbConnection(connstr);
        }
    }
}
