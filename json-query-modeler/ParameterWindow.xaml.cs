using json_query_modeler.Logic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using z.Data;

namespace json_query_modeler
{
    /// <summary>
    /// Interaction logic for ParameterWindow.xaml
    /// </summary>
    public partial class ParameterWindow : Window
    {
        public List<ParameterData> ParamSet { get; private set; }

        public ParameterWindow(List<ParameterData> paramSet)
        {
            InitializeComponent();
            this.ParamSet = paramSet;
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.grdMain.CommitEdit();
                var kj = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "JsonQueryModeler", "appdata.json");
                var gh = this.ParamSet.ToJson(true);
                File.WriteAllText(kj, gh);
                DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.grdMain.ItemsSource = this.ParamSet;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.grdMain.CancelEdit();
            DialogResult = false;
        }

        private void grdMain_AddingNewItem(object sender, System.Windows.Controls.AddingNewItemEventArgs e)
        {
            try
            {
                e.NewItem = new ParameterData { SystemDefault = false };
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void grdMain_CellEditEnding(object sender, System.Windows.Controls.DataGridCellEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Cancel)
                return;

            var item = (e.Row.Item as ParameterData);
            if (item.SystemDefault && e.Column.Header.ToString() == "Name")
            {
                e.Cancel = true;
                (sender as DataGrid).CancelEdit(DataGridEditingUnit.Cell);
            } 
        }

        private void DataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            DataGrid dg = sender as DataGrid;
            if (dg != null)
            {
                DataGridRow dgr = (DataGridRow)(dg.ItemContainerGenerator.ContainerFromIndex(dg.SelectedIndex));
                if (e.Key == Key.Delete && !dgr.IsEditing)
                {
                    var dgi = dgr.Item as ParameterData;
                    e.Handled = dgi.SystemDefault;
                }
            }
        }
    }
}
