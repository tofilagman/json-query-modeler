using json_query_modeler.Logic;
using ScintillaNET;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using z.Data;

namespace json_query_modeler
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private ISqlService Sql;
        private DataSet CurSet = new DataSet();
        private List<ParameterData> ParamSet = new List<ParameterData>();

        public MainWindow()
        {
            InitializeComponent();
            Width = SystemParameters.WorkArea.Width / 2;
            Height = SystemParameters.WorkArea.Height / 2;
            InitiateSqlEditor();
        }

        private void InitiateSqlEditor()
        {
            this.TextArea.TextChanged += new EventHandler(this.OnTextChanged);
            this.TextArea.WrapMode = WrapMode.None;
            this.TextArea.IndentationGuides = IndentView.LookBoth;
            this.InitColors();
            this.InitSyntaxColoring();
            this.InitNumberMargin();
            this.InitBookmarkMargin();
            this.InitCodeFolding();
            //this.InitDragDropFile();
            //HotKeyManager.AddHotKey(this, () => this.btnExecute_Click(this, EventArgs.Empty), Keys.F5, false, false, false);

            trMain.Lexer = Lexer.Json;
            trMain.Styles[0].ForeColor = Color.Silver;
            trMain.Styles[7].ForeColor = Color.FromArgb(0, 128, 0);
            trMain.Styles[6].ForeColor = Color.FromArgb(0, 128, 0);
            trMain.Styles[1].ForeColor = Color.Olive;
            trMain.Styles[4].ForeColor = Color.Blue;
            trMain.Styles[2].ForeColor = Color.FromArgb(163, 21, 21);
            trMain.Styles[3].BackColor = Color.Pink;
            trMain.Styles[8].ForeColor = Color.Purple;
            trMain.SetProperty("fold", "1");
            trMain.SetProperty("fold.compact", "1");
            trMain.Margins[2].Type = MarginType.Symbol;
            trMain.Margins[2].Mask = uint.MinValue;
            trMain.Margins[2].Sensitive = true;
            trMain.Margins[2].Width = 20;
            for (int i = 25; i <= 31; i++)
            {
                trMain.Markers[i].SetForeColor(System.Drawing.SystemColors.ControlLightLight);
                trMain.Markers[i].SetBackColor(System.Drawing.SystemColors.ControlDark);
            }
            trMain.Markers[30].Symbol = MarkerSymbol.BoxPlus;
            trMain.Markers[31].Symbol = MarkerSymbol.BoxMinus;
            trMain.Markers[25].Symbol = MarkerSymbol.BoxPlusConnected;
            trMain.Markers[27].Symbol = MarkerSymbol.TCorner;
            trMain.Markers[26].Symbol = MarkerSymbol.BoxMinusConnected;
            trMain.Markers[29].Symbol = MarkerSymbol.VLine;
            trMain.Markers[28].Symbol = MarkerSymbol.LCorner;
            trMain.AutomaticFold = AutomaticFold.Show | AutomaticFold.Click | AutomaticFold.Change;
            //trMain.ReadOnly = true; 
        }

        private void InitColors()
        {
            this.TextArea.SetSelectionBackColor(true, IntToColor(1133980));
        }

        private void InitSyntaxColoring()
        {
            this.TextArea.StyleResetDefault();
            this.TextArea.Styles[32].Font = "Courier New";
            this.TextArea.Styles[32].Size = 10;
            this.TextArea.StyleClearAll();
            this.TextArea.Lexer = Lexer.Sql;
            this.TextArea.Styles[33].ForeColor = Color.FromArgb(255, 128, 128, 128);
            this.TextArea.Styles[33].BackColor = Color.FromArgb(255, 228, 228, 228);
            this.TextArea.Styles[1].ForeColor = Color.Green;
            this.TextArea.Styles[2].ForeColor = Color.Green;
            this.TextArea.Styles[15].ForeColor = Color.Green;
            this.TextArea.Styles[4].ForeColor = Color.Maroon;
            this.TextArea.Styles[5].ForeColor = Color.Blue;
            this.TextArea.Styles[16].ForeColor = Color.Fuchsia;
            this.TextArea.Styles[19].ForeColor = Color.Gray;
            this.TextArea.Styles[20].ForeColor = Color.FromArgb(255, 0, 128, 192);
            this.TextArea.Styles[6].ForeColor = Color.Red;
            this.TextArea.Styles[7].ForeColor = Color.Red;
            this.TextArea.Styles[10].ForeColor = Color.Black;
            this.TextArea.SetKeywords(0, "add alter as authorization backup begin bigint binary bit break browse bulk by cascade case catch check checkpoint close clustered column commit compute constraint containstable continue create current cursor cursor database date datetime datetime2 datetimeoffset dbcc deallocate decimal declare default delete deny desc disk distinct distributed double drop dump else end errlvl escape except exec execute exit external fetch file fillfactor float for foreign freetext freetexttable from full function goto grant group having hierarchyid holdlock identity identity_insert identitycol if image index insert int intersect into key kill lineno load merge money national nchar nocheck nocount nolock nonclustered ntext numeric nvarchar of off offsets on open opendatasource openquery openrowset openxml option order over percent plan precision primary print proc procedure public raiserror read readtext real reconfigure references replication restore restrict return revert revoke rollback rowcount rowguidcol rule save schema securityaudit select set setuser shutdown smalldatetime smallint smallmoney sql_variant statistics table table tablesample text textsize then time timestamp tinyint to top tran transaction trigger truncate try union unique uniqueidentifier update updatetext use user values varbinary varchar varying view waitfor when where while with writetext xml go ");
            this.TextArea.SetKeywords(1, "ascii cast char charindex ceiling coalesce collate contains convert current_date current_time current_timestamp current_user floor isnull max min nullif object_id session_user substring system_user tsequal ");
            this.TextArea.SetKeywords(4, "all and any between cross exists in inner is join left like not null or outer pivot right some unpivot ( ) * ");
            this.TextArea.SetKeywords(5, "sys objects sysobjects ");
        }

        private void InitNumberMargin()
        {
            this.TextArea.Styles[33].BackColor = IntToColor(2760988);
            this.TextArea.Styles[33].ForeColor = IntToColor(12040119);
            this.TextArea.Styles[37].ForeColor = IntToColor(12040119);
            this.TextArea.Styles[37].BackColor = IntToColor(2760988);
            Margin nums = this.TextArea.Margins[1];
            nums.Width = 30;
            nums.Type = MarginType.Number;
            nums.Sensitive = true;
            nums.Mask = 0;
            this.TextArea.MarginClick += new EventHandler<MarginClickEventArgs>(this.TextArea_MarginClick);
        }

        public static Color IntToColor(int rgb)
        {
            Color color = Color.FromArgb(rgb); //Color.FromArgb(255, (int)(rgb >> 16), (int)(rgb >> 8), (int)rgb);
            return color;
        }

        private void InitBookmarkMargin()
        {
            Margin margin = this.TextArea.Margins[2];
            margin.Width = 20;
            margin.Sensitive = true;
            margin.Type = MarginType.Symbol;
            margin.Mask = 4;
            Marker marker = this.TextArea.Markers[2];
            marker.Symbol = MarkerSymbol.Circle;
            marker.SetBackColor(IntToColor(16711739));
            marker.SetForeColor(IntToColor(0));
            marker.SetAlpha(100);
        }

        private void InitCodeFolding()
        {
            this.TextArea.SetFoldMarginColor(true, IntToColor(2760988));
            this.TextArea.SetFoldMarginHighlightColor(true, IntToColor(2760988));
            this.TextArea.SetProperty("fold", "1");
            this.TextArea.SetProperty("fold.compact", "1");
            this.TextArea.Margins[3].Type = MarginType.Symbol;
            //this.TextArea.Margins[3].Mask = Convert.ToUInt32(-33554432);
            this.TextArea.Margins[3].Sensitive = true;
            this.TextArea.Margins[3].Width = 20;
            for (int i = 25; i <= 31; i++)
            {
                this.TextArea.Markers[i].SetForeColor(IntToColor(2760988));
                this.TextArea.Markers[i].SetBackColor(IntToColor(12040119));
            }
            this.TextArea.Markers[30].Symbol = MarkerSymbol.CirclePlus;
            this.TextArea.Markers[31].Symbol = MarkerSymbol.CircleMinus;
            this.TextArea.Markers[25].Symbol = MarkerSymbol.CirclePlusConnected;
            this.TextArea.Markers[27].Symbol = MarkerSymbol.TCorner;
            this.TextArea.Markers[26].Symbol = MarkerSymbol.CircleMinusConnected;
            this.TextArea.Markers[29].Symbol = MarkerSymbol.VLine;
            this.TextArea.Markers[28].Symbol = MarkerSymbol.LCorner;
            this.TextArea.AutomaticFold = AutomaticFold.Show | AutomaticFold.Click | AutomaticFold.Change;
        }

        private void OnTextChanged(object sender, EventArgs e)
        {
        }

        private void TextArea_MarginClick(object sender, MarginClickEventArgs e)
        {
            if (e.Margin == 2)
            {
                var line = this.TextArea.Lines[this.TextArea.LineFromPosition(e.Position)];
                if ((line.MarkerGet() & 4) == 0)
                {
                    line.MarkerAdd(2);
                }
                else
                {
                    line.MarkerDelete(2);
                }
            }
        }

        private void showResult()
        {
            //if (this.currentResult != null)
            //{
            //    this.splMain.Panel2.Controls.Clear();
            //    if (!this.btnGrid.Checked)
            //    {
            //        Scintilla trMain = new Scintilla()
            //        {
            //            Dock = DockStyle.Fill
            //        };
            //        this.splMain.Panel2.Controls.Add(trMain);
            //        trMain.Lexer = Lexer.Json;
            //        trMain.Styles[0].ForeColor = Color.Silver;
            //        trMain.Styles[7].ForeColor = Color.FromArgb(0, 128, 0);
            //        trMain.Styles[6].ForeColor = Color.FromArgb(0, 128, 0);
            //        trMain.Styles[1].ForeColor = Color.Olive;
            //        trMain.Styles[4].ForeColor = Color.Blue;
            //        trMain.Styles[2].ForeColor = Color.FromArgb(163, 21, 21);
            //        trMain.Styles[3].BackColor = Color.Pink;
            //        trMain.Styles[8].ForeColor = Color.Purple;
            //        trMain.SetProperty("fold", "1");
            //        trMain.SetProperty("fold.compact", "1");
            //        trMain.Margins[2].Type = MarginType.Symbol;
            //        trMain.Margins[2].Mask = -33554432;
            //        trMain.Margins[2].Sensitive = true;
            //        trMain.Margins[2].Width = 20;
            //        for (int i = 25; i <= 31; i++)
            //        {
            //            trMain.Markers[i].SetForeColor(SystemColors.ControlLightLight);
            //            trMain.Markers[i].SetBackColor(SystemColors.ControlDark);
            //        }
            //        trMain.Markers[30].Symbol = MarkerSymbol.BoxPlus;
            //        trMain.Markers[31].Symbol = MarkerSymbol.BoxMinus;
            //        trMain.Markers[25].Symbol = MarkerSymbol.BoxPlusConnected;
            //        trMain.Markers[27].Symbol = MarkerSymbol.TCorner;
            //        trMain.Markers[26].Symbol = MarkerSymbol.BoxMinusConnected;
            //        trMain.Markers[29].Symbol = MarkerSymbol.VLine;
            //        trMain.Markers[28].Symbol = MarkerSymbol.LCorner;
            //        trMain.AutomaticFold = AutomaticFold.Show | AutomaticFold.Click | AutomaticFold.Change;
            //        if (this.currentResult.Tables.Count > 1)
            //        {
            //            Pair p = new Pair();
            //            for (int i = 0; i < this.currentResult.Tables.Count; i++)
            //            {
            //                p.Add(string.Format("ResultSet{0}", i), new PairCollection(this.currentResult.Tables[i]));
            //            }
            //            trMain.Text = new { ResultSets = p }.ToJson(true);
            //        }
            //        else if (this.currentResult.Tables.Count == 1)
            //        {
            //            trMain.Text = new { ResultSet = new PairCollection(this.currentResult.Tables[0]) }.ToJson(true);
            //        }
            //    }
            //    else
            //    {
            //        TabControl tb = new TabControl()
            //        {
            //            Dock = DockStyle.Fill
            //        };
            //        this.splMain.Panel2.Controls.Add(tb);
            //        for (int i = 0; i < this.currentResult.Tables.Count; i++)
            //        {
            //            TabPage tp = new TabPage(string.Format("Result {0}", i));
            //            tp.Controls.Clear();
            //            DataGridView dg = new DataGridView()
            //            {
            //                Dock = DockStyle.Fill
            //            };
            //            tp.Controls.Add(dg);
            //            tb.TabPages.Add(tp);
            //            dg.ReadOnly = true;
            //            dg.DataSource = this.currentResult.Tables[i];
            //            dg.Refresh();
            //            Label status = new Label()
            //            {
            //                Text = (this.currentResult.Tables[i].Rows.Count == 0 ? "No Records Found" : string.Format("{0} Records Found", this.currentResult.Tables[i].Rows.Count)),
            //                Dock = DockStyle.Bottom
            //            };
            //            status.BringToFront();
            //            status.TextAlign = ContentAlignment.MiddleRight;
            //            tp.Controls.Add(status);
            //        }
            //    }
            //}
        }

        private void btnServerConnection_Click(object sender, RoutedEventArgs e)
        {
            var con = new ConnectionWindow(this.Sql);
            con.Owner = this;
            if (con.ShowDialog() == true)
            {
                this.Sql = con.Sql;
                this.txtStatus.Text = this.Sql.GetDisplayConnection;
            }
        }

        private void btnParameters_Click(object sender, RoutedEventArgs e)
        {
            var con = new ParameterWindow(ParamSet);
            con.Owner = this;
            if (con.ShowDialog() == true)
            {
                this.ParamSet = con.ParamSet;
            }
        }

        private void btnExecute_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Sql == null) throw new Exception("Connection is not yet establish");
                if (string.IsNullOrWhiteSpace(TextArea.Text)) throw new Exception("Cannot execute an empty query");

                CurSet = Sql.Query(TextArea.Text);
                PlotSet();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void PlotSet(string display = null)
        {
            if (tbMain == null) return;

            switch (display ?? cbDisplay.Text)
            {
                case "Grid":
                    var TabDatas = new ObservableCollection<TabData>();

                    for (var i = 0; i < CurSet.Tables.Count; i++)
                    {
                        TabDatas.Add(new TabData
                        {
                            Header = $"ResultSet{i} ({CurSet.Tables[i].Rows.Count})",
                            Data = CurSet.Tables[i]
                        });
                    }

                    tbMain.ItemsSource = TabDatas;
                    tbMain.SelectedIndex = 0;
                    hstMain.Visibility = Visibility.Collapsed;
                    tbMain.Visibility = Visibility.Visible;
                    break;
                case "Json":
                    if (CurSet.Tables.Count > 1)
                    {
                        Pair p = new Pair();
                        for (int i = 0; i < CurSet.Tables.Count; i++)
                        {
                            p.Add(string.Format("ResultSet{0}", i), new PairCollection(CurSet.Tables[i]));
                        }
                        trMain.Text = new {
                            ResultSets = p,
                            Parameters  = this.FormattedParamSet()
                        }.ToJson(true);
                    }
                    else if (CurSet.Tables.Count == 1)
                    {
                        trMain.Text = new { 
                            ResultSet = new PairCollection(CurSet.Tables[0]),
                            Parameters = this.FormattedParamSet()
                        }.ToJson(true);
                    }
                    hstMain.Visibility = Visibility.Visible;
                    tbMain.Visibility = Visibility.Collapsed;
                    break;
            }
        }

        private void cbDisplay_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                PlotSet((e.AddedItems[0] as ComboBoxItem).Content as string);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            HotKeyHost hotKeyHost = new HotKeyHost((HwndSource)HwndSource.FromVisual(App.Current.MainWindow));
            hotKeyHost.AddHotKey(new CustomHotKey("Execute", Key.F5, ModifierKeys.None, true, new EventHandler(delegate { btnExecute_Click(sender, e); })));
            this.LoadParamSet();
        }

        private void LoadParamSet()
        {
            try
            {
                var ht = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "JsonQueryModeler");
                var kj = Path.Combine(ht, "appdata.json");
                
                if (!File.Exists(kj))
                {
                    var _assembly = Assembly.GetExecutingAssembly();
                    using (var _textStreamReader = new StreamReader(_assembly.GetManifestResourceStream("json_query_modeler.ParamSet.json")))
                    {
                        var hh = _textStreamReader.ReadToEnd();
                        ParamSet = hh.ToObject<List<ParameterData>>();

                        if (!Directory.Exists(ht))
                            Directory.CreateDirectory(ht);

                        File.WriteAllText(kj, hh);
                    }
                }
                else
                {
                    var hj = File.ReadAllText(kj);
                    ParamSet = hj.ToObject<List<ParameterData>>();
                }
            } 
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private Pair FormattedParamSet()
        {
            var pr = new Pair();
            foreach(var prs in this.ParamSet)
            {
                pr.Add(prs.Name, prs.Value);
            }
            return pr;
        }
    }
}
