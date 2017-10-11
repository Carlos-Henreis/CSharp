using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dbScan
{
    class DBSCAN
    {
        /*
        *  GetClusters retorna todos os clusters os pontos que os compoe (usando o algoritmo DBSCAN )
        *  Entrada: lista de pontos, o raio de densidade e a quantidade minima de pontos
        */
        public List<List<Point>> GetClusters(List<Point> points, double eps, int minPts)
        {
            if (points == null)//Teste de controle
                return null;
            List<List<Point>> clusters = new List<List<Point>>();//inicia nossa lista de lista (cluster = lista de objetos pontos)
            int clusterId = 1;
            for (int i = 0; i < points.Count; i++)
            {//Varre os pontos
                Point p = points[i];
                if (p.ClusterId == Point.UNCLASSIFIED)
                {//Quer dizer que este ponto não foi classificado
                    if (ExpandCluster(points, p, clusterId, eps, minPts))//Chamamos o método ExpandCluster para avaliar o ponto
                        //Neste caso verifica se o ponto p é um ponto central de um novo cluster
                        clusterId++;//Retornar verdade quer dizer que a partir do ponto avaliado possui mos um cluster (atualizadamos o a ID)
                }
            }
            //Temos classificar os pontos e seus cluster
            int maxClusterId = points.OrderBy(p => p.ClusterId).Last().ClusterId;//ordenamos os pontos a partir do clusterID e pegamos o últmo
            if (maxClusterId < 1)
                return clusters; //sem clusters, então a lista está vazia
            for (int i = 0; i < maxClusterId; i++)
                clusters.Add(new List<Point>());//Para cada cluster iniciamos um lista de objetos pontos (compoem o cluster)
            foreach (Point p in points)
            {
                if (p.ClusterId > 0)
                    clusters[p.ClusterId - 1].Add(p);//Varremos todos os pontos e adicionamos cada ponto ao seu respectivo cluster
            }
            return clusters;
        }

        /*
         * Método auxiliar 
         * Dados uam lista de pontos, um ponto central e um raio
         * retorna os pontos que fazerm parte desse regiao (a ditancia entre p e points[i] <= eps) 
        */
        static List<Point> GetRegion(List<Point> points, Point p, double eps)
        {
            List<Point> region = new List<Point>();//Essa regiao é composta por uma lista de pontos
            for (int i = 0; i < points.Count; i++)
            {
                double distSquared = Point.DistanceSquared(p, points[i]);//Calcula a distancia entre o ponto central (o avaliado) e os demais pontos
                if (distSquared <= eps)
                    region.Add(points[i]);
            }
            return region;
        }

        static bool ExpandCluster(List<Point> points, Point p, int clusterId, double eps, int minPts)
        {
            List<Point> seeds = GetRegion(points, p, eps);
            if (seeds.Count < minPts)
            { // não é ponto central 
                p.ClusterId = Point.NOISE;//Define o ponto como ruido
                return false;
            }
            else
            {//todos os pontos contidos em seeds são alcançáveis a partir do ponto 'p'
                for (int i = 0; i < seeds.Count; i++)
                    seeds[i].ClusterId = clusterId;//Marcamos esses pontos com o ID do cluster
                seeds.Remove(p);
                //Temos que verificar se os pontos contidos nesse cluster tb pode ser um ponto central
                while (seeds.Count > 0)
                {
                    Point currentP = seeds[0];//ponto que será verificado
                    List<Point> result = GetRegion(points, currentP, eps);
                    if (result.Count >= minPts)
                    {//Ponto central
                        for (int i = 0; i < result.Count; i++)
                        {
                            Point resultP = result[i];
                            if (resultP.ClusterId == Point.UNCLASSIFIED || resultP.ClusterId == Point.NOISE)
                            {
                                if (resultP.ClusterId == Point.UNCLASSIFIED)
                                    seeds.Add(resultP);//Adiciona ele no fim do seed para ser avalado
                                resultP.ClusterId = clusterId;//Adiciona esse ponto ao cluster "pai"
                            }
                        }
                    }
                    seeds.Remove(currentP);//remove ponto avaliado
                }
                return true;//Se chegamos aqui o ponto foi avaliado junto aos seus vizinhos e um cluster foi montado
            }
        }
    }
}
