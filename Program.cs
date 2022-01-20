using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ConsoleApp1
{
    /*
    class Perlin2D
    {
        byte[] permutationTable;

        public Perlin2D(int seed = 0)
        {
            var rand = new System.Random(seed);
            permutationTable = new byte[1024];
            rand.NextBytes(permutationTable);
        }

        private float[] GetPseudoRandomGradientVector(int x, int y)
        {
            int v = (int)(((x * 1836311903) ^ (y * 2971215073) + 4807526976) & 1023);
            v = permutationTable[v] & 3;

            switch (v)
            {
                case 0: return new float[] { 1, 0 };
                case 1: return new float[] { -1, 0 };
                case 2: return new float[] { 0, 1 };
                default: return new float[] { 0, -1 };
            }
        }

        static float QunticCurve(float t)
        {
            return t * t * t * (t * (t * 6 - 15) + 10);
        }

        static float Lerp(float a, float b, float t)
        {
            return a + (b - a) * t;
        }

        static float Dot(float[] a, float[] b)
        {
            return a[0] * b[0] + a[1] * b[1];
        }

        public float Noise(float fx, float fy)
        {
            int left = (int)System.Math.Floor(fx);
            int top = (int)System.Math.Floor(fy);
            float pointInQuadX = fx - left;
            float pointInQuadY = fy - top;

            float[] topLeftGradient = GetPseudoRandomGradientVector(left, top);
            float[] topRightGradient = GetPseudoRandomGradientVector(left + 1, top);
            float[] bottomLeftGradient = GetPseudoRandomGradientVector(left, top + 1);
            float[] bottomRightGradient = GetPseudoRandomGradientVector(left + 1, top + 1);

            float[] distanceToTopLeft = new float[] { pointInQuadX, pointInQuadY };
            float[] distanceToTopRight = new float[] { pointInQuadX - 1, pointInQuadY };
            float[] distanceToBottomLeft = new float[] { pointInQuadX, pointInQuadY - 1 };
            float[] distanceToBottomRight = new float[] { pointInQuadX - 1, pointInQuadY - 1 };

            float tx1 = Dot(distanceToTopLeft, topLeftGradient);
            float tx2 = Dot(distanceToTopRight, topRightGradient);
            float bx1 = Dot(distanceToBottomLeft, bottomLeftGradient);
            float bx2 = Dot(distanceToBottomRight, bottomRightGradient);

            pointInQuadX = QunticCurve(pointInQuadX);
            pointInQuadY = QunticCurve(pointInQuadY);

            float tx = Lerp(tx1, tx2, pointInQuadX);
            float bx = Lerp(bx1, bx2, pointInQuadX);
            float tb = Lerp(tx, bx, pointInQuadY);

            return tb;
        }

        public float Noise(float fx, float fy, int octaves, float persistence = 0.5f)
        {
            float amplitude = 1;
            float max = 0;
            float result = 0;

            while (octaves-- > 0)
            {
                max += amplitude;
                result += Noise(fx, fy) * amplitude;
                amplitude *= persistence;
                fx *= 2;
                fy *= 2;
            }

            return result / max;
        }
    }
    */

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
