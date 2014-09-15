using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ZedGraph;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            
        }
        private static void CreateGraph(ZedGraphControl zgc, double h, double n)
        {
            GraphPane myPane = zgc.GraphPane;
            // Set the titles and axis labels
            myPane.Title.Text = "Axis Cross";
            myPane.XAxis.Title.Text = "X Axis";
            myPane.YAxis.Title.Text = "Y Axis";
 
            // Make up some data arrays based on the Sine function
            double x, y;
            PointPairList list = new PointPairList();
            for (x = -3.14159; x < 0; x += h)
            {
                y = -x;
                list.Add(x, y);
            }

            for (x = 0; x < 3.14159; x += h)
            {
                y = x * x / 3.14159;
                list.Add(x, y);
            }
 
            // Generate a red curve with diamond
            // symbols, and "Porsche" in the legend
            LineItem myCurve = myPane.AddCurve("OrigGraph", list, Color.Green, SymbolType.None);

            PointPairList list1 = new PointPairList();

            for (x = -3.14159; x < 0; x += h)
            {
                y =  Calculate(x,h,n);
                list1.Add(x,y);
            }

            for (x = 0; x < 3.14159; x += h)
            {
                y = Calculate(x, h, n);
                list1.Add(x,y);
            }

            // Generate a red curve with diamond
            // symbols, and "Porsche" in the legend
            LineItem myCurve1 = myPane.AddCurve("Graph", list1, Color.Red, SymbolType.Diamond);
 
            // Set the Y axis intersect the X axis at an X value of 0.0
            myPane.YAxis.Cross = 0.0;
            // Turn off the axis frame and all the opposite side tics
            myPane.Chart.Border.IsVisible = false;
            myPane.XAxis.MajorTic.IsOpposite = false;
            myPane.XAxis.MinorTic.IsOpposite = false;
            myPane.YAxis.MajorTic.IsOpposite = false;
            myPane.YAxis.MinorTic.IsOpposite = false;
 
            // Calculate the Axis Scale Ranges
            zgc.AxisChange();
            zgc.Refresh();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void OrGraph()
        {
            double n = Convert.ToDouble(textBox1.Text);
            double h = Convert.ToDouble(textBox2.Text);
            ZedGraphControl zedGraph = new ZedGraphControl();

            zedGraph.Location = new System.Drawing.Point(0, 0);
            zedGraph.Name = "zedGraph";
            zedGraph.Size = new System.Drawing.Size(300, 300);
            this.Controls.Add(zedGraph);
            CreateGraph(zedGraph,h,n);
        }

        private void Report()
        {
            double pi = 3.14159;
            double n = Convert.ToDouble(textBox1.Text);
            double h = Convert.ToDouble(textBox2.Text);
            textBox4.Paste("A0 = " + Convert.ToString(A0(0)) + "                                                         ");
            //int n1 = 2 * pi / h;
            for (int i = 1; i <= n; i++ )
            {
                //richTextBox1.Paste("A" + Convert.ToString(i) + " = " + Convert.ToString(Ai(i)) + "\n");
                textBox4.Paste("A" + Convert.ToString(i) + " = " + Convert.ToString(Ai(i, 0)) +"                                                        ");
                textBox4.Paste("B" + Convert.ToString(i) + " = " + Convert.ToString(Bi(i, 0)) +"                                                        ");
            }

            double I1, I2, abs, otn;
            double a0, a = 0, b = 0;
            int n1 = Convert.ToInt32(textBox3.Text);
            a0 = A0(0);
            for (int i = 1; i <= n; i++)
            {
                double temp;
                temp = Ai(i,n1);
                a += temp;
                temp = Bi(i,n1);
                b += temp;
            }
            I1 = a0 / 2 + a + b;
            a = b = 0;

            a0 = A0(n1*2);
            for (int i = 1; i <= n; i++)
            {
                double temp;
                temp = Ai(i,n1);
                a += temp;
                temp = Bi(i,n1);
                b += temp;
            }
            I2 = a0 / 2 + a + b;

            abs = (I2 - I1) / 3;
            abs = Math.Abs(abs);
            textBox4.Paste("Абсолютная погрешность" + " = " + Convert.ToString(abs) + "              ");
            otn = abs / I2;
            otn = Math.Abs(otn);
            textBox4.Paste("Относительная погрешность" + " = " + Convert.ToString(otn));
        }

        private double A0(int n)
        {
            double h = Convert.ToDouble(textBox2.Text);
            double pi = 3.14159;
            if (n != 0)
            {
                h = 2 * pi / n;
            }
            double a0 = 0, x0,x1,x,y;
            for(x0 = -pi, x1 = x0 + h; x1 < 0; x0 += h, x1 += h)
            {
                x = (x0 + x1) / 2;
                y = -x;
                a0 += y;
            }
            for(x0 = 0, x1 = x0 + h; x1 < pi; x0 += h, x1 += h)
            {
                x = (x0 + x1) / 2;
                y = x * x / pi;
                a0 += y;
            }
            a0 = a0 * h / pi;
            return a0;
        }

        private double Ai(int n, int n1)
        {
            double h = Convert.ToDouble(textBox2.Text);
            double pi = 3.14159;
            if (n1 != 0)
            {
                h = 2 * pi / n;
            }
            double a = 0, x0, x1, x, y;
            for (x0 = -pi, x1 = x0 + h; x1 < 0; x0 += h, x1 += h)
            {
                x = (x0 + x1) / 2;
                y = -x * Math.Cos(n*x);
                a += y;
            }
            for (x0 = 0, x1 = x0 + h; x1 < pi; x0 += h, x1 += h)
            {
                x = (x0 + x1) / 2;
                y = x * x / pi * Math.Cos(n*x);
                a += y;
            }
            a = a * h / pi;
            return a;
        }

        private double Bi(int n,int n1)
        {
            double h = Convert.ToDouble(textBox2.Text);
            double pi = 3.14159;
            if (n1 != 0)
            {
                h = 2 * pi / n;
            }
            double b = 0, x0, x1, x, y;
            for (x0 = -pi, x1 = x0 + h; x1 < 0; x0 += h, x1 += h)
            {
                x = (x0 + x1) / 2;
                y = -x * Math.Sin(n * x);
                b += y;
            }
            for (x0 = 0, x1 = x0 + h; x1 < pi; x0 += h, x1 += h)
            {
                x = (x0 + x1) / 2;
                y = x * x / pi * Math.Sin(n * x);
                b += y;
            }
            b = b * h / pi;
            return b;
        }

        private static double Calculate(double x, double h, double n)
        {
            double a0 = 0, a = 0, b = 0, y, res = 0;
            double pi = 3.14159;
            //double h = Convert.ToDouble(textBox2.Text);
            //double n = Convert.ToDouble(textBox1.Text);
            if (x < 0)
            {
                y = -x;
            }
            else
            {
                y = x * x / pi;
            }
            a0 = y;

            a0 = a0 * h / pi;
            a0 = a0 / 2;

            res = a0;

            for (int i = 1; i <= n; i++)
            {
                a = 0;
                b = 0;

                if (x < 0)
                {
                    y = -x * Math.Cos(i * x);
                }
                else
                {
                    y = x * x / pi * Math.Cos(i * x);
                }
                a = y;
                a = a * h / pi;

                if (x < 0)
                {
                    y = -x * Math.Sin(i * x);
                }
                else
                {
                    y = x * x / pi * Math.Sin(i * x);
                }
                b = y;
                b = b * h / pi;

                res = res + a + b;
            }
            return res;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OrGraph();
            Report();
        }
    }
}
