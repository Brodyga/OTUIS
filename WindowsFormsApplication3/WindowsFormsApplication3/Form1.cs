using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ZedGraph;

namespace WindowsFormsApplication3
{
    public partial class Form1 : Form
    {
        private double AnalisFunction(double i)
        {
            double y;
            double t = Convert.ToDouble(textBox1.Text);
            //y = 1.25*Math.Pow(Math.E, (-2*i))*(Math.Sin(2*t - 2*i) + Math.Sin(2*i));
            y = 0.62500000065037536917 * Math.Cos(2 * i) + 0.62499999829822485588 * Math.Sin(2 * t);
            return y;
        }

        private double OrigFunction(double i)
        {
            double y;
            //double t = Convert.ToDouble(textBox1.Text);
            y = Math.Cos(2 * i) / 4 + Math.Pow(Math.E, -2 * i) + Math.Sin(2 * i) / 4;
            return y;
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
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
            double t = Convert.ToDouble(textBox1.Text);

            PointPairList list1 = new PointPairList();
            list1.Clear();

            for (double i = 0; i < t; i += 0.0001)
            {
                x = i;
                y = AnalisFunction(i);
                //y = 1.25 * Math.Pow(Math.E, (-2 * t)) * (Math.Sin(2 * t - 2 * i) + Math.Sin(2 * i));
                list1.Add(x,y);
            }

            // Generate a red curve with diamond
            // symbols, and "Porsche" in the legend
            LineItem myCurve1 = myPane.AddCurve("AnalisGraph", list1, Color.Green, SymbolType.None);
            //myCurve1.Clear();

            PointPairList list2 = new PointPairList();
            list2.Clear();

            for (double i = 0; i < t; i += 0.0001)
            {
                x = i;
                y = OrigFunction(i);
                //y = 1.25 * Math.Pow(Math.E, (-2 * t)) * (Math.Sin(2 * t - 2 * i) + Math.Sin(2 * i));
                list2.Add(x, y);
            }

            LineItem myCurve2 = myPane.AddCurve("OrigGraph", list2, Color.Red, SymbolType.None);

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

        private void button1_Click(object sender, EventArgs e)
        {
            textBox2.Clear();
            textBox3.Clear();
            ZedGraphControl zedGraph = new ZedGraphControl();
            zedGraph.Location = new System.Drawing.Point(0, 0);
            zedGraph.Name = "zedGraph";
            zedGraph.Size = new System.Drawing.Size(480, 320);
            this.Controls.Add(zedGraph);
            CreateGraph(zedGraph);

            double Ea = 0;
            double Er = 0;
            double h = 0.0001;
            for (double i = 0; i < Convert.ToDouble(textBox1.Text); i += 0.0001)
            {
                Ea += Math.Pow(OrigFunction(i + h/2) - AnalisFunction(i + h/2), 2);
                Er += Math.Pow(OrigFunction(i + h / 2), 2);
            }
            Ea = Ea*h/Convert.ToDouble(textBox1.Text);
            textBox2.Paste(Convert.ToString(Ea));

            Er = Er * h /*/ Convert.ToDouble(textBox1.Text)*/;
            Er = Ea/Er;
            textBox3.Paste(Convert.ToString(Er));
        }
    }
}
