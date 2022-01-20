using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ConsoleApp1
{
    class Program
    {

        static double Lerp(double a, double b, double t)
        {
            return a + (b - a) * t;
        }

        static double Normalize(double x)
        {
            return (x + 1) / 2;
        }

        static void Main(string[] args)
        {
			Random random = new Random((int)DateTime.Now.Ticks);
            const int pointsCount = 26;
            const int lerpIterations = 20;
            
            double[,] pointsX0 = new double[pointsCount,2];

            for (int i = 0; i < pointsCount; i++)
            {
                if (i==0)
                {
                    pointsX0[i,0] = 0;
                }
                else
                {
                    pointsX0[i,0] = pointsX0[i - 1,0] + (double)1 / (pointsCount - 1);
                }
                Console.WriteLine("x0(" + i + ") = " + pointsX0[i,0]);
            }

            Console.WriteLine();

            double[] pointsGradient = new double[pointsCount];

            for (int i = 0; i < pointsCount; i++)
            {
                if (random.NextDouble()>0.5)
                {
                    pointsGradient[i] = random.NextDouble();
                    Console.WriteLine("m(" + i + ") = " + pointsGradient[i]);
                }
                else 
                {
                    pointsGradient[i] = -random.NextDouble();
                    Console.WriteLine("m(" + i + ") = " + pointsGradient[i]);
                }
            }
            Console.WriteLine();

            double[,] lerpingPoints = new double[pointsCount - 1, 2];

            for (int i = 0; i < pointsCount - 1; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    if (j == 0)
                    {
                        lerpingPoints[i, j] = (double)(pointsGradient[i] * pointsX0[i,0] - pointsGradient[i + 1] * pointsX0[i + 1,0]) / (pointsGradient[i] - pointsGradient[i + 1]);
                        Console.Write("X[" + i + "] = " + lerpingPoints[i, j]);
                        
                    }
                    else
                    {
                        lerpingPoints[i, j] = pointsGradient[i] * ((double)(pointsGradient[i] * pointsX0[i,0] - pointsGradient[i + 1] * pointsX0[i + 1,0]) / (pointsGradient[i] - pointsGradient[i + 1]) - pointsX0[i,0]);
                        Console.Write("\t\t\t\t Y[" + i + "] = " + lerpingPoints[i, j]);
                        Console.WriteLine();
                    }
                }
            }
            Console.WriteLine();

            double[,] pointA = new double[pointsCount - 1, 2];
            double[,] pointC = new double[pointsCount - 1, 2];
            double[,] pointB = new double[pointsCount - 1, 2];

            double t = 0;

            for (int k = 0; k < lerpIterations; k++)
            {
                if (k > 0)
                {
                    t = ((double)1 / lerpIterations) * k;
                    Console.WriteLine("t = " + t);

                    for (int i = 0; i < pointsCount - 1; i++)
                    {
                        for (int j = 0; j < 2; j++)
                        {
                            pointA[i, j] = Lerp(pointsX0[i, j], lerpingPoints[i, j], t);
                            pointC[i, j] = Lerp(lerpingPoints[i, j], pointsX0[i + 1, j], t);
                            pointB[i, j] = Lerp(pointA[i, j], pointC[i, j], t);

                            if (j == 0)
                            {
                                Console.WriteLine("xA " + i + " = " + pointA[i, j]);
                                Console.WriteLine("xC " + i + " = " + pointC[i, j]);
                                Console.WriteLine("xB " + i + " = " + pointB[i, j]);
                                Console.WriteLine();
                            }
                            else
                            {
                                Console.WriteLine("yA " + i + " = " + pointA[i, j]);
                                Console.WriteLine("yC " + i + " = " + pointC[i, j]);
                                Console.WriteLine("yB " + i + " = " + pointB[i, j]);
                                Console.WriteLine();
                            }
                        }
                    }
                }
            }

            Console.Read();

		}
    }
}
