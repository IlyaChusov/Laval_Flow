using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using ZedGraph;

namespace Расчёт_течения_в_сопле_Лаваля
{
    public partial class Form3 : Form
    {
        public double X;
        List<string> Global_List_E_q_1 = new List<string>();
        List<string> Global_List_E_q_2 = new List<string>();
        List<string> Global_List_T1 = new List<string>();
        List<string> Global_List_T2 = new List<string>();
        List<double> List_Of_e = new List<double>();
        List<double> List_Of_L_sh = new List<double>();
        List<string> Global_List_L_q_1 = new List<string>();
        List<string> Global_List_L_q_2 = new List<string>();
        public bool Mode;
        public int Y;
        List<double> List_Of_X = new List<double>();

        public Form3(bool Mode1, double X1, int Y1, List<double> List_Of_X1, List<double> List_Of_L_sh1, List<string> Global_List_L_q_1_1, List<string> Global_List_L_q_2_1, List<string> Global_List_E_q_1_1, List<string> Global_List_E_q_2_1, List<string> Global_List_T1_1, List<string> Global_List_T2_1, List<double> List_Of_e1)
        {
            List_Of_X = List_Of_X1;
            X = X1;
            Global_List_E_q_1 = Global_List_E_q_1_1;
            Global_List_E_q_2 = Global_List_E_q_2_1;
            Global_List_T1 = Global_List_T1_1;
            Global_List_T2 = Global_List_T2_1;
            List_Of_e = List_Of_e1;
            List_Of_L_sh = List_Of_L_sh1;
            Global_List_L_q_1 = Global_List_L_q_1_1;
            Global_List_L_q_2 = Global_List_L_q_2_1;
            Mode = Mode1;
            Y = Y1;
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
                this.Text = "Эпсилон, Тау и Лямбда от X";
                #region Третий график
            GraphPane pane3 = new GraphPane();
            PointPairList list_E_in_jump = new PointPairList();
            PointPairList list_E_over_jump = new PointPairList();
            PointPairList list_T_in_jump = new PointPairList();
            PointPairList list_T_over_jump = new PointPairList();
            PointPairList list2_2 = new PointPairList();
            PointPairList list2_4 = new PointPairList();
            PointPairList list_Y = new PointPairList();

            for (int i = 0; i < Global_List_E_q_1.Count; i++)
            {
                list2_2.Add(List_Of_X[i], Convert.ToDouble(Global_List_E_q_2[i]));
            }

            for (int i = 0; i < Global_List_E_q_1.Count; i++)
            {
                list2_4.Add(List_Of_X[i], Convert.ToDouble(Global_List_T2[i]));
            }

            int g1 = List_Of_X.IndexOf(X);
            list_E_in_jump.Add(X, Convert.ToDouble(Global_List_E_q_2[Y]));
            list_E_in_jump.Add(X, List_Of_e[0]);

            for (int i = 0; i < List_Of_e.Count; i++)
            {
                list_E_over_jump.Add(Convert.ToDouble(List_Of_X[g1 + i]), List_Of_e[i]);
            }

            list_T_in_jump.Add(X, Convert.ToDouble(Global_List_T2[Y]));
            list_T_in_jump.Add(X, Convert.ToDouble(Global_List_E_q_1[Y]));

            for (int i = 0; i < List_Of_e.Count; i++)
            {
                list_T_over_jump.Add(Convert.ToDouble(List_Of_X[g1 + i]), Convert.ToDouble(Global_List_E_q_1[g1 + i]));
            }

            list_Y.Add(0, -1000);
            list_Y.Add(0, 1000);

            LineItem Curve3_1 = pane3.AddCurve("Зависимость Эпсилон от X (расчётный режим)", list2_2, Color.Green, SymbolType.Circle);
            LineItem Curve3_2 = pane3.AddCurve("Зависимость Эпсилон от X (в скачке уплотнения)", list_E_in_jump, Color.Aqua, SymbolType.XCross);            
            LineItem Curve3_3 = pane3.AddCurve("Зависимость Эпсилон от X (за скачком уплотнения)", list_E_over_jump, Color.Blue, SymbolType.Star);
            LineItem Curve3_4 = pane3.AddCurve("Зависимость Тау от X (расчётный режим)", list2_4, Color.LawnGreen, SymbolType.Triangle);
            LineItem Curve3_5 = pane3.AddCurve("Зависимость Тау от X (в скачке уплотнения)", list_T_in_jump, Color.Red, SymbolType.Square);
            LineItem Curve3_6 = pane3.AddCurve("Зависимость Тау от X (за скачком уплотнения)", list_T_over_jump, Color.DarkRed, SymbolType.Diamond);
            LineItem Curve1_3 = pane3.AddCurve("", list_Y, Color.Black, SymbolType.Circle);

                Curve3_1.Symbol.Size = 5;
                Curve3_2.Symbol.Size = 5;
                Curve3_3.Symbol.Size = 5;
                Curve3_4.Symbol.Size = 6;
                Curve3_5.Symbol.Size = 5;
                Curve3_6.Symbol.Size = 6;

                Curve3_1.Symbol.Fill.Color = Color.Green;
                Curve3_1.Symbol.Fill.Type = FillType.Solid;

                Curve3_2.Symbol.Fill.Color = Color.Aqua;
                Curve3_2.Symbol.Fill.Type = FillType.Solid;

                Curve3_3.Symbol.Fill.Color = Color.Blue;
                Curve3_3.Symbol.Fill.Type = FillType.Solid;

                Curve3_4.Symbol.Fill.Color = Color.LawnGreen;
                Curve3_4.Symbol.Fill.Type = FillType.Solid;

                Curve3_5.Symbol.Fill.Color = Color.Red;
                Curve3_5.Symbol.Fill.Type = FillType.Solid;

                Curve3_6.Symbol.Fill.Color = Color.DarkRed;
                Curve3_6.Symbol.Fill.Type = FillType.Solid;

                pane3.XAxis.Scale.Min = -3;
            pane3.XAxis.Scale.Max = 4;

            pane3.YAxis.Scale.Min = 0;
            pane3.YAxis.Scale.Max = 1.1;

            pane3.IsBoundedRanges = true;

            pane3.XAxis.MajorGrid.IsVisible = true;

            pane3.YAxis.MajorGrid.IsVisible = true;

            masterPane.Add(pane3);
            #endregion

                #region Четвёртый график
            GraphPane pane4 = new GraphPane();
            PointPairList list4_1 = new PointPairList();
            PointPairList list4_2 = new PointPairList();
            PointPairList list4_3 = new PointPairList();
            PointPairList list4_4 = new PointPairList();

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

            LineItem Curve4_1 = pane4.AddCurve("Зависимость Лямбда от X (расчётный режим)", list4_1, Color.Green, SymbolType.Circle);
            LineItem Curve4_2 = pane4.AddCurve("Зависимость Лямбда от X (трубка Вентури)", list4_2, Color.LawnGreen, SymbolType.Triangle);
            LineItem Curve4_3 = pane4.AddCurve("Зависимость Лямбда от X (в скачке)", list4_4, Color.Red, SymbolType.Diamond);
            LineItem Curve4_4 = pane4.AddCurve("Зависимость Лямбда от X (за скачком)", list4_3, Color.Black, SymbolType.Square);
            Curve1_3 = pane4.AddCurve("", list_Y, Color.Black, SymbolType.Circle);

                Curve4_1.Symbol.Size = 5;
                Curve4_2.Symbol.Size = 6;
                Curve4_3.Symbol.Size = 5;
                Curve4_4.Symbol.Size = 5;

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
            }

