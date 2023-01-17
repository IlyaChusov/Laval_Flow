using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using ZedGraph;

namespace Расчёт_течения_в_сопле_Лаваля
{
    public partial class Form5 : Form
    {
        public double X;
        public int Y;
        List<double> List_Of_X = new List<double>();
        List<double> List_Of_L_sh = new List<double>();
        List<string> Global_List_L_q_1 = new List<string>();
        List<string> Global_List_L_q_2 = new List<string>();

        public Form5(double X1, int Y1, List<double> List_Of_X1, List<double> List_Of_L_sh1, List<string> Global_List_L_q_1_1, List<string> Global_List_L_q_2_1)
        {
            X = X1;
            Y = Y1;
            List_Of_X = List_Of_X1;
            List_Of_L_sh = List_Of_L_sh1;
            Global_List_L_q_1 = Global_List_L_q_1_1;
            Global_List_L_q_2 = Global_List_L_q_2_1;
            InitializeComponent();
            zedGraph.ContextMenuBuilder += new ZedGraphControl.ContextMenuBuilderEventHandler(zedGraph_ContextMenuBuilder);
            DrawGraph();
        }

        void zedGraph_ContextMenuBuilder(ZedGraphControl sender, ContextMenuStrip menuStrip, Point mousePt, ZedGraphControl.ContextMenuObjectState objState)
        {
            menuStrip.Items[0].Text = "Копировать";
            menuStrip.Items[1].Text = "Сохранить как картинку";
            menuStrip.Items[2].Text = "Параметры страницы";
            menuStrip.Items[3].Text = "Печать";
            menuStrip.Items[4].Text = "Показывать значения в точках";
            menuStrip.Items[6].Text = "Отмена приближения";

            menuStrip.Items.RemoveAt(7);
            menuStrip.Items.RemoveAt(5);
        }

        public void DrawGraph()
        {
            ZedGraph.MasterPane masterPane = zedGraph.MasterPane;
            masterPane.PaneList.Clear();

            this.Text = "Лямбда от X";
            #region Четвёртый график
                GraphPane pane4 = new GraphPane();
                PointPairList list4_1 = new PointPairList();
                PointPairList list4_2 = new PointPairList();
                PointPairList list4_3 = new PointPairList();
                PointPairList list4_4 = new PointPairList();
            PointPairList list_Y = new PointPairList();

            int g1 = List_Of_X.IndexOf(X);

                for (int i = 0; i < Global_List_L_q_1.Count; i++)
                {
                    list4_1.Add(List_Of_X[i], Convert.ToDouble(Global_List_L_q_2[i]));
                }

                for (int i = 0; i < Global_List_L_q_1.Count; i++)
                {
                    list4_2.Add(List_Of_X[i], Convert.ToDouble(Global_List_L_q_1[i]));
                }

                for (int i = 0; i < List_Of_L_sh.Count; i++)
                {
                    list4_3.Add(Convert.ToDouble(List_Of_X[g1 + i]), List_Of_L_sh[i]);
                }

                list4_4.Add(X, List_Of_L_sh[0]);
                list4_4.Add(X, Convert.ToDouble(Global_List_L_q_2[Y]));

            list_Y.Add(0, -1000);
            list_Y.Add(0, 1000);

            LineItem Curve4_1 = pane4.AddCurve("Зависимость Лямбда от X (расчётный режим)", list4_1, Color.Green, SymbolType.Circle);
            LineItem Curve4_2 = pane4.AddCurve("Зависимость Лямбда от X (трубка Вентури)", list4_2, Color.LawnGreen, SymbolType.Triangle);
            LineItem Curve4_3 = pane4.AddCurve("Зависимость Лямбда от X (в скачке)", list4_4, Color.Red, SymbolType.Diamond);
            LineItem Curve4_4 = pane4.AddCurve("Зависимость Лямбда от X (за скачком)", list4_3, Color.Black, SymbolType.Square);
            LineItem Curve1_3 = pane4.AddCurve("", list_Y, Color.Black, SymbolType.Circle);

            Curve4_1.Symbol.Size = 4;
            Curve4_2.Symbol.Size = 5;
            Curve4_3.Symbol.Size = 4;
            Curve4_4.Symbol.Size = 4;

            Curve4_1.Symbol.Fill.Color = Color.Green;
            Curve4_1.Symbol.Fill.Type = FillType.Solid;

            Curve4_2.Symbol.Fill.Color = Color.LawnGreen;
            Curve4_2.Symbol.Fill.Type = FillType.Solid;

            Curve4_3.Symbol.Fill.Color = Color.Red;
            Curve4_3.Symbol.Fill.Type = FillType.Solid;

            Curve4_4.Symbol.Fill.Color = Color.Black;
            Curve4_4.Symbol.Fill.Type = FillType.Solid;

            pane4.XAxis.Scale.Min = -3;
            pane4.XAxis.Scale.Max = 4;

            pane4.YAxis.Scale.Min = 0;
            pane4.YAxis.Scale.Max = 2;

            pane4.IsBoundedRanges = true;

            pane4.XAxis.MajorGrid.IsVisible = true;

            pane4.YAxis.MajorGrid.IsVisible = true;

            masterPane.Add(pane4);
                #endregion

            using (Graphics g = CreateGraphics())
            {
                masterPane.SetLayout(g, PaneLayout.SingleRow);
            }
            zedGraph.AxisChange();
            zedGraph.Invalidate();
        }
    }
}
