using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace json_query_modeler.Logic
{
    public class MsSqlService : ISqlService
    {
        public IDbConnection Connection { get; private set; }
        public IConnectionInfo ConnectionInfo { get; private set; }

        public MsSqlService()
        {

        }

        public string GetDisplayConnection
        {
            get
            {
                return $"MsSql: {ConnectionInfo.Server}, {ConnectionInfo.Username}/{ConnectionInfo.Database}";
            }
        }

        public ConnectionProvider ConnnectionType => ConnectionProvider.MsSql;

        public void Build(IConnectionInfo conInfo)
        {
            ConnectionInfo = conInfo;
            Connection = new SqlConnection(conInfo.ConnectionString);
        }

        public List<string> LoadDatabase(IConnectionInfo conInfo)
        {
            using (var con = new SqlConnection(conInfo.ConnectionString))
            {
                return con.Query<string>("select name from sys.databases where snapshot_isolation_state = 0 ORDER by name").ToList();
            }
        }

        public void TestConnect()
        {
            Connection.Open();
            Connection.Close();
        }

        public DataSet Query(string query)
        {
            try
            {
                Connection.Open();

                using (SqlCommand cmd = new SqlCommand(query, Connection as SqlConnection))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = 3000;
                    using (DataSet ds = new DataSet())
                    {
                        using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                        {
                            adp.Fill(ds);
                            return ds;
                        }
                    }
                }
            }
            finally
            {
                Connection.Close();
            }
        }
    }

    public class MsSqlConnectionInfo : IConnectionInfo
    {
        public MsSqlConnectionInfo()
        {
            Port = -1;
        }

        public MsSqlConnectionInfo(SqlConnection sqlConnection) : this()
        {
            var conStr = new SqlConnectionStringBuilder(sqlConnection.ConnectionString);
            var srv = conStr.DataSource.Split(',');

            Server = srv[0];
            Port = srv.Length > 1 ? Convert.ToInt32(srv[1].Trim()) : Port;
            Username = conStr.UserID;
            Password = conStr.Password;
            Database = conStr.InitialCatalog;
            TrustedConnection = conStr.IntegratedSecurity;
        }

        public string ConnectionString
        {
            get
            {
                var src = Port == -1 ? Server : $"{Server},{Port}";

                var conStr = new SqlConnectionStringBuilder { DataSource = src, IntegratedSecurity = TrustedConnection };
                if (!TrustedConnection)
                {
                    conStr.UserID = Username;
                    conStr.Password = Password;
                }

                if (Database != null)
                    conStr.InitialCatalog = Database;

                return conStr.ConnectionString;
            }
        }

        public string Server { get; set; }

        public int Port { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Database { get; set; }

        public bool TrustedConnection { get; set; }
    }
}
