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
    /// Interaction logic for ParameterWindow.xaml
    /// </summary>
    public partial class ParameterWindow : Window
    {
        public List<ParameterData> ParamSet { get; private set; }

        public ParameterWindow(List<ParameterData> paramSet )
        {
            InitializeComponent();
            this.ParamSet = paramSet;
        }
    }
}
