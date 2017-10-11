using System;

namespace dbScan
{
    class Point
    {
        public const int NOISE = -1;//Constante que refere ao ruido
        public const int UNCLASSIFIED = 0;//Constante que indica se um ponto foi ou não classificado

        public double X, Y;//Posilção x y do ponto (R^2)
        public int ClusterId;//classica o ponto (>0 significa que o ponto pertence a um cluster)

        public Point(double x, double y)
        {//Construtor da classe Point
            this.X = x;
            this.Y = y;
        }
        public override string ToString()
        {//Estamos sobrescrevendo o metodo toString (para printar o ponto bonitinho)
            return String.Format("({0}, {1})", X, Y);
        }
        public static double DistanceSquared(Point p1, Point p2)
        {//Metodo que calcula a distancia euclidiana entre os pontos p1 e p2
            double diffX = p2.X - p1.X;
            double diffY = p2.Y - p1.Y;
            return Math.Sqrt((diffX * diffX) + (diffY * diffY));
        }
    }
}

