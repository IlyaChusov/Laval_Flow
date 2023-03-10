using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using ZedGraph;

namespace Расчёт_течения_в_сопле_Лаваля
{
    public partial class Form2 : Form
    {
        List<double> List_Of_X = new List<double>();
        List<string> List_Of_r = new List<string>();
        List<string> Global_List_E_q_1 = new List<string>();
        List<string> Global_List_E_q_2 = new List<string>();
        List<string> Global_List_T1 = new List<string>();
        List<string> Global_List_T2 = new List<string>();
        List<string> List_Of_q = new List<string>();
        public bool Mode;
        public int Y;

        public Form2(bool Mode1, int Y1, List<double> List_Of_X1, List<string> List_Of_r2, List<string> Global_List_E_q_1_1, List<string> Global_List_E_q_2_1, List<string> Global_List_T1_1, List<string> Global_List_T2_1, List<string> List_Of_q2)
        {
            List_Of_r = List_Of_r2;
            List_Of_X = List_Of_X1;
            Global_List_E_q_1 = Global_List_E_q_1_1;
            Global_List_E_q_2 = Global_List_E_q_2_1;
            Global_List_T1 = Global_List_T1_1;
            Global_List_T2 = Global_List_T2_1;
            List_Of_q = List_Of_q2;
            Y = Y1;
            Mode = Mode1;
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

            if (Mode)
            {
                Text = "Профиль сопла Лаваля и Эпсилон, Тау, Ку от x";
                #region Первый график
                GraphPane pane1 = new GraphPane();
                PointPairList list1_1 = new PointPairList();
                PointPairList list1_2 = new PointPairList();
                PointPairList list_Y = new PointPairList();

                for (int i = 0; i < List_Of_r.Count; i++)
                {
                    list1_1.Add(List_Of_X[i], Convert.ToDouble(List_Of_r[i]));
                }

                for (int i = 0; i < List_Of_r.Count; i++)
                {
                    list1_2.Add(List_Of_X[i], -(Convert.ToDouble(List_Of_r[i])));
                }

                list_Y.Add(0, -1000);
                list_Y.Add(0, 1000);

                LineItem Curve1_1 = pane1.AddCurve("", list1_1, Color.Blue, SymbolType.Circle);
                LineItem Curve1_2 = pane1.AddCurve("", list1_2, Color.Blue, SymbolType.Circle);
                LineItem Curve1_3 = pane1.AddCurve("", list_Y, Color.Black, SymbolType.Circle);

                Curve1_1.Symbol.Size = 5;
                Curve1_2.Symbol.Size = 5;

                Curve1_1.Symbol.Fill.Color = Color.Blue;
                Curve1_1.Symbol.Fill.Type = FillType.Solid;

                Curve1_2.Symbol.Fill.Color = Color.Blue;
                Curve1_2.Symbol.Fill.Type = FillType.Solid;

                pane1.XAxis.Scale.Min = -3;
                pane1.XAxis.Scale.Max = 4;

                pane1.YAxis.Scale.Min = -0.08;
                pane1.YAxis.Scale.Max = 0.08;

                pane1.IsBoundedRanges = true;

                pane1.XAxis.MajorGrid.IsVisible = true;

                pane1.YAxis.MajorGrid.IsVisible = true;

                masterPane.Add(pane1);
                #endregion

                #region Второй график
                GraphPane pane2 = new GraphPane();
                PointPairList list2_1 = new PointPairList();
                PointPairList list2_2 = new PointPairList();
                PointPairList list2_3 = new PointPairList();
                PointPairList list2_4 = new PointPairList();
                PointPairList list2_5 = new PointPairList();

                for (int i = 0; i < Global_List_E_q_1.Count; i++)
                {
                    list2_1.Add(List_Of_X[i], Convert.ToDouble(Global_List_E_q_1[i]));
                }

                for (int i = 0; i < Global_List_E_q_1.Count; i++)
                {
                    list2_2.Add(List_Of_X[i], Convert.ToDouble(Global_List_E_q_2[i]));
                }

                for (int i = 0; i < Global_List_E_q_1.Count; i++)
                {
                    list2_3.Add(List_Of_X[i], Convert.ToDouble(Global_List_T1[i]));
                }

                for (int i = 0; i < Global_List_E_q_1.Count; i++)
                {
                    list2_4.Add(List_Of_X[i], Convert.ToDouble(Global_List_T2[i]));
                }

                for (int i = 0; i < Global_List_E_q_1.Count; i++)
                {
                    list2_5.Add(List_Of_X[i], Convert.ToDouble(List_Of_q[i]));
                }

                LineItem Curve2_2 = pane2.AddCurve("Зависимость Эпсилон от X (расчётный режим)", list2_2, Color.Green, SymbolType.Circle);
                LineItem Curve2_1 = pane2.AddCurve("Зависимость Эпсилон от X (трубка Вентури))", list2_1, Color.LawnGreen, SymbolType.XCross);
                LineItem Curve2_4 = pane2.AddCurve("Зависимость Тау от X (расчётный режим)", list2_4, Color.DarkBlue, SymbolType.Star);
                LineItem Curve2_3 = pane2.AddCurve("Зависимость Тау от X (трубка Вентури)", list2_3, Color.Blue, SymbolType.Triangle);
                LineItem Curve2_5 = pane2.AddCurve("Зависимость Ку от X", list2_5, Color.Red, SymbolType.Diamond);
                Curve1_3 = pane2.AddCurve("", list_Y, Color.Black, SymbolType.Circle);

                Curve2_2.Symbol.Size = 5;
                Curve2_1.Symbol.Size = 5;
                Curve2_4.Symbol.Size = 5;
                Curve2_3.Symbol.Size = 5;
                Curve2_5.Symbol.Size = 6;

                Curve2_2.Symbol.Fill.Color = Color.Green;
                Curve2_2.Symbol.Fill.Type = FillType.Solid;

                Curve2_1.Symbol.Fill.Color = Color.LawnGreen;
                Curve2_1.Symbol.Fill.Type = FillType.Solid;

                Curve2_4.Symbol.Fill.Color = Color.DarkBlue;
                Curve2_4.Symbol.Fill.Type = FillType.Solid;

                Curve2_3.Symbol.Fill.Color = Color.Blue;
                Curve2_3.Symbol.Fill.Type = FillType.Solid;

                Curve2_5.Symbol.Fill.Color = Color.Red;
                Curve2_5.Symbol.Fill.Type = FillType.Solid;

                pane2.XAxis.Scale.Min = -3;
                pane2.XAxis.Scale.Max = 4;

                pane2.YAxis.Scale.Min = 0;
                pane2.YAxis.Scale.Max = 1.1;

                pane2.IsBoundedRanges = true;

                pane2.XAxis.MajorGrid.IsVisible = true;

                pane2.YAxis.MajorGrid.IsVisible = true;

                masterPane.Add(pane2);
                #endregion
            }

            else
            {
                Text = "Профиль сопла Лаваля";
                #region Первый график
                GraphPane pane1 = new GraphPane();
                PointPairList list1_1 = new PointPairList();
                PointPairList list1_2 = new PointPairList();
                PointPairList list_Y = new PointPairList();

                for (int i = 0; i < List_Of_r.Count; i++)
                {
                    list1_1.Add(List_Of_X[i], Convert.ToDouble(List_Of_r[i]));
                }

                for (int i = 0; i < List_Of_r.Count; i++)
                {
                    list1_2.Add(List_Of_X[i], -(Convert.ToDouble(List_Of_r[i])));
                }

                list_Y.Add(0, -1000);
                list_Y.Add(0, 1000);

                LineItem Curve1_1 = pane1.AddCurve("", list1_1, Color.Blue, SymbolType.Circle);
                LineItem Curve1_2 = pane1.AddCurve("", list1_2, Color.Blue, SymbolType.Circle);
                LineItem Curve1_3 = pane1.AddCurve("", list_Y, Color.Black, SymbolType.Circle);

                Curve1_1.Symbol.Size = 4;
                Curve1_2.Symbol.Size = 4;

                Curve1_1.Symbol.Fill.Color = Color.Blue;
                Curve1_1.Symbol.Fill.Type = FillType.Solid;

                Curve1_2.Symbol.Fill.Color = Color.Blue;
                Curve1_2.Symbol.Fill.Type = FillType.Solid;

                pane1.XAxis.Scale.Min = -3;
                pane1.XAxis.Scale.Max = 4;

                pane1.YAxis.Scale.Min = -0.06;
                pane1.YAxis.Scale.Max = 0.06;

                pane1.IsBoundedRanges = true;

                pane1.XAxis.MajorGrid.IsVisible = true;

                pane1.YAxis.MajorGrid.IsVisible = true;

                masterPane.Add(pane1);
                #endregion
            }

            using (Graphics g = CreateGraphics())
            {
                masterPane.SetLayout(g, PaneLayout.SingleRow);
            }
            zedGraph.AxisChange();
            zedGraph.Invalidate();
        }
    }
}