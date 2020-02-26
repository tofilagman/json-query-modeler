using json_query_modeler.Logic;
using System;
using System.Collections.Generic;
using System.Text;
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
        public ISqlService Sql { get; set; }

        public ConnectionWindow()
        {
            InitializeComponent();

            if (Sql != null)
            {
                SetControls();
            }
        }

        private void SetControls()
        {
            var conInfo = Sql.ConnectionInfo;
            this.txtServer.Text = conInfo.Server;
            this.txtPort.Text = conInfo.Port.ToString();
            this.txtUsername.Text = conInfo.Username;
            this.txtPassword.Text = conInfo.Password;
            this.cbDatabase.Text = conInfo.Database;
            this.chkIntegratedSecurity.IsChecked = conInfo.TrustedConnection;
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
