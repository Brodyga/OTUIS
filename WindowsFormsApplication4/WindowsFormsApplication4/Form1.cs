using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ZedGraph;

namespace WindowsFormsApplication4
{
    public partial class Form1 : Form
    {
        private double MathExp(int a, int b)
        {
            return ((a + b)/2);
        }

        private double XRet(int a, int b, double x,double i)
        {
            if (x < a)
            {
                return 0;
            }
            if (x >= a && x <=b)
            {
                return (((x-a)/(b-a))*i);
            }
            if (x > b)
            {
                return i;
            }
            return 0;
        }

        private double MathExpMr(int a, int b, double x)
        {
            double sum = 0;
            int n = 0;
            for (double i = a; i < b; i += 1)
            {
                sum += XRet(a,b,x,x);
                n++;
            }
            sum = sum/n;
            return sum;
        }

        private double DispMr(int a, int b, double x)
        {
            double sum = 0;
            int n = 0;
            for (double i = a; i < b; i += 1)
            {
                sum += Math.Pow(XRet(a, b, x,x),2);
                n++;
            }
            sum = sum - MathExpMr(-1, 3, x);
            sum = sum / (n - 1);
            return sum;
        }

        private double RxxMr(int a, int b, double x)
        {
            double sum = 0;
            int n = 0;
            for (double i = a; i < b; i += 1)
            {
                double temp = 0;
                double temp1 = 0;
                temp = XRet(a, b, x,i) - MathExpMr(-1,3,i);
                temp1 = XRet(a, b, x, 10) - MathExpMr(-1, 3, 10);
                sum += temp*temp1;
                n++;
            }
            sum = sum / n;
            return sum;
        }

        private double Rxx(int a, int b)
        {
            return ((a - MathExp(a,b))*(b - MathExp(a,b)));
        }

        private double Disp(int a, int b)
        {
            return (Math.Pow((b - a), 2) / 12);
        }

        private void CreateGraph(ZedGraphControl zgc)
        {
            GraphPane myPane = zgc.GraphPane;
            // Set the titles and axis labels
            myPane.Title.Text = "Axis Cross";
            myPane.XAxis.Title.Text = "X Axis";
            myPane.YAxis.Title.Text = "Y Axis";

            // Make up some data arrays based on the Sine function
            double x, y;

            PointPairList list = new PointPairList();
            list.Clear();
            PointPairList list1 = new PointPairList();
            list1.Clear();
            PointPairList list2 = new PointPairList();
            list2.Clear();
            PointPairList list3 = new PointPairList();
            list3.Clear();
            PointPairList list4 = new PointPairList();
            list4.Clear();
            PointPairList list5 = new PointPairList();
            list5.Clear();

            for (double i = -1;i < 3; i += 0.0001)
            {
                x = i;
                y = MathExp(-1,3) *x ;
                list.Add(x, y);
                y = Disp(-1, 3) * x;
                list1.Add(x, y);
                y = Rxx(-1, 3)*x;
                list2.Add(x, y);
                y = MathExpMr(-1, 3, x);
                list3.Add(x,y);
                y = DispMr(-1, 3, x);
                list4.Add(x, y);
                y = RxxMr(-1, 3, x);
                list5.Add(x,y);
            }

            // Generate a red curve with diamond
            // symbols, and "Porsche" in the legend
            LineItem myCurve1 = myPane.AddCurve("Mx1", list, Color.Green, SymbolType.None);
            LineItem myCurve2 = myPane.AddCurve("Dx1", list1, Color.Blue, SymbolType.None);
            LineItem myCurve3 = myPane.AddCurve("Rxx1", list2, Color.Red, SymbolType.None);
            LineItem myCurve4 = myPane.AddCurve("Mx2", list3, Color.GreenYellow, SymbolType.None);
            LineItem myCurve5 = myPane.AddCurve("Dx2", list4, Color.BlueViolet, SymbolType.None);
            LineItem myCurve6 = myPane.AddCurve("Rxx2", list5, Color.DarkRed, SymbolType.None);

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
            //zgc.Invalidate();
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ZedGraphControl zedGraph = new ZedGraphControl();
            zedGraph.Location = new System.Drawing.Point(0, 0);
            zedGraph.Name = "zedGraph";
            zedGraph.Size = new System.Drawing.Size(640, 480);
            this.Controls.Add(zedGraph);
            CreateGraph(zedGraph);
        }
    }
}
