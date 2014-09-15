using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ZedGraph;
using System.Numerics;

namespace WindowsFormsApplication2
{
    public partial class Form1 : Form
    {
        private static Complex function(int t, double z)
        {
            Complex result = new Complex(0, 0);
            Complex E1Temp = new Complex(-1 + 2 * t, -t * z + 2 * t); 
            Complex E2Temp = new Complex(-1 + 2 * t, -t * z - 2 * t);
            Complex I = new Complex(0, 1);
            Complex Num1Temp = new Complex(-2, 2);
            Complex Num2Temp = new Complex(2, 2);
            Complex Num3Temp = new Complex(Math.Pow(2,0.5), 0);
            Complex ZTemp = new Complex(z, 0);
            Complex Num4Temp = new Complex(4,0);
            Complex Num8Temp = new Complex(8,0);
            Complex Pi = new Complex(Math.Pow(Math.PI,0.5),0);

            result = -(Num3Temp*(Num1Temp*Complex.Exp(E1Temp)-ZTemp*Complex.Exp(E2Temp)*I+Num2Temp*Complex.Exp(E2Temp)+ZTemp*Complex.Exp(E1Temp)*I)*I) / (Num4Temp*Pi*(ZTemp*ZTemp-Num8Temp+Num4Temp*ZTemp*I));

            return result;
        }

        private double funcA(double z2)
        {
            double res = 0;
            double z1 = 0;
            //double z2 = Convert.ToDouble(textBox2.Text);
            for (z1 = 0; z1 <= z2; z1 += 0.0001 )
            {
                Complex temp = function(Convert.ToInt32(textBox2.Text), z1) - function(Convert.ToInt32(textBox1.Text), z1);
                double temp1 = Math.Pow(temp.Real, 2) + Math.Pow(temp.Imaginary, 2);
                res = res + temp1;
            }
            res = res * 0.0001;
            return res;
        }

        private double comp()
        {
            double fullA = funcA(Convert.ToDouble(textBox2.Text));
            double Ai = 0;
            double res = 0;
            double z = 1;
            for (Ai = 0; res < 0.9; )
            {
                Ai = funcA(z);
                res = Ai / fullA;
                if (res >= 0.9)
                {
                    break;
                }
                z = z * 2;
            }
            double z1 = z;
            z = (z + z / 2) / 2;
            while (true)
            {
                Ai = funcA(z);
                res = Ai / fullA;
                if (res >= 0.9)
                {
                    return z;
                    //z = z1 / 2;
                }
                else
                {
                    z = (z1 + z) / 2;
                }
            }
            return 0;
        }

        private void Graph()
        {
            ZedGraphControl zedGraph = new ZedGraphControl();
            zedGraph.Location = new System.Drawing.Point(0, 0);
            zedGraph.Name = "zedGraph";
            zedGraph.Size = new System.Drawing.Size(480 , 320);
            this.Controls.Add(zedGraph);
            CreateGraph(zedGraph);
        }

        public Form1()
        {
            InitializeComponent();
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
            Complex temp;

            PointPairList list = new PointPairList();
            int t1 = Convert.ToInt32(textBox1.Text);
            int t2 = Convert.ToInt32(textBox2.Text);

            for (double i = -10; i <= 10;i+=0.0001 )
            {
                x = i;
                temp = function(t2,x) - function(t1,x);
                y = Math.Pow(Math.Pow(temp.Real, 2) + Math.Pow(temp.Imaginary, 2),0.5);
                list.Add(x, y);
            }

            // Generate a red curve with diamond
            // symbols, and "Porsche" in the legend
            LineItem myCurve = myPane.AddCurve("Graph", list, Color.Green, SymbolType.None);

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

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Graph();
            textBox3.Paste("От 0 до " + Convert.ToString(comp()));
        }
    }
}
