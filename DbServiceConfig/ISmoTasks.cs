using System.Collections.Generic;

namespace ProductConfig
{
    public interface ISmoTasks
    {
        IEnumerable<string> SqlServers {get;}
       // List<string> GetDatabases(ConnectionStringSql connectionString);
        //List<DatabaseTable> GetTables(ConnectionStringSql connectionString);
    }
}
