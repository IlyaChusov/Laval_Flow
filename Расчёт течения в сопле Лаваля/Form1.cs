using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Word = Microsoft.Office.Interop.Word;

namespace Расчёт_течения_в_сопле_Лаваля
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        #region Переменные

        public double n, P0, T0, step, x, k1, k2, k3, k4, jump, L2, E2_sh, Q2_sh, T2, E2, P2, P3, a_кр, c1, c2, c3, c3_sh, T3_1, T3_2, t2, m;
        public int i = 0, g;
        public bool Mode = false;
        public const double A = 0.0404, E = 2.71828, k = 1.4;
        public const int R = 287;

        #endregion

        public void Check()
        {
            n = Convert.ToDouble(a1.Text);
            P0 = Convert.ToDouble(a2.Text);
            T0 = Convert.ToDouble(a3.Text);
            step = Convert.ToDouble(a4.Text);
            jump = Convert.ToDouble(a5.Text);
        }

        List<string> List_Of_F = new List<string>();
        List<string> List_Of_X = new List<string>();
        List<string> List_Of_d = new List<string>();
        List<string> List_Of_r = new List<string>();
        List<string> List_Of_q = new List<string>();
        List<string> Global_List_E_q_1 = new List<string>();
        List<string> Global_List_E_q_2 = new List<string>();
        List<string> Global_List_L_q_1 = new List<string>();
        List<string> Global_List_L_q_2 = new List<string>();
        List<string> Global_List_T1 = new List<string>();
        List<string> Global_List_T2 = new List<string>();
        List<string> List_Of_E = new List<string>();
        List<string> List_Of_E1 = new List<string>();
        List<string> List_Of_E2 = new List<string>();
        List<double> List_Of_L1 = new List<double>();
        List<double> List_Of_L = new List<double>();
        List<double> List_Of_L2 = new List<double>();
        List<double> List_Of_e = new List<double>();
        List<double> List_Of_L_sh = new List<double>();

        public void Count(List<string> List_Of_X, List<string> List_Of_F)
        {
            string[] Names;
            Names = null;
            Names = new string[] { "F", "d", "r", "q", "e1", "e2", "l1", "l2", "t1", "t2" };
            string[] Names2;
            Names2 = null;
            Names2 = new string[] { "q'", "l'", "e'", "t", "e" };

            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
            dataGridView2.Rows.Clear();
            dataGridView2.Columns.Clear();
            List_Of_e.Clear();
            List_Of_L_sh.Clear();

            button1.Text = "РАССЧИТАТЬ ЗАНОВО";

            dataGridView1.ColumnCount = List_Of_F.Count;
            for (i = 0; i < List_Of_X.Count; i++)
                dataGridView1.Columns[i].Name = List_Of_X[i];
            string[] Row_Of_F = List_Of_F.ToArray();
            dataGridView1.Rows.Add(Row_Of_F);
            progressBar1.PerformStep();

            #region Расчёт d

            for (i = 0; i < List_Of_F.Count; i++)
            {
                List_Of_d.Add(Convert.ToString(Math.Round(Math.Sqrt((4 * Convert.ToDouble(List_Of_F[i])) / Math.PI), 8)));
            }

            string[] Row_Of_d = List_Of_d.ToArray();
            dataGridView1.Rows.Add(Row_Of_d);
            #endregion

            #region Расчёт r
            for (i = 0; i < List_Of_d.Count; i++)
            {
                List_Of_r.Add(Convert.ToString(Math.Round((Convert.ToDouble(List_Of_d[i]) / 2), 8)));
            }

            string[] Row_Of_r = List_Of_r.ToArray();
            dataGridView1.Rows.Add(Row_Of_r);
            #endregion

            #region Расчёт q
            int index_x = List_Of_X.IndexOf("0");
            for (i = 0; i < List_Of_d.Count; i++)
            {
                List_Of_q.Add(Convert.ToString(Math.Round((Convert.ToDouble(List_Of_F[index_x]) / Convert.ToDouble(List_Of_F[i])), 8)));
            }

            string[] Row_Of_q = List_Of_q.ToArray();
            dataGridView1.Rows.Add(Row_Of_q);
            #endregion

            #region Расчёт Ку

            List<string> List_Of_q1 = new List<string>();
            double L;
            for (L = 0.000001; L < 1; L += 0.000001)
            {
                List_Of_q1.Add(Convert.ToString(Math.Round(1.5774409 * (L * (Math.Pow(1 - (((L) * (L)) * 0.1666667), 2.5))), 8)));
            }
            progressBar1.PerformStep();

            List<string> List_Of_q2 = new List<string>();
            for (L = L; L < 2; L += 0.000001)
            {
                List_Of_q2.Add(Convert.ToString(Math.Round(1.5774409 * (L * (Math.Pow(1 - (((L) * (L)) * 0.1666667), 2.5))), 8)));
            }
            progressBar1.PerformStep();
            #endregion

            #region Расчёт Лямбды
            for (i = (List_Of_q.Count) / 2; i < List_Of_q.Count; i++)
            {
                List_Of_L1.Add(Math.Round(Convert.ToDouble(List_Of_q1.IndexOf(List_Of_q[i])), 8) / 1000000);
            }
            progressBar1.PerformStep();

            for (i = 0; i < (List_Of_q.Count) / 2; i++)
            {
                List_Of_L.Add(Math.Round(Convert.ToDouble(List_Of_q1.IndexOf(List_Of_q[i])), 8) / 1000000);
            }
            progressBar1.PerformStep();

            for (i = (List_Of_q.Count) / 2; i < List_Of_q.Count; i++)
            {
                List_Of_L2.Add(Math.Round(Convert.ToDouble(List_Of_q2.IndexOf(List_Of_q[i])), 8) / 1000000 + 1);
            }
            progressBar1.PerformStep();
            #endregion

            #region Расчёт Эпсилон
            int j;
            for (i = 0; i < List_Of_L.Count; i++)
            {
                List_Of_E.Add(Convert.ToString(Math.Round((Math.Pow((1 - (((k - 1) / (k + 1)) * (Math.Pow(List_Of_L[i], 2)))), (k / (k - 1)))), 5)));
            }
            progressBar1.PerformStep();

            for (i = 0; i < List_Of_L1.Count; i++)
            {
                List_Of_E1.Add(Convert.ToString(Math.Round((Math.Pow((1 - (((k - 1) / (k + 1)) * (Math.Pow(List_Of_L1[i], 2)))), (k / (k - 1)))), 5)));
            }
            progressBar1.PerformStep();

            for (i = 0; i < List_Of_L1.Count; i++)
            {
                List_Of_E2.Add(Convert.ToString(Math.Round((Math.Pow((1 - (((k - 1) / (k + 1)) * (Math.Pow(List_Of_L2[i], 2)))), (k / (k - 1)))), 5)));
            }
            progressBar1.PerformStep();

            // Вывод в таблицу (первая строка):
            for (j = 0, i = 0; i < (List_Of_E.Count * 2 + 1); i++)
            {
                if (i <= (List_Of_E.Count - 1))
                {
                    Global_List_E_q_1.Add(Convert.ToString(List_Of_E[i]));
                }
                else
                {
                    Global_List_E_q_1.Add(Convert.ToString(List_Of_E1[j]));
                    j++;
                }
            }
            progressBar1.PerformStep();

            string[] Global_Row_E_q_1 = Global_List_E_q_1.ToArray();
            dataGridView1.Rows.Add(Global_Row_E_q_1);

            // Вывод в таблицу (вторая строка):
            for (j = 0, i = 0; i < (List_Of_E.Count * 2 + 1); i++)
            {
                if (i <= (List_Of_E.Count - 1))
                {
                    Global_List_E_q_2.Add(Convert.ToString(List_Of_E[i]));
                }
                else
                {
                    Global_List_E_q_2.Add(Convert.ToString(List_Of_E2[j]));
                    j++;
                }
            }
            progressBar1.PerformStep();

            string[] Global_Row_E_q_2 = Global_List_E_q_2.ToArray();
            dataGridView1.Rows.Add(Global_Row_E_q_2);
            #endregion

            #region Вывод Лямбды
            // Вывод в таблицу (первая строка):
            for (j = 0, i = 0; i < (List_Of_L.Count * 2 + 1); i++)
            {
                if (i <= (List_Of_L.Count - 1))
                {
                    Global_List_L_q_1.Add(Convert.ToString(List_Of_L[i]));
                }
                else
                {
                    Global_List_L_q_1.Add(Convert.ToString(List_Of_L1[j]));
                    j++;
                }
            }
            progressBar1.PerformStep();

            string[] Global_Row_L_q_1 = Global_List_L_q_1.ToArray();
            dataGridView1.Rows.Add(Global_Row_L_q_1);

            // Вывод в таблицу (вторая строка):
            for (j = 0, i = 0; i < (List_Of_L.Count * 2 + 1); i++)
            {
                if (i <= (List_Of_L.Count - 1))
                {
                    Global_List_L_q_2.Add(Convert.ToString(List_Of_L[i]));
                }
                else
                {
                    Global_List_L_q_2.Add(Convert.ToString(List_Of_L2[j]));
                    j++;
                }
            }
            progressBar1.PerformStep();

            string[] Global_Row_L_q_2 = Global_List_L_q_2.ToArray();
            dataGridView1.Rows.Add(Global_Row_L_q_2);

            #endregion

            #region Расчёт Тау
            List<double> List_Of_T = new List<double>();
            for (i = 0; i < List_Of_L.Count; i++)
            {
                List_Of_T.Add(Math.Round(1 - (((k - 1) / (k + 1)) * (List_Of_L[i] * (List_Of_L[i]))), 8));
            }
            progressBar1.PerformStep();

            List<double> List_Of_T1 = new List<double>();
            for (i = 0; i < List_Of_L1.Count; i++)
            {
                List_Of_T1.Add(Math.Round(1 - (((k - 1) / (k + 1)) * (List_Of_L1[i] * (List_Of_L1[i]))), 8));
            }
            progressBar1.PerformStep();

            List<double> List_Of_T2 = new List<double>();
            for (i = 0; i < List_Of_L1.Count; i++)
            {
                List_Of_T2.Add(Math.Round(1 - (((k - 1) / (k + 1)) * (List_Of_L2[i] * (List_Of_L2[i]))), 8));
            }
            progressBar1.PerformStep();

            // Вывод в таблицу (первая строка):
            for (j = 0, i = 0; i < (List_Of_L.Count * 2 + 1); i++)
            {
                if (i <= (List_Of_L.Count - 1))
                {
                    Global_List_T1.Add(Convert.ToString(List_Of_T[i]));
                }
                else
                {
                    Global_List_T1.Add(Convert.ToString(List_Of_T1[j]));
                    j++;
                }
            }
            progressBar1.PerformStep();

            string[] Global_Row_T1 = Global_List_T1.ToArray();
            dataGridView1.Rows.Add(Global_Row_T1);

            // Вывод в таблицу (вторая строка):
            for (j = 0, i = 0; i < (List_Of_L.Count * 2 + 1); i++)
            {
                if (i <= (List_Of_L.Count - 1))
                {
                    Global_List_T2.Add(Convert.ToString(List_Of_T[i]));
                }
                else
                {
                    Global_List_T2.Add(Convert.ToString(List_Of_T2[j]));
                    j++;
                }
            }
            progressBar1.PerformStep();

            string[] Global_Row_T2 = Global_List_T2.ToArray();
            dataGridView1.Rows.Add(Global_Row_T2);
            #endregion

            g = List_Of_X.IndexOf(Convert.ToString(jump));
            int r = List_Of_X.Count - g;

            dataGridView2.ColumnCount = r;
            for (i = 0; i < r; i++)
            {
                dataGridView2.Columns[i].Name = List_Of_X[g + i];
            }

            #region Расчёт Лямбды 2
            L2 = Math.Round((1 / List_Of_L2[g - (List_Of_X.Count / 2)]), 8);
            textBox7.Text = Convert.ToString(L2);
            #endregion
            progressBar1.PerformStep();

            #region Расчёт Эпсилон 2 (штрих)
            E2_sh = Math.Round((Math.Pow((1 - (((k - 1) / (k + 1)) * (Math.Pow(L2, 2)))), (k / (k - 1)))), 8);
            textBox6.Text = Convert.ToString(E2_sh);
            #endregion
            progressBar1.PerformStep();

            #region Расчёт Ку 2 (штрих)
            Q2_sh = Math.Round(Math.Pow(((k + 1) / 2), (1 / (k - 1))) * Math.Pow((1 - (((k - 1) / (k + 1)) * Math.Pow(L2, 2))), (1 / (k - 1))) * L2, 8);
            textBox5.Text = Convert.ToString(Q2_sh);
            #endregion
            progressBar1.PerformStep();

            #region Расчёт Тау 2
            T2 = Math.Round((1 - (((k - 1) / (k + 1)) * (L2 * L2))), 8);
            textBox4.Text = Convert.ToString(T2);
            #endregion
            progressBar1.PerformStep();

            #region Расчёт Эпсилон 0
            g = List_Of_X.IndexOf(Convert.ToString(jump));
            double E0 = Math.Round((Math.Pow(Convert.ToDouble(Global_List_L_q_2[g]), 2) * Math.Pow((((k + 1) - ((k - 1) * Math.Pow(Convert.ToDouble(Global_List_L_q_2[g]), 2))) / ((k + 1) - ((k - 1) / Math.Pow(Convert.ToDouble(Global_List_L_q_2[g]), 2)))), (1 / (k - 1)))), 8);
            textBox1.Text = Convert.ToString(E0);
            #endregion
            progressBar1.PerformStep();

            #region Расчёт Эпсилон 2
            E2 = Math.Round((E2_sh * E0), 8);
            textBox2.Text = Convert.ToString(E2);
            #endregion
            progressBar1.PerformStep();

            #region Расчёт Ку (штрих)
            g = List_Of_X.IndexOf(Convert.ToString(jump));
            string[] Array_Of_q_sh = new string[r];
            for (i = 0; i < r; i++)
            {
                Array_Of_q_sh[i] = Convert.ToString(Math.Round((Q2_sh * Convert.ToDouble(List_Of_F[g])) / Convert.ToDouble(List_Of_F[g + i]), 8));
            }
            dataGridView2.Rows.Add(Array_Of_q_sh);
            #endregion
            progressBar1.PerformStep();

            #region Расчёт Лямбды (штрих)
            string[] Array_Of_L_sh = new string[r];
            for (i = 0; i < r; i++)
            {
                Array_Of_L_sh[i] = Convert.ToString(Math.Round((((Convert.ToDouble(List_Of_q1.IndexOf(Array_Of_q_sh[i])) / 1000000))), 8));
            }
            dataGridView2.Rows.Add(Array_Of_L_sh);
            for (i = 0; i < Array_Of_L_sh.Length; i++)
            {
                List_Of_L_sh.Add(Convert.ToDouble(Array_Of_L_sh[i]));
            }
            #endregion
            progressBar1.PerformStep();

            #region Расчёт Эпсилон (штрих)
            string[] Array_Of_e_sh = new string[r];
            for (i = 0; i < r; i++)
            {
                Array_Of_e_sh[i] = (Convert.ToString(Math.Round((Math.Pow((1 - (((k - 1) / (k + 1)) * (Math.Pow(Convert.ToDouble(Array_Of_L_sh[i]), 2)))), (k / (k - 1)))), 8)));
            }
            dataGridView2.Rows.Add(Array_Of_e_sh);
            #endregion
            progressBar1.PerformStep();

            #region Расчёт Тау
            string[] Array_Of_T = new string[r];
            for (i = 0; i < r; i++)
            {
                Array_Of_T[i] = Math.Round((1 - (((k - 1) / (k + 1)) * Math.Pow(Convert.ToDouble(Array_Of_L_sh[i]), 2))), 8).ToString();
            }
            dataGridView2.Rows.Add(Array_Of_T);
            #endregion
            progressBar1.PerformStep();

            #region Расчёт Эпсилон
            string[] Array_Of_e = new string[r];
            for (i = 0; i < r; i++)
            {
                Array_Of_e[i] = Convert.ToString(Math.Round((Convert.ToDouble(Array_Of_e_sh[i]) * E0), 8));
            }
            dataGridView2.Rows.Add(Array_Of_e);
            for (i = 0; i < Array_Of_e.Length; i++)
            {
                List_Of_e.Add(Convert.ToDouble(Array_Of_e[i]));
            }
            #endregion
            progressBar1.PerformStep();

            #region Расчёт P2 (ск)
            P2 = E2 * P0 * 1000;
            textBox3.Text = P2.ToString();
            #endregion
            progressBar1.PerformStep();

            #region Расчёт P3
            P3 = Convert.ToDouble(Array_Of_e[Array_Of_e.Length - 1]) * P0 * 1000;
            textBox8.Text = P3.ToString();
            #endregion
            progressBar1.PerformStep();

            #region Расчёт А кр
            a_кр = Math.Round((Math.Sqrt(((2 * k) / (k + 1)) * R * T0)), 1);
            textBox9.Text = Convert.ToString(a_кр);
            #endregion
            progressBar1.PerformStep();

            #region Расчёт С 3
            c3 = Math.Round((Convert.ToDouble(Global_List_L_q_2[Global_List_L_q_2.Count - 1]) * a_кр), 1);
            textBox10.Text = Convert.ToString(c3);
            #endregion
            progressBar1.PerformStep();

            #region Расчёт С 1
            c1 = Math.Round((Convert.ToDouble(Global_List_L_q_2[g]) * a_кр), 1);
            textBox11.Text = Convert.ToString(c1);
            #endregion
            progressBar1.PerformStep();

            #region Расчёт С 2
            c2 = Math.Round((L2 * a_кр), 1);
            textBox12.Text = Convert.ToString(c2);
            #endregion
            progressBar1.PerformStep();

            #region Расчёт С 3 (штрих)
            c3_sh = Math.Round((Convert.ToDouble(Array_Of_L_sh[Array_Of_L_sh.Length - 1]) * a_кр), 1);
            textBox13.Text = Convert.ToString(c3_sh);
            #endregion
            progressBar1.PerformStep();

            #region Расчёт Т 3 (1)
            T3_1 = Math.Round((List_Of_T2[List_Of_T2.Count - 1] * T0), 1);
            textBox14.Text = Convert.ToString(T3_1);
            #endregion
            progressBar1.PerformStep();

            #region Расчёт Т 2
            t2 = Math.Round((T2 * T0), 1);
            textBox15.Text = Convert.ToString(t2);
            #endregion
            progressBar1.PerformStep();

            #region Расчёт Т 3 (2)
            T3_2 = Math.Round((Convert.ToDouble(Array_Of_T[Array_Of_T.Length - 1]) * T0), 1);
            textBox16.Text = Convert.ToString(T3_2);
            #endregion
            progressBar1.PerformStep();

            #region Расчёт м
            m = Math.Round((A * ((Convert.ToDouble(List_Of_F[index_x]) * P0 * 1000000) / Math.Sqrt(T0))), 3);
            textBox17.Text = Convert.ToString(m);
            #endregion
            progressBar1.PerformStep();

            i = 0;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.IsNewRow) continue;
                row.HeaderCell.Value = Names[i];
                i++;
            }
            i = 0;
            foreach (DataGridViewRow row in dataGridView2.Rows)
            {
                if (row.IsNewRow) continue;
                row.HeaderCell.Value = Names2[i];
                i++;
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            button3.Enabled = true;
            if ((a1.Text == "0")| (a4.Text == "0,15") | (a4.Text == "0,0") | (a2.Text == "0") | (a3.Text == "0") | (a4.Text == "0") | (a5.Text == "0") | (a1.Text == "") | (a2.Text == "") | (a3.Text == "") | (a4.Text == "") | (a5.Text == ""))
            {
                MessageBox.Show("Нельзя рассчитывать со значением 0!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (Convert.ToDouble(a4.Text) < 0.002)
                {
                    MessageBox.Show("Нельзя рассчитывать с шагом меньше 0,002!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    Cursor = Cursors.WaitCursor;
                    progressBar1.Value = 0;
                    if (Convert.ToDouble(a4.Text) <= 0.02) { button2.Enabled = false; }
                    else { button2.Enabled = true; }
                    List_Of_X.Clear();
                    List_Of_F.Clear();
                    List_Of_d.Clear();
                    List_Of_r.Clear();
                    List_Of_q.Clear();
                    Global_List_E_q_1.Clear();
                    Global_List_E_q_2.Clear();
                    Global_List_L_q_1.Clear();
                    Global_List_L_q_2.Clear();
                    Global_List_T1.Clear();
                    Global_List_T2.Clear();
                    List_Of_L.Clear();
                    List_Of_L1.Clear();
                    List_Of_L2.Clear();
                    List_Of_E.Clear();
                    List_Of_E1.Clear();
                    List_Of_E2.Clear();

                    #region Первый профиль
                    if (radioButton1.Checked)
                    {
                        Check();
                        #region Расчёт F

                        // Первое уравнение 
                        for (x = -2; x < -0.5; x += 0.5)
                        {
                            List_Of_F.Add(Convert.ToString(Math.Round((((Math.Pow(n, (1.0 / (n + 1))) * (x * x) + 1)) / 1000), 6)));
                            List_Of_X.Add(Convert.ToString(x));
                        }

                        for (x = -0.5; x < -0.05; x += step)
                        {
                            List_Of_F.Add(Convert.ToString(Math.Round((((Math.Pow(n, (1.0 / (n + 1))) * (x * x) + 1)) / 1000), 6)));
                            List_Of_X.Add(Convert.ToString(x));
                        }
                        // Второе уравнение
                        for (x = 0; x < 0.45; x += step)
                        {
                            List_Of_F.Add(Convert.ToString(Math.Round(((1 + (Math.Sqrt(x) / Math.Pow((n + 1), (1.0 / (n + 1))))) / 1000), 6)));
                            List_Of_X.Add(Convert.ToString(x));
                        }

                        for (x = 0.5; x <= 3; x += 0.5)
                        {
                            List_Of_F.Add(Convert.ToString(Math.Round(((1 + (Math.Sqrt(x) / Math.Pow((n + 1), (1.0 / (n + 1))))) / 1000), 6)));
                            List_Of_X.Add(Convert.ToString(x));
                        }

                        #endregion
                        Count(List_Of_X, List_Of_F);
                    }
                    #endregion

                    #region Второй профиль
                    if (radioButton2.Checked)
                    {
                        Check();
                        #region Расчёт F

                        // Первое уравнение
                        for (x = -2; x < -0.5; x += 0.5)
                        {
                            List_Of_F.Add(Convert.ToString(Math.Round((((Math.Pow((n + 1), (1.0 / (n + 1))) * (x * x) + 1)) / 1000), 5)));
                            List_Of_X.Add(Convert.ToString(x));
                        }

                        for (x = -0.5; x < -0.05; x += step)
                        {
                            List_Of_F.Add(Convert.ToString(Math.Round((((Math.Pow((n + 1), (1.0 / (n + 1))) * (x * x) + 1)) / 1000), 5)));
                            List_Of_X.Add(Convert.ToString(x));
                        }

                        // Второе уравнение
                        for (x = 0; x < 0.45; x += step)
                        {
                            List_Of_F.Add(Convert.ToString(Math.Round(((1 + (Math.Pow(x, (Math.Sqrt(2))) / (Math.Sqrt(n + 9)))) / 1000), 5)));
                            List_Of_X.Add(Convert.ToString(x));
                        }

                        for (x = 0.5; x <= 3; x += 0.5)
                        {
                            List_Of_F.Add(Convert.ToString(Math.Round(((1 + (Math.Pow(x, (Math.Sqrt(2))) / (Math.Sqrt(n + 9)))) / 1000), 5)));
                            List_Of_X.Add(Convert.ToString(x));
                        }

                        #endregion
                        Count(List_Of_X, List_Of_F);
                    }
                    #endregion

                    #region Третий профиль
                    if (radioButton3.Checked)
                    {
                        Check();
                        #region Расчёт F

                        // Первое уравнение
                        for (x = -2; x < -0.5; x += 0.5)
                        {
                            List_Of_F.Add(Convert.ToString(Math.Round(((2 - Math.Pow(E, (-((x * x) / Math.Pow(n, (1.0 / 3.0)))))) / 1000), 5)));
                            List_Of_X.Add(Convert.ToString(x));
                        }

                        for (x = -0.5; x < -0.05; x += step)
                        {
                            List_Of_F.Add(Convert.ToString(Math.Round(((2 - Math.Pow(E, (-((x * x) / Math.Pow(n, (1.0 / 3.0)))))) / 1000), 5)));
                            List_Of_X.Add(Convert.ToString(x));
                        }

                        // Второе уравнение
                        for (x = 0; x < 0.45; x += step)
                        {
                            List_Of_F.Add(Convert.ToString(Math.Round(((1 + (Math.Sqrt(x) / Math.Pow((n + 1), (1.0 / (n + 1))))) / 1000), 5)));
                            List_Of_X.Add(Convert.ToString(x));
                        }

                        for (x = 0.5; x <= 3; x += 0.5)
                        {
                            List_Of_F.Add(Convert.ToString(Math.Round(((1 + (Math.Sqrt(x) / Math.Pow((n + 1), (1.0 / (n + 1))))) / 1000), 5)));
                            List_Of_X.Add(Convert.ToString(x));
                        }

                        #endregion
                        Count(List_Of_X, List_Of_F);
                    }
                    #endregion
                }
                Cursor = Cursors.Arrow;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult result = 0;
            if (Convert.ToDouble(a4.Text) <= 0.028)
            {
                result = MessageBox.Show("Таблица не поместится в файле! Продолжить?", "Внимание!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            }
            if (result == DialogResult.Yes | result == 0)
            {
                Cursor = Cursors.WaitCursor;
                progressBar1.Value = 0;
                int j;
                Word.Application application = new Word.Application();
                Object missing = Type.Missing;
                application.Documents.Add(ref missing, ref missing, ref missing, ref missing);
                Word.Document doc = application.ActiveDocument;
                Word.Range range = doc.Paragraphs[doc.Paragraphs.Count].Range;

                range.Font.Size = 10;
                range.PageSetup.Orientation = Word.WdOrientation.wdOrientLandscape;
                Object defaultTableBehavior = Word.WdDefaultTableBehavior.wdWord9TableBehavior;
                Object autoFitBehavior = Word.WdAutoFitBehavior.wdAutoFitWindow;
                doc.Tables.Add(range, 11, List_Of_X.Count + 1, ref missing, ref missing);

                doc.Tables[1].Cell(1, 1).Range.Text = "X";
                doc.Tables[1].Cell(2, 1).Range.Text = "F";
                doc.Tables[1].Cell(3, 1).Range.Text = "d";
                doc.Tables[1].Cell(4, 1).Range.Text = "r";
                doc.Tables[1].Cell(5, 1).Range.Text = "q";
                doc.Tables[1].Cell(6, 1).Range.Text = "E1";
                doc.Tables[1].Cell(7, 1).Range.Text = "E2";
                doc.Tables[1].Cell(8, 1).Range.Text = "L1";
                doc.Tables[1].Cell(9, 1).Range.Text = "L2";
                doc.Tables[1].Cell(10, 1).Range.Text = "T1";
                doc.Tables[1].Cell(11, 1).Range.Text = "T2";
                progressBar1.PerformStep();

                for (j = 2, i = 0; i < List_Of_X.Count; i++)
                {
                    doc.Tables[1].Cell(1, j++).Range.Text = List_Of_X[i];
                }
                progressBar1.PerformStep();

                for (j = 2, i = 0; i < List_Of_X.Count; i++)
                {
                    doc.Tables[1].Cell(2, j++).Range.Text = List_Of_F[i];
                }
                progressBar1.PerformStep();

                for (j = 2, i = 0; i < List_Of_X.Count; i++)
                {
                    doc.Tables[1].Cell(3, j++).Range.Text = List_Of_d[i];
                }
                progressBar1.PerformStep();

                for (j = 2, i = 0; i < List_Of_X.Count; i++)
                {
                    doc.Tables[1].Cell(4, j++).Range.Text = List_Of_r[i];
                }
                progressBar1.PerformStep();

                for (j = 2, i = 0; i < List_Of_X.Count; i++)
                {
                    doc.Tables[1].Cell(5, j++).Range.Text = List_Of_q[i];
                }
                progressBar1.PerformStep();

                for (j = 2, i = 0; i < List_Of_X.Count; i++)
                {
                    doc.Tables[1].Cell(6, j++).Range.Text = Global_List_E_q_1[i];
                }
                progressBar1.PerformStep();

                for (j = 2, i = 0; i < List_Of_X.Count; i++)
                {
                    doc.Tables[1].Cell(7, j++).Range.Text = Global_List_E_q_2[i];
                }
                progressBar1.PerformStep();

                for (j = 2, i = 0; i < List_Of_X.Count; i++)
                {
                    doc.Tables[1].Cell(8, j++).Range.Text = Global_List_L_q_1[i];
                }
                progressBar1.PerformStep();

                for (j = 2, i = 0; i < List_Of_X.Count; i++)
                {
                    doc.Tables[1].Cell(9, j++).Range.Text = Global_List_L_q_2[i];
                }
                progressBar1.PerformStep();

                for (j = 2, i = 0; i < List_Of_X.Count; i++)
                {
                    doc.Tables[1].Cell(10, j++).Range.Text = Global_List_T1[i];
                }
                progressBar1.PerformStep();

                for (j = 2, i = 0; i < List_Of_X.Count; i++)
                {
                    doc.Tables[1].Cell(11, j++).Range.Text = Global_List_T2[i];
                }
                progressBar1.PerformStep();

                Word.Border[] borders = new Word.Border[6];
                Word.Table tbl = doc.Tables[doc.Tables.Count];
                borders[0] = tbl.Borders[Word.WdBorderType.wdBorderLeft];
                borders[1] = tbl.Borders[Word.WdBorderType.wdBorderRight];
                borders[2] = tbl.Borders[Word.WdBorderType.wdBorderTop];
                borders[3] = tbl.Borders[Word.WdBorderType.wdBorderBottom];
                borders[4] = tbl.Borders[Word.WdBorderType.wdBorderHorizontal];
                borders[5] = tbl.Borders[Word.WdBorderType.wdBorderVertical];
                foreach (Word.Border border in borders)
                {
                    border.LineStyle = Word.WdLineStyle.wdLineStyleSingle;
                    border.Color = Word.WdColor.wdColorBlack;
                }
                application.Visible = true;
            }
            Cursor = Cursors.Arrow;
        }

        private void a4_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (a4.Text != "")
                {
                    Convert.ToDouble(a4.Text);
                    button2.Enabled = false;
                }

            }
            catch (FormatException)
            {
                MessageBox.Show("Можно вводить только цифры!");
                a4.Text = "0,0";
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            button2.Enabled = false;
            button3.Enabled = false;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            button2.Enabled = false;
            button3.Enabled = false;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            button2.Enabled = false;
            button3.Enabled = false;
        }

        private void a2_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (a2.Text != "")
                {
                    Convert.ToDouble(a2.Text);
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Можно вводить только цифры!");
                a2.Text = "1";
            }
        }

        private void a3_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (a3.Text != "")
                {
                    Convert.ToDouble(a3.Text);
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Можно вводить только цифры!");
                a3.Text = "500";
            }
        }

        private void a5_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (a5.Text != "")
                {
                    Convert.ToDouble(a5.Text);
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Можно вводить только цифры!");
                a5.Text = "2";
            }
        }

        private void a1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (a1.Text != "")
                {
                    Convert.ToDouble(a1.Text);
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Можно вводить только цифры!");
                a1.Text = "45";
            }
        }

        public Form frm2 = new Form();
        public Form frm3 = new Form();
        public Form frm4 = new Form();
        public Form frm5 = new Form();

        private void button3_Click(object sender, EventArgs e)
        {
            List<double> List_Of_X1 = new List<double>();
            for (i = 0; i < List_Of_X.Count; i++)
            {
                List_Of_X1.Add(Convert.ToDouble(List_Of_X[i]));
            }
            
            if (radioButton4.Checked)
            {
                Mode = true; // True - в два окна

                if (frm5.Visible)
                {
                    frm5.Close();
                }

                if (frm4.Visible)
                {
                    frm4.Close();
                }

                if (frm3.Visible)
                {
                    frm3.Close();
                }
                frm3 = new Form3(Mode, Convert.ToDouble(jump), g, List_Of_X1, List_Of_L_sh, Global_List_L_q_1, Global_List_L_q_2, Global_List_E_q_1, Global_List_E_q_2, Global_List_T1, Global_List_T2, List_Of_e);
                frm3.Show();

                if (frm2.Visible)
                {
                    frm2.Close();
                }
                frm2 = new Form2(Mode, g, List_Of_X1, List_Of_r, Global_List_E_q_1, Global_List_E_q_2, Global_List_T1, Global_List_T2, List_Of_q);
                frm2.Show();
            }

            else
            {
                Mode = false;

                if (frm5.Visible)
                {
                    frm5.Close();
                }
                frm5 = new Form5(Convert.ToDouble(jump), g, List_Of_X1, List_Of_L_sh, Global_List_L_q_1, Global_List_L_q_2);
                frm5.Show();

                if (frm3.Visible)
                {
                    frm3.Close();
                }
                frm3 = new Form3(Mode, Convert.ToDouble(jump), g, List_Of_X1, List_Of_L_sh, Global_List_L_q_1, Global_List_L_q_2, Global_List_E_q_1, Global_List_E_q_2, Global_List_T1, Global_List_T2, List_Of_e);
                frm3.Show();

                if (frm4.Visible)
                {
                    frm4.Close();
                }
                frm4 = new Form4(List_Of_X1, Global_List_E_q_1, Global_List_E_q_2, Global_List_T1, Global_List_T2, List_Of_q);
                frm4.Show();

                if (frm2.Visible)
                {
                    frm2.Close();
                }
                frm2 = new Form2(Mode, g, List_Of_X1, List_Of_r, Global_List_E_q_1, Global_List_E_q_2, Global_List_T1, Global_List_T2, List_Of_q);
                frm2.Show();
            }
        }
    }
}