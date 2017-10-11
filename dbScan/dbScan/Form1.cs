using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace dbScan
{
    public partial class Form1 : Form
    {

        int total = 0;//Contador de clusters e pontos (variável auxiliar)
        double eps = 0.2;//Variável de controle (raio do densidade)
        int minPts = 20;//quantidade de pontos dentro de um raio eps necessário para criar um cluster
        Random randNum = new Random();//Instancia um objeto de Random para gerar os valores aleatórios
        DBSCAN db = new DBSCAN();//Objeto DBSCAN (para "rodar" o algoritmo de DBSCAN)
        List<List<Point>> clusters;//lista de lista(armazena os clusters e seus pontos)
        List<Point> points = new List<Point>();//Lista de pontos (nosso dataset)

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            chart1.Series.RemoveAt(0);
        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            points.RemoveRange(0, points.Count);
           while (chart1.Series.Count > 0) {
                chart1.Series.RemoveAt(0);
            }
            if (clusters != null)
            {
                while (clusters.Count > 0)
                {
                    clusters.RemoveAt(0);
                }
            }

            

            eps = Double.Parse(textBox1.Text);//Valor digitado pelo usuário
            minPts = int.Parse(textBox2.Text);//Valor digitado pelo usuário
                                              // label1.Text = Double.Parse(textBox1.Text).ToString();
                                              //Setar nosso base de dados (representados por pontos)
            for (int i = 0; i < 1000; i++) {
                //points around (1,1) with most 1 distance
                points.Add(new Point(1, 1 + ((double)i / 1000)));
                points.Add(new Point(1, 1 - ((double)i / 1000)));
                points.Add(new Point(1 - ((double)i / 1000), 1));
                points.Add(new Point(1 + ((double)i / 1000), 1));

                //points around (5,5) with most 1 distance
                points.Add(new Point(5, 5 + ((double)i / 1000)));
                points.Add(new Point(5, 5 - ((double)i / 1000)));
                points.Add(new Point(5 - ((double)i / 1000), 5));
                points.Add(new Point(5 + ((double)i / 1000), 5));
            }

            points.Add(new Point(20.012, 20));
            points.Add(new Point(20.013, 20));
            points.Add(new Point(20.014, 20));
            points.Add(new Point(20.015, 20));
            points.Add(new Point(20.011, 20));
            points.Add(new Point(20.012, 20));
            points.Add(new Point(20.013, 20));
            points.Add(new Point(20.014, 20));
            points.Add(new Point(20.015, 20));
            points.Add(new Point(20.011, 20));
            points.Add(new Point(20, 20));

            for (int i = 0; i < points.Count; i++) {
                listBox1.Items.Add(points[i]);
            }

                /*for (int i = 0; i < 100; i++) {
                    //Gera 100 valores entre 0 e 1 randômicamente para x e y de um ponto
                    points.Add(new Point(randNum.NextDouble(), randNum.NextDouble()));
                    listBox1.Items.Add(points[i]);
                }*/

            clusters = db.GetClusters(points, eps, minPts);//Chama o método "GetClusters" que inicia o algoritmos de DBSCAN
            total = 0;
            for (int i = 0; i < clusters.Count; i++) {
                int count = clusters[i].Count;//Verifica quantos pontos existem no cluster[i]
                total += count;
                string nome = "Cluster "+(i+1)+"(elementos"+count+")";
                chart1.Series.Add(nome);
                chart1.Series[i].ChartType = SeriesChartType.Point;
                chart1.Series[i].XValueType = ChartValueType.Double;
                chart1.Series[i].YValueType = ChartValueType.Double;
                chart1.Series[i].MarkerStyle = MarkerStyle.Circle;



                foreach (Point p in clusters[i])//Printa os pontos do cluster[i]
                    chart1.Series[i].Points.Add(p.X, p.Y);
            }
            //Temos que printar os pontos ruidos
            total = points.Count - total;//Total de ruidos vai ser igual a todos os pontos menos os não ruidos (que pertencem a um cluster)
            if (total > 0)
            {//Quer dizer que possuimos ruidos em nossa amostra de pontos
                string nome = "Ruidos (elementos" + total +")";
                chart1.Series.Add(nome);
                chart1.Series[chart1.Series.Count - 1].ChartType = SeriesChartType.Point;
                chart1.Series[chart1.Series.Count - 1].MarkerStyle = MarkerStyle.Circle;

                foreach (Point p in points)
                {
                    if (p.ClusterId == Point.NOISE)
                        chart1.Series[chart1.Series.Count-1].Points.Add(p.X, p.Y);
                }
            }

        }

        private void chart1_Click_1(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
