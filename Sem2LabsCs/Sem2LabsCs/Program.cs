using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Reflection;
using static System.Console;
using static System.Math;

namespace Sem2LabsCs
{
    public struct Point
    {
        public Point(double x, double y)
        {
            Number++;
            this.x = x;
            this.y = y;
            number = Number;
        }
        public double x, y;
        public static int Number { get; private set; } = 0;
        int number;
        public static double Distance(Point a, Point b)
        {
            return Sqrt(Pow(a.x - b.x, 2) + Pow(a.y - b.y, 2));
        }
        public override string ToString()
        {
            return $"P{number}({x}, {y})";
        }
    }
    public class Rectangle // Task 11
    {
        public Rectangle(Point A, Point C)
        {
            a = A;
            c = C;
        }
        Point a, c;
        public double Length { get { return Point.Distance(a, c); } }
        public Point[] Apex
        {
            get
            {
                Point[] points = new Point[4];
                points[0] = a; points[1] = c;
                points[2] = new Point(a.x, c.y);
                points[3] = new Point(c.x, a.y);
                return points;
            }
        }
        public double[] Leg
        {
            get
            {
                return new double[] { Abs(a.x - c.x), Abs(a.y - c.y) };
            }
        }
        public string Legs()
        {
            var s = (string)null;
            foreach (var item in Leg)
            {
                s += $"leg {item} ";
            }
            return s;
        }
        public double Tan
        {
            get
            {
                var t = Leg;
                return t[1] / t[0];
            }
        }
        public Point Center
        {
            get
            {
                return new Point((a.x + c.x) / 2, (a.y + c.y) / 2);
            }
        }
        public double Perimeter
        {
            get
            {
                return 2 * Leg[0] + 2 * Leg[1];
            }
        }
        public double Area
        {
            get
            {
                return Leg[0] * Leg[1];
            }
        }
        public string Line
        {
            get
            {
                return $"{Tan}x + {Min(a.y, c.y)}";
            }
        }
        public static bool operator >(Rectangle A, Rectangle B)
        {
            return A.Area > B.Area;
        }
        public static bool operator <(Rectangle A, Rectangle B)
        {
            return A.Area < B.Area;
        }
        public override bool Equals(object obj)
        {
            return (obj as Rectangle).Area.Equals(Area);
        }
        public override int GetHashCode()
        {
            return a.GetHashCode() * c.GetHashCode();
        }
        public static bool operator ==(Rectangle A, Rectangle B)
        {
            return A.Area == B.Area;
        }
        public static bool operator !=(Rectangle A, Rectangle B)
        {
            return A.Area != B.Area;
        }
        public static Rectangle operator *(double N, Rectangle r)
        {
            Rectangle rect = new Rectangle(r.a, r.c);
            rect.a.x -= (N - 1) * rect.Leg[0] / 2;
            rect.a.y -= (N - 1) * rect.Leg[1] / 2;
            rect.c.x += (N - 1) * rect.Leg[0] / 2;
            rect.c.y += (N - 1) * rect.Leg[1] / 2;
            return rect;
        }
        public static double operator +(Rectangle A, Rectangle B)
        {
            return A.Area + B.Area;
        }
        public static double operator -(Rectangle A, Rectangle B)
        {
            return A.Area - B.Area;
        }
        public static Rectangle operator |(Rectangle A, Point P)
        {
            return new Rectangle(new Point(A.a.x + P.x, A.a.y + P.y), new Point(A.c.x + P.x, A.c.y + P.y));
        }
        public override string ToString()
        {
            var s = "Rectangle ";
            foreach (var item in Apex)
            {
                s += item.ToString();
            }
            return s;
        }
    }
    public struct BelAvia
    {
        public string destination;
        public int number;
    }
    public static class Extensions
    {
        public static T KeyByValue<T, W>(this Dictionary<T, W> dict, W val)
        {
            T key = default;
            foreach (KeyValuePair<T, W> pair in dict)
            {
                if (EqualityComparer<W>.Default.Equals(pair.Value, val))
                {
                    key = pair.Key;
                    break;
                }
            }
            return key;
        }
    }
    public class Point<T>
    {
        public Point(int x, int y)
        {
            this.x = x; this.y = y;
        }
        public Point(int x, int y, T value)
        {
            this.x = x; this.y = y; this.value = value;
        }
        public readonly int x, y;
        public T value;
        public static double Distance(Point<T> a, Point<T> b)
        {
            return Sqrt(Pow((a.x - b.x), 2) + Pow((a.y - b.y), 2));
        }
    }
    public class Matrix<T>
    {
        public Matrix(int n, int m)
        {
            Elem = new T[n][];
            for (int i = 0; i < n; i++)
            {
                Elem[i] = new T[m];
            }
        }
        public Matrix(int n, int m, params T[] border)
        {
            Elem = new T[n][];
            for (int i = 0; i < n; i++)
            {
                Elem[i] = new T[m];
            }
            Elem[0][0] = border[0];
            Elem[0][m - 1] = border[1];
            Elem[n - 1][0] = border[2];
            Elem[n - 1][m - 1] = border[3];
            Point<T>[] point = { new Point<T>(0, 0, border[0]), new Point<T>(0, m-1, border[1]),
            new Point<T>(n - 1, 0, border[2]), new Point<T>(n-1, m-1, border[3])};
            Dictionary<Point<T>, double> dict = new Dictionary<Point<T>, double>();
            foreach (var item in point)
            {
                dict.Add(item, 0);
            }
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    Point<T> temp = new Point<T>(i, j);
                    for (int k = 0; k < dict.Count; k++)
                    {
                        dict[point[k]] = Point<T>.Distance(temp, point[k]);
                    }
                    double[] darr = new double[4];
                    dict.Values.CopyTo(darr, 0);
                    Array.Sort(darr);
                    if (darr[0] == darr[1])
                    {
                        if (darr[1] == darr[2])
                        {
                            dynamic d = (dynamic)point[0].value + (dynamic)point[1].value + (dynamic)point[2] + (dynamic)point[3];
                            Elem[i][j] = (T)d;
                        }
                        else
                        {
                            dynamic d; bool b = false;
                            int k = 0;
                            for (; k < 2; k++)
                            {
                                if (dict[point[k]] == darr[0])
                                {
                                    b = true;
                                    break;
                                }
                            }
                            if (b) d = point[k].value;
                            else d = point[2].value;
                            k++;
                            while (k < 4)
                            {
                                if (dict[point[k]] == darr[0]) d += point[k].value;
                                k++;
                            }
                            Elem[i][j] = (T)d;
                        }
                    }
                    else Elem[i][j] = dict.KeyByValue(darr[0]).value;
                }
            }
        }
        public void RemoveIfContains(bool n, T val)
        {
            List<List<T>> list = new List<List<T>>();
            foreach (var item in Elem)
            {
                list.Add(new List<T>(item));
            }
            int cnt;
            if (n)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    cnt = 0;
                    foreach (var item in list[i])
                    {
                        if ((dynamic)item == (dynamic)val) cnt++;
                    }
                    if (cnt == list[i].Count)
                    {
                        list.RemoveAt(i);
                        continue;
                    }
                }
            }
            else
            {
                for (int i = 0; i < list[0].Count; i++)
                {
                    cnt = 0;
                    for (int j = 0; j < list.Count; j++)
                    {
                        if ((dynamic)list[j][i] == (dynamic)val) cnt++;
                    }
                    if (cnt == list.Count)
                    {
                        for (int j = 0; j < list.Count; j++)
                        {
                            list[j].RemoveAt(i);
                            continue;
                        }
                    }
                }
            }
            Elem = new T[list.Count][];
            for (int i = 0; i < Elem.Length; i++)
            {
                Elem[i] = list[i].ToArray();
            }
        }
        public override string ToString()
        {
            string s = null;
            for (int i = 0; i < Elem.Length; i++)
            {
                for (int j = 0; j < Elem[i].Length; j++)
                {
                    s += Elem[i][j] + " ";
                }
                s += "\n";
            }
            return s;
        }
        public T[][] Elem { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {
            /*
            // Task 5
            var arr0 = new int[] { 3, 2, 4, 5, 3, 2, 5 };
            var arr = Task5(arr0);
            WriteLine("Array:");
            foreach (var item in arr0)
            {
                Write(item + " ");
            }
            WriteLine("Indexes of non local minimums are:");
            foreach (var item in arr)
            {
                Write(item + " ");
            }
            WriteLine();
            // Task 6
            WriteLine("Default matrix:");
            Matrix<int> mat = new Matrix<int>(4, 5);
            for (int i = 0; i < mat.Elem.Length; i++)
            {
                for (int j = 0; j < mat.Elem[0].Length; j++)
                {
                    mat.Elem[i][j] = 1;
                }
            }
            for (int i = 0; i < mat.Elem.Length; i++)
            {
                mat.Elem[i][2] = 0;
            }
            for (int i = 0; i < mat.Elem[2].Length; i++)
            {
                mat.Elem[2][i] = 0;
            }
            WriteLine(mat.ToString());
            mat.RemoveIfContains(true, 0);
            mat.RemoveIfContains(false, 0);
            WriteLine("Handled matrix: \n" + mat.ToString());
            //
            Task7();
            WriteLine(new Matrix<string>(4, 5, "nw", "ne", "sw", "se").ToString()); // task 8
            Task9();
            */
            Task10();
            Task11Demo();
            Task13();
            Task14();
        }
        public static void Task10()
        {
            string[] temp;
            using (StreamReader sr = new StreamReader(new FileStream("Input10.txt", FileMode.Open)))
            {
                temp = sr.ReadToEnd().Split("\n");
            }
            SortedDictionary<int, string> dict = new SortedDictionary<int, string>();
            for (int i = 1; i < temp.Length; i+=2)
            {
                dict.Add(Convert.ToInt32(temp[i + 1]), temp[i]);
            }
            foreach (var item in dict)
            {
                WriteLine($"{item.Key} Brest - {item.Value}");
            }
        }
        public static void Task11Demo()
        {
            WriteLine("Enter a point in the format: x y");
            var a = ReadLine().Split();
            WriteLine("Enter another one");
            var c = ReadLine().Split();
            Rectangle r1 = new Rectangle(new Point(Convert.ToDouble(a[0]), Convert.ToDouble(a[1])),
                new Point(Convert.ToDouble(c[0]), Convert.ToDouble(c[1])));
            WriteLine(r1.ToString());
            var properties = typeof(Rectangle).GetProperties();
            foreach (var item in properties)
            {
                WriteLine($"{item.Name}: {item.GetValue(r1)}");
            }
            WriteLine(r1.Legs());
            WriteLine("And now another rectangle. Enter the first point");
            a = ReadLine().Split();
            WriteLine("Enter another one");
            c = ReadLine().Split();
            Rectangle r2 = new Rectangle(new Point(Convert.ToDouble(a[0]), Convert.ToDouble(a[1])),
    new Point(Convert.ToDouble(c[0]), Convert.ToDouble(c[1])));
            WriteLine(r2.ToString());
            foreach (var item in properties)
            {
                WriteLine($"{item.Name}: {item.GetValue(r2)}");
            }
            WriteLine(r2.Legs());
            WriteLine("r1 == r2 - " + (r1 == r2).ToString());
            WriteLine("r1 + r2 = " + (r1 + r2).ToString());
            WriteLine("r1 - r2 = " + (r1 - r2).ToString());
            WriteLine("r1 > r2 = " + (r1 > r2).ToString());
            WriteLine("Enter a number N");
            double N = Convert.ToDouble(ReadLine());
            WriteLine("N * r1 = " + (N * r1).ToString());
            WriteLine("Enter a vector");
            a = ReadLine().Split();
            Point vector = new Point(Convert.ToDouble(a[0]), Convert.ToDouble(a[1]));
            WriteLine("vector || r1 = " + (r1 | vector).ToString());
        }
        public static void Task13()
        {
            WriteLine("Enter x");
            double x = Convert.ToDouble(ReadLine());
            CubePolinom cb = new CubePolinom();
            Hyperbola hb = new Hyperbola();
            WriteLine("Cube Polinom: " + cb.Calc(x));
            WriteLine("Hyperbola: " + hb.Calc(x));
        }
        public static void Task14()
        {
            WriteLine("Enter integer A");
            int A = Convert.ToInt32(ReadLine());
            var s = Convert.ToString(A, 2);
            WriteLine(s);
            int zeroes = s.Replace("1", null).Length;
            int ones = s.Length - zeroes;
            WriteLine($"There are {ones} '1' which is by {ones - zeroes} more then '0'");
            while (true)
            {
                try
                {
                    WriteLine("Enter integer B");
                    int B = Convert.ToInt32(ReadLine());
                    var b = Convert.ToString(B, 2);
                    WriteLine($"B is {b}\nEnter integer p");
                    int p = Convert.ToInt32(ReadLine());
                    WriteLine("Enter integer n");
                    int n = Convert.ToInt32(ReadLine());
                    WriteLine("Enter integer m");
                    int m = Convert.ToInt32(ReadLine());
                    WriteLine("Enter integer q");
                    int q = Convert.ToInt32(ReadLine());
                    StringBuilder asb = new StringBuilder(s), bsb = new StringBuilder(b);
                    bsb.Remove(q, m);
                    if (q > 0) q--;
                    for (int i = p; i < n + p; i++)
                    {
                        bsb[q + i - p] = asb[i];
                        asb[i] = '1';
                    }
                    WriteLine($"After changes:\nA is {asb}\n B is {bsb}");
                    return;
                }
                catch (IndexOutOfRangeException)
                {
                    WriteLine("Try again!");
                    continue;
                }
                catch (ArgumentOutOfRangeException)
                {
                    WriteLine("Try again!");
                    continue;
                }
            }
        }
        abstract class Function
        {
            public abstract string Calc(double x); 
        }
        class CubePolinom : Function
        {
            public override string Calc(double x)
            {
                return $"{x * x * x}a + {x * x}b + {x}c + d";
            }
        }
        class Hyperbola : Function
        {
            public override string Calc(double x)
            {
                return $"+-b*sqrt({x*x}/a^2 - 1)";
            }
        }
        public static void Task9()
        {
            List<string> list;
            FileStream fs = new FileStream("Input9.txt", FileMode.Open);
            using (StreamReader sr = new StreamReader(fs))
            {
                list = new List<string>(sr.ReadToEnd().Split());
            }
            fs.Close();
            char X = list[0][0];
            list.RemoveAt(0);
            foreach (var item in list)
            {
                if (item.Length > 0 && item[0] == X)
                {
                    WriteLine(item);
                    return;
                }
            }
            WriteLine(0);
        }
        public static void Task7()
        {
            var list = new List<string>();
            FileStream fs = new FileStream("Input7.txt", FileMode.Open);
            using (StreamReader sr = new StreamReader(fs))
            {
                list.AddRange(sr.ReadToEnd().Split());
            }
            var ch = list[list.Count - 1][0];
            int cnt = -1;
            foreach (var item in list)
            {
                if (item.Length > 0 && item[item.Length - 1] == ch) cnt++;
            }
            WriteLine($"There are {cnt} words that end with {ch}");
        }
        public static int[] Task5(int[] input)
        {
            var list = new List<int>();
            for (int i = 1; i < input.Length -1; i++)
            {
                if (input[i - 1] <= input[i] || input[i + 1] <= input[i]) list.Add(i + 1);
            }
            return list.ToArray();
        }
    }
}