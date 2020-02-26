using json_query_modeler.Logic;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace json_query_modeler
{
    /// <summary>
    /// Interaction logic for ConnectionWindow.xaml
    /// </summary>
    public partial class ConnectionWindow : Window
    {
        public ISqlService Sql { get; private set; }

        public ConnectionWindow(ISqlService sql)
        {
            InitializeComponent();

            cbProvider.ItemsSource = new List<ConnectionProvider> { ConnectionProvider.MsSql, ConnectionProvider.PostgreSql };
           
            this.Sql = sql;

            if (Sql != null)
            {
                cbProvider.SelectedItem = sql.ConnnectionType;
                SetControls();
            }
            else
            {
                cbProvider.SelectedIndex = 0;
                chkIntegratedSecurity.IsChecked = true;
                this.txtPort.Text = "-1";
            }
        }

        private void SetControls()
        {
            var conInfo = Sql.ConnectionInfo;
            this.txtServer.Text = conInfo.Server;
            this.txtPort.Text = conInfo.Port.ToString();
            this.txtUsername.Text = conInfo.Username;
            this.txtPassword.Password = conInfo.Password;
            this.cbDatabase.ItemsSource = new List<string> { conInfo.Database };
            this.cbDatabase.Text = conInfo.Database;
            this.chkIntegratedSecurity.IsChecked = conInfo.TrustedConnection;
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var sel = cbProvider.SelectedItem;

                switch (sel)
                {
                    case ConnectionProvider.MsSql:
                        var conInfo = new MsSqlConnectionInfo()
                        {
                            Server = txtServer.Text,
                            Port = Convert.ToInt32(string.IsNullOrWhiteSpace(txtPort.Text) ? "-1" : txtPort.Text),
                            TrustedConnection = chkIntegratedSecurity.IsChecked.Value,
                            Username = txtUsername.Text,
                            Password = txtPassword.Password,
                            Database = cbDatabase.Text
                        };

                        Sql = new MsSqlService();
                        Sql.Build(conInfo);
                        Sql.TestConnect();
                        break;
                    case ConnectionProvider.PostgreSql:
                        var npgInfo = new NpgConnectionInfo
                        {
                            Server = txtServer.Text,
                            Port = Convert.ToInt32(string.IsNullOrWhiteSpace(txtPort.Text) ? "-1" : txtPort.Text),
                            TrustedConnection = chkIntegratedSecurity.IsChecked.Value,
                            Username = txtUsername.Text,
                            Password = txtPassword.Password,
                            Database = cbDatabase.Text
                        };

                        Sql = new NpgService();
                        Sql.Build(npgInfo);
                        Sql.TestConnect();
                        break;
                }

                DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void chkIntegratedSecurity_Checked(object sender, RoutedEventArgs e)
        {
            lblUsername.Visibility = Visibility.Collapsed;
            lblPassword.Visibility = Visibility.Collapsed;
            txtUsername.Visibility = Visibility.Collapsed;
            txtPassword.Visibility = Visibility.Collapsed;
        }

        private void chkIntegratedSecurity_Unchecked(object sender, RoutedEventArgs e)
        {
            lblUsername.Visibility = Visibility.Visible;
            lblPassword.Visibility = Visibility.Visible;
            txtUsername.Visibility = Visibility.Visible;
            txtPassword.Visibility = Visibility.Visible;
        }

        private void cbDatabase_DropDownOpened(object sender, EventArgs e)
        {
            cbDatabase.ItemsSource = new List<string> { "<Loading databases>" };
            var sel = Enum.Parse<ConnectionProvider>(cbProvider.SelectedItem.ToString());

            try
            {
                switch (sel)
                {
                    case ConnectionProvider.MsSql:
                        var conInfo = new MsSqlConnectionInfo
                        {
                            Server = txtServer.Text,
                            Port = Convert.ToInt32(string.IsNullOrWhiteSpace(txtPort.Text) ? "-1" : txtPort.Text),
                            TrustedConnection = chkIntegratedSecurity.IsChecked.Value,
                            Username = txtUsername.Text,
                            Password = txtPassword.Password
                        };

                        var mssql = new MsSqlService();
                        cbDatabase.ItemsSource = mssql.LoadDatabase(conInfo);
                        break;
                    case ConnectionProvider.PostgreSql:
                        var npgInfo = new NpgConnectionInfo
                        {
                            Server = txtServer.Text,
                            Port = Convert.ToInt32(string.IsNullOrWhiteSpace(txtPort.Text) ? "-1" : txtPort.Text),
                            TrustedConnection = chkIntegratedSecurity.IsChecked.Value,
                            Username = txtUsername.Text,
                            Password = txtPassword.Password
                        };

                        var npgsql = new NpgService();
                        cbDatabase.ItemsSource = npgsql.LoadDatabase(npgInfo);
                        break;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}