            else
            {
                this.Text = "Эпсилон и Тау от X";
                #region Третий график
                GraphPane pane3 = new GraphPane();
                PointPairList list_E_in_jump = new PointPairList();
                PointPairList list_E_over_jump = new PointPairList();
                PointPairList list_T_in_jump = new PointPairList();
                PointPairList list_T_over_jump = new PointPairList();
                PointPairList list2_2 = new PointPairList();
                PointPairList list2_4 = new PointPairList();
                PointPairList list_Y = new PointPairList();

                for (int i = 0; i < Global_List_E_q_1.Count; i++)
                {
                    list2_2.Add(List_Of_X[i], Convert.ToDouble(Global_List_E_q_2[i]));
                }

                for (int i = 0; i < Global_List_E_q_1.Count; i++)
                {
                    list2_4.Add(List_Of_X[i], Convert.ToDouble(Global_List_T2[i]));
                }

                int g1 = List_Of_X.IndexOf(X);
                list_E_in_jump.Add(X, Convert.ToDouble(Global_List_E_q_2[Y]));
                list_E_in_jump.Add(X, List_Of_e[0]);

                for (int i = 0; i < List_Of_e.Count; i++)
                {
                    list_E_over_jump.Add(Convert.ToDouble(List_Of_X[g1 + i]), List_Of_e[i]);
                }

                list_T_in_jump.Add(X, Convert.ToDouble(Global_List_T2[Y]));
                list_T_in_jump.Add(X, Convert.ToDouble(Global_List_E_q_1[Y]));

                for (int i = 0; i < List_Of_e.Count; i++)
                {
                    list_T_over_jump.Add(Convert.ToDouble(List_Of_X[g1 + i]), Convert.ToDouble(Global_List_E_q_1[g1 + i]));
                }

                list_Y.Add(0, -1000);
                list_Y.Add(0, 1000);

                LineItem Curve3_1 = pane3.AddCurve("Эпсилон от X (расчётный режим)", list2_2, Color.Green, SymbolType.Circle);
                LineItem Curve3_2 = pane3.AddCurve("Эпсилон от X (в скачке уплотнения)", list_E_in_jump, Color.Aqua, SymbolType.XCross);
                LineItem Curve3_3 = pane3.AddCurve("Эпсилон от X (за скачком уплотнения)", list_E_over_jump, Color.Blue, SymbolType.Star);
                LineItem Curve3_4 = pane3.AddCurve("Тау от X (расчётный режим)", list2_4, Color.LawnGreen, SymbolType.Triangle);
                LineItem Curve3_5 = pane3.AddCurve("Тау от X (в скачке уплотнения)", list_T_in_jump, Color.Red, SymbolType.Square);
                LineItem Curve3_6 = pane3.AddCurve("Тау от X (за скачком уплотнения)", list_T_over_jump, Color.DarkRed, SymbolType.Diamond);
                LineItem Curve1_3 = pane3.AddCurve("", list_Y, Color.Black, SymbolType.Circle);

                Curve3_1.Symbol.Size = 4;
                Curve3_2.Symbol.Size = 4;
                Curve3_3.Symbol.Size = 4;
                Curve3_4.Symbol.Size = 5;
                Curve3_5.Symbol.Size = 4;
                Curve3_6.Symbol.Size = 5;

                Curve3_1.Symbol.Fill.Color = Color.Green;
                Curve3_1.Symbol.Fill.Type = FillType.Solid;

                Curve3_2.Symbol.Fill.Color = Color.Aqua;
                Curve3_2.Symbol.Fill.Type = FillType.Solid;

                Curve3_3.Symbol.Fill.Color = Color.Blue;
                Curve3_3.Symbol.Fill.Type = FillType.Solid;

                Curve3_4.Symbol.Fill.Color = Color.LawnGreen;
                Curve3_4.Symbol.Fill.Type = FillType.Solid;

                Curve3_5.Symbol.Fill.Color = Color.Red;
                Curve3_5.Symbol.Fill.Type = FillType.Solid;

                Curve3_6.Symbol.Fill.Color = Color.DarkRed;
                Curve3_6.Symbol.Fill.Type = FillType.Solid;

                pane3.XAxis.Scale.Min = -3;
                pane3.XAxis.Scale.Max = 4;

                pane3.YAxis.Scale.Min = 0;
                pane3.YAxis.Scale.Max = 1.1;

                pane3.IsBoundedRanges = true;

                pane3.XAxis.MajorGrid.IsVisible = true;

                pane3.YAxis.MajorGrid.IsVisible = true;

                masterPane.Add(pane3);
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
