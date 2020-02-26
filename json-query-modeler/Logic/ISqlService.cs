using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace json_query_modeler.Logic
{
    public interface ISqlService
    {
        IDbConnection Connection { get; }
        IConnectionInfo ConnectionInfo { get; }

        void Build(DbConnectionStringBuilder sqlConnectionString);

    }

    public interface IConnectionInfo
    {
        string Server { get; }
        int Port { get; }
        string Username { get; }
        string Password { get; }
        string Database { get; }
        bool TrustedConnection { get; }
    }
}
