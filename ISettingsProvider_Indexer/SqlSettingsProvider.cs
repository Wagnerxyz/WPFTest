using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISettingsProvider_Indexer
{
    class SqlSettingsProvider : BaseDbSettingsProvider
    {
        private string connstr = @"Data Source=.\SQLEXPRESS;Initial Catalog=SJMS;Integrated Security=True";
        protected override IDbConnection CreateDbConnection()
        {
            return new SqlConnection(connstr);
        }
    }
}
