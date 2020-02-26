using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using Npgsql;

namespace json_query_modeler.Logic
{
    public class NpgService : ISqlService
    {
        public IDbConnection Connection { get; private set; }

        public IConnectionInfo ConnectionInfo
        {
            get
            {
                return new NpgConnectionInfo(Connection as NpgsqlConnection);
            }
        }

        public NpgService()
        {

        }

        public void Build(DbConnectionStringBuilder sqlConnectionString)
        {
            var constr = sqlConnectionString as NpgsqlConnectionStringBuilder;
            Connection = new NpgsqlConnection(constr.ConnectionString);
        }
    }

    public class NpgConnectionInfo : IConnectionInfo
    {
        public NpgConnectionInfo(NpgsqlConnection npgConnection)
        {
            var conStr = new NpgsqlConnectionStringBuilder(npgConnection.ConnectionString);

            Server = conStr.Host;
            Port = conStr.Port;
            Username = conStr.Username;
            Password = conStr.Password;
            Database = conStr.Database;
            TrustedConnection = conStr.IntegratedSecurity;
        }

        public string Server { get; private set; }

        public int Port { get; private set; }

        public string Username { get; private set; }

        public string Password { get; private set; }

        public string Database { get; private set; }

        public bool TrustedConnection { get; private set; }
    }
}
