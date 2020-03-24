using Dapper;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace json_query_modeler.Logic
{
    public class MySqlService : ISqlService
    {
        public IDbConnection Connection { get; private set; }
        public IConnectionInfo ConnectionInfo { get; private set; }

        public MySqlService()
        {

        }

        public string GetDisplayConnection
        {
            get
            {
                return $"MySql: {ConnectionInfo.Server}, {ConnectionInfo.Username}/{ConnectionInfo.Database}";
            }
        }

        public ConnectionProvider ConnnectionType => ConnectionProvider.MySql;

        public void Build(IConnectionInfo conInfo)
        {
            ConnectionInfo = conInfo;
            Connection = new MySqlConnection(conInfo.ConnectionString);
        }

        public List<string> LoadDatabase(IConnectionInfo conInfo)
        {
            using (var con = new MySqlConnection(conInfo.ConnectionString))
            {
                return con.Query<string>("Show Databases;").ToList();
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

                using (MySqlCommand cmd = new MySqlCommand(query, Connection as MySqlConnection))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = 3000;
                    using (DataSet ds = new DataSet())
                    {
                        using (MySqlDataAdapter adp = new MySqlDataAdapter(cmd))
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

    public class MySqlConnectionInfo : IConnectionInfo
    {
        public MySqlConnectionInfo()
        {
            Port = 3306;
        }

        public MySqlConnectionInfo(MySqlConnection sqlConnection) : this()
        {
            var conStr = new MySqlConnectionStringBuilder(sqlConnection.ConnectionString);
            var srv = conStr.Server.Split(',');

            Server = conStr.Server;
            Port = Convert.ToInt32(conStr.Port);
            Username = conStr.UserID;
            Password = conStr.Password;
            Database = conStr.Database;
            TrustedConnection = false;
        }

        public string ConnectionString
        {
            get
            {
                var conStr = new MySqlConnectionStringBuilder { Server = Server, Port = Convert.ToUInt32(Port) };
                if (!TrustedConnection)
                {
                    conStr.UserID = Username;
                    conStr.Password = Password;
                }

                if (Database != null)
                    conStr.Database = Database;

                return conStr.ConnectionString;
            }
        }

        public string Server { get; set; }

        public int Port { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Database { get; set; }

        public bool TrustedConnection { get; set; } = false;
    }
}
