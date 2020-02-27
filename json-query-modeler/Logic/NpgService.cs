using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using Dapper;
using Npgsql;

namespace json_query_modeler.Logic
{
    public class NpgService : ISqlService
    {
        public IDbConnection Connection { get; private set; }
        public IConnectionInfo ConnectionInfo { get; private set; }

        public NpgService()
        {

        } 

        public string GetDisplayConnection
        {
            get
            { 
                return $"PostgreSql: {ConnectionInfo.Server}, {ConnectionInfo.Username}/{ConnectionInfo.Database}";
            }
        }

        public ConnectionProvider ConnnectionType => ConnectionProvider.PostgreSql;

        public void Build(IConnectionInfo conInfo)
        {
            ConnectionInfo = conInfo;
            Connection = new NpgsqlConnection(conInfo.ConnectionString);
        }

        public List<string> LoadDatabase(IConnectionInfo conInfo)
        {
            using (var con = new NpgsqlConnection(conInfo.ConnectionString))
            {
                return con.Query<string>("Select datname from pg_database;").ToList();
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

                using (NpgsqlCommand cmd = new NpgsqlCommand(query, Connection as NpgsqlConnection))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = 3000;
                    using (DataSet ds = new DataSet())
                    {
                        using (NpgsqlDataAdapter adp = new NpgsqlDataAdapter(cmd))
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

    public class NpgConnectionInfo : IConnectionInfo
    {
        public NpgConnectionInfo() { }

        public NpgConnectionInfo(NpgsqlConnection npgConnection) : this()
        {
            var conStr = new NpgsqlConnectionStringBuilder(npgConnection.ConnectionString);

            Server = conStr.Host;
            Port = conStr.Port;
            Username = conStr.Username;
            Password = conStr.Password;
            Database = conStr.Database;
            TrustedConnection = conStr.IntegratedSecurity;
        }

        public string Server { get; set; }

        public int Port { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Database { get; set; }

        public bool TrustedConnection { get; set; }

        public string ConnectionString
        {
            get
            {
                var conStr = new NpgsqlConnectionStringBuilder { Host = Server, Port = Port == -1 ? 5432 : Port, IntegratedSecurity = TrustedConnection };
                if (!TrustedConnection)
                {
                    conStr.Username = Username;
                    conStr.Password = Password;
                }

                if (Database != null)
                    conStr.Database = Database;

                return conStr.ConnectionString;
            }
        }
    }
}
