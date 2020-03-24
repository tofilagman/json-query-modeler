using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace json_query_modeler.Logic
{
    public interface ISqlService
    {
        ConnectionProvider ConnnectionType { get; }
        IDbConnection Connection { get; }
        IConnectionInfo ConnectionInfo { get; }

        void Build(IConnectionInfo connectionInfo);
        List<string> LoadDatabase(IConnectionInfo conInfo);
        void TestConnect();

        string GetDisplayConnection { get; } 
        DataSet Query(string query);
    }

    public interface IConnectionInfo
    {
        string Server { get; set; }
        int Port { get; set; }
        string Username { get; set; }
        string Password { get; set; }
        string Database { get; set; }
        bool TrustedConnection { get; set; }

        string ConnectionString { get; }
    }

    public enum ConnectionProvider
    {
        MsSql = 0,
        PostgreSql = 1,
        MySql = 2
    }
}
