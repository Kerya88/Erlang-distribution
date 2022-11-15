using System;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace WindowsFormsApplication16
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        double lambda;
        double m;
        double y;
        int n = 1000;

        private double Fact(double x)
        {
            if (x == 0)
                return 1;
            else
                return x * Fact(x - 1);
        }

        private void ButtonOk_Click(object sender, EventArgs e)
        {
            lambda = double.Parse(textBoxA.Text);
            m = double.Parse(textBoxB.Text);

            double[] array = new double[n];
            Random rand = new Random(123);
            Random rand1 = new Random(123);
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < (m + 1); j++)
                {
                    y = lambda * Math.Exp(-lambda * rand.NextDouble());
                    array[i] += y;
                }
            }
            Array.Sort(array);

            listBox1.Items.Clear();
            for (int i = 0; i < n; i++)
            {
                listBox1.Items.Add(i + 1 + ".  " + array[i]);
            }

            double size = array[n - 1] - array[0];
            int N = (int)(1.0 + 3.22 * Math.Log10(n));
            double length = size /(double)N;
            double[] intervals = new double[N + 1];
            intervals[0] = array[0];
            for (int i = 1; i < intervals.Length; i++)
            {
                intervals[i] = intervals[i - 1] + length;
            }

            int f;//считаем частоты попадания
            int[] frequency = new int[N];
            for (int i = 0; i < N; i++)
            {
                f = 0;
                for (int j = 0; j < n; j++)
                {
                    if (array[j] > intervals[i] && array[j] <= intervals[i + 1])
                    {
                        f++;
                    }
                }
                frequency[i] = f;
            }

            listBox2.Items.Clear();
            for (int i = 0; i < N; i++)
            {
                listBox2.Items.Add(i + 1 + ".  " + frequency[i]);
            }

            //высоты столбцов 
            double[] heights = new double[N];
            for (int i = 0; i < N; i++)
            {
                heights[i] = frequency[i] / (n * length);
            }

            //находим середины полууинервалов для гистограммы 
            double[] middleDistance = new double[N];
            middleDistance[0] = array[0] + length / 2.0;
            for (int i = 1; i < N; i++)
            {
                middleDistance[i] = middleDistance[i - 1] + length;
            }

            //гистограмма
            this.chart1.Series.Clear();
            Series series1 = this.chart1.Series.Add("Гистограмма");
            Series series2 = this.chart1.Series.Add("Полигон");
            chart1.Legends.Clear();
            chart1.ChartAreas[0].AxisX.Enabled = AxisEnabled.False;
            series1.ChartType = SeriesChartType.Column;
            series1.Points.DataBindXY(middleDistance, heights);
            series2.ChartType = SeriesChartType.Line;
            series2.Points.DataBindXY(middleDistance, heights);

            double[] teorArray = new double[n];
            for (int i = 0; i < n; i++)
            {
                teorArray[i] = lambda * Math.Pow(lambda * rand1.NextDouble(), m) * Math.Exp(-lambda * rand1.NextDouble()) / Fact(m);
            }
            Array.Sort(teorArray);

            int tf;//считаем теоретические частоты попадания
            int[] teorFrequency = new int[N];
            for (int i = 0; i < N; i++)
            {
                tf = 0;
                for (int j = 0; j < n; j++)
                {
                    if (teorArray[j] > intervals[i] && teorArray[j] <= intervals[i + 1])
                    {
                        tf++;
                    }
                }
                teorFrequency[i] = tf;
            }

            listBox3.Items.Clear();
            for (int i = 0; i < N; i++)
            {
                listBox3.Items.Add(teorFrequency[i]);
            }

            //xи квадрат
            double pirson = 0;
            for (int i = 0; i < N; i++)
            {
                pirson += Math.Pow(frequency[i] - teorFrequency[i] * n, 2) / teorFrequency[i] * n;
            }

            textBoxRes.Text = pirson.ToString();
        }
        private void studentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Вариант 6, Кубанов К.В., группа 6302");
        }
        private void taskToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            MessageBox.Show("Получить на ЭВМ последовательность из N=1000 реализаций случайной величины X, распределенной по закону Эрланга. " +
                "Контроль качества провести для n = 12. Уровень значимости - 0.025.");
        }
    }
}
