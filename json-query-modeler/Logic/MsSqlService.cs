using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;

namespace json_query_modeler.Logic
{
    public class MsSqlService : ISqlService
    {
        public IDbConnection Connection { get; private set; }

        public IConnectionInfo ConnectionInfo
        {
            get
            {
                return new MsSqlConnectionInfo(Connection as SqlConnection);
            }
        }

        public MsSqlService()
        {

        }

        public void Build(DbConnectionStringBuilder sqlConnectionString)
        {
            var conStr = sqlConnectionString as SqlConnectionStringBuilder;
            Connection = new SqlConnection(conStr.ConnectionString);
        } 
    }

    public class MsSqlConnectionInfo : IConnectionInfo
    { 
        public MsSqlConnectionInfo (SqlConnection sqlConnection)
        {
            var conStr = new SqlConnectionStringBuilder(sqlConnection.ConnectionString);

            Server = conStr.DataSource;
            Port = 1433;
            Username = conStr.UserID;
            Password = conStr.Password;
            Database = conStr.InitialCatalog;
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
