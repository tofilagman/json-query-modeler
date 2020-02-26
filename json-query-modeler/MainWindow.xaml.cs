using json_query_modeler.Logic;
using ScintillaNET;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace json_query_modeler
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ISqlService Sql;

        public MainWindow()
        {
            InitializeComponent();
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
            //        Scintilla rtp = new Scintilla()
            //        {
            //            Dock = DockStyle.Fill
            //        };
            //        this.splMain.Panel2.Controls.Add(rtp);
            //        rtp.Lexer = Lexer.Json;
            //        rtp.Styles[0].ForeColor = Color.Silver;
            //        rtp.Styles[7].ForeColor = Color.FromArgb(0, 128, 0);
            //        rtp.Styles[6].ForeColor = Color.FromArgb(0, 128, 0);
            //        rtp.Styles[1].ForeColor = Color.Olive;
            //        rtp.Styles[4].ForeColor = Color.Blue;
            //        rtp.Styles[2].ForeColor = Color.FromArgb(163, 21, 21);
            //        rtp.Styles[3].BackColor = Color.Pink;
            //        rtp.Styles[8].ForeColor = Color.Purple;
            //        rtp.SetProperty("fold", "1");
            //        rtp.SetProperty("fold.compact", "1");
            //        rtp.Margins[2].Type = MarginType.Symbol;
            //        rtp.Margins[2].Mask = -33554432;
            //        rtp.Margins[2].Sensitive = true;
            //        rtp.Margins[2].Width = 20;
            //        for (int i = 25; i <= 31; i++)
            //        {
            //            rtp.Markers[i].SetForeColor(SystemColors.ControlLightLight);
            //            rtp.Markers[i].SetBackColor(SystemColors.ControlDark);
            //        }
            //        rtp.Markers[30].Symbol = MarkerSymbol.BoxPlus;
            //        rtp.Markers[31].Symbol = MarkerSymbol.BoxMinus;
            //        rtp.Markers[25].Symbol = MarkerSymbol.BoxPlusConnected;
            //        rtp.Markers[27].Symbol = MarkerSymbol.TCorner;
            //        rtp.Markers[26].Symbol = MarkerSymbol.BoxMinusConnected;
            //        rtp.Markers[29].Symbol = MarkerSymbol.VLine;
            //        rtp.Markers[28].Symbol = MarkerSymbol.LCorner;
            //        rtp.AutomaticFold = AutomaticFold.Show | AutomaticFold.Click | AutomaticFold.Change;
            //        if (this.currentResult.Tables.Count > 1)
            //        {
            //            Pair p = new Pair();
            //            for (int i = 0; i < this.currentResult.Tables.Count; i++)
            //            {
            //                p.Add(string.Format("ResultSet{0}", i), new PairCollection(this.currentResult.Tables[i]));
            //            }
            //            rtp.Text = new { ResultSets = p }.ToJson(true);
            //        }
            //        else if (this.currentResult.Tables.Count == 1)
            //        {
            //            rtp.Text = new { ResultSet = new PairCollection(this.currentResult.Tables[0]) }.ToJson(true);
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
            var con = new ConnectionWindow { Sql = Sql }; 
            if (con.ShowDialog() == true)
            {
                Sql = con.Sql;
            }
        }

        private void btnParameters_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
