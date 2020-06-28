using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;

namespace ConsoleApp2
{
    public class PathNode
    {
        public Point Position { get; set; }
        public int PathLengthFromStart { get; set; }
        public PathNode CameFrom { get; set; }
        public int HeuristicEstimatePathLength { get; set; }
        public int EstimateFullPathLength
        {
            get
            {
                return this.PathLengthFromStart + this.HeuristicEstimatePathLength;
            }
        }
    }

    class Program
    {
        private static int GetHeuristicPathLength(Point from, Point to)
        {
            return Math.Abs(from.X - to.X) + Math.Abs(from.Y - to.Y);
        }
        private static List<Point> GetPathForNode(PathNode pathNode)
        {
            var result = new List<Point>();
            var currentNode = pathNode;
            while (currentNode != null)
            {
                result.Add(currentNode.Position);
                currentNode = currentNode.CameFrom;
            }
            result.Reverse();
            return result;
        }
        private static Collection<PathNode> GetNeighbours(PathNode pathNode,
  Point goal, int[,] field)
        {
            var result = new Collection<PathNode>();
            Point[] neighbourPoints = new Point[4];
            neighbourPoints[0] = new Point(pathNode.Position.X + 1, pathNode.Position.Y);
            neighbourPoints[1] = new Point(pathNode.Position.X - 1, pathNode.Position.Y);
            neighbourPoints[2] = new Point(pathNode.Position.X, pathNode.Position.Y + 1);
            neighbourPoints[3] = new Point(pathNode.Position.X, pathNode.Position.Y - 1);

            foreach (var point in neighbourPoints)
            {
                if (point.X < 0 || point.X >= field.GetLength(0))
                    continue;
                if (point.Y < 0 || point.Y >= field.GetLength(1))
                    continue;
                if ((field[point.X, point.Y] != 1))
                    continue;
                var neighbourNode = new PathNode()
                {
                    Position = point,
                    CameFrom = pathNode,
                    PathLengthFromStart = pathNode.PathLengthFromStart + 1,
                    HeuristicEstimatePathLength = GetHeuristicPathLength(point, goal)
                };
                result.Add(neighbourNode);
            }
            return result;
        }    
        public static List<Point> FindPath(int[,] field, Point start, Point goal)
        {
            var closedSet = new Collection<PathNode>();
            var openSet = new Collection<PathNode>();
            PathNode startNode = new PathNode()
            {
                Position = start,
                CameFrom = null,
                PathLengthFromStart = 0,
                HeuristicEstimatePathLength = GetHeuristicPathLength(start, goal)
            };
            openSet.Add(startNode);
            while (openSet.Count > 0)
            {
                var currentNode = openSet.OrderBy(node =>
                  node.EstimateFullPathLength).First();
                if (currentNode.Position == goal)
                    return GetPathForNode(currentNode);
                openSet.Remove(currentNode);
                closedSet.Add(currentNode);
                foreach (var neighbourNode in GetNeighbours(currentNode, goal, field))
                {
                    if (closedSet.Count(node => node.Position == neighbourNode.Position) > 0)
                        continue;
                    var openNode = openSet.FirstOrDefault(node =>
                      node.Position == neighbourNode.Position);
                    if (openNode == null)
                        openSet.Add(neighbourNode);
                    else
                      if (openNode.PathLengthFromStart > neighbourNode.PathLengthFromStart)
                    {
                        openNode.CameFrom = currentNode;
                        openNode.PathLengthFromStart = neighbourNode.PathLengthFromStart;
                    }
                }
            }
            return null;
        }
        public static int[,] waveAlg(int[,] field, Point start, Point finish,ref int  d)
        {
            int[,] newField = new int[field.GetLength(0), field.GetLength(1)];
            for (int i = 0; i < field.GetLength(0); i++)
                for (int j = 0; j < field.GetLength(1); j++)
                    newField[i, j] = field[i, j] != 1 ? -1 : -2;
            newField[start.X, start.Y] = 0;
            bool isFind = false;
            bool isError = false;
            d = 0;
            while (!isFind && !isError)
            {
                isError = true;
                for (int i = 0; i < newField.GetLength(0); i++)
                    for (int j = 0; j < newField.GetLength(1); j++)
                    {
                        if (newField[i, j] == d)
                        {
                            if (finish.X == i && finish.Y == j)
                            {
                                isFind = true;
                                isError = false;
                                d--;
                                goto Wexit;
                            }
                            if (newField.GetLength(0) > i + 1)
                            {
                                newField[i + 1, j] = newField[i + 1, j] == -2 ? d + 1 : newField[i + 1, j];
                                isError = newField[i + 1, j] == d + 1 ? false : isError;
                            }
                            if (-1 < i - 1)
                            {
                                newField[i - 1, j] = newField[i - 1, j] == -2 ? d + 1 : newField[i - 1, j];
                                isError = newField[i - 1, j] == d + 1 ? false : isError; ;
                            }
                            if (newField.GetLength(1) > j+1)
                            {
                                newField[i, j + 1] = newField[i, j + 1] == -2 ? d + 1 : newField[i, j + 1];
                                isError = newField[i , j+1] == d + 1 ? false : isError;
                            }
                            if (-1 < j - 1)
                            {
                                newField[i, j - 1] = newField[i, j - 1] == -2 ? d + 1 : newField[i, j - 1];
                                isError = newField[i, j - 1] == d + 1 ? false : isError;
                            }
                        }
                    }
                d++;
            Wexit:;
            }

            return isError ? null : newField;
        }
        static void printPachAlg(int[,] field,Point start,Point finish ,ref int d)
        {
            bool isEnd = false;
            int[,] newField = new int[field.GetLength(0), field.GetLength(1)];
            for (int i = 0; i < field.GetLength(0); i++)
                for (int j = 0; j < field.GetLength(1); j++)
                    if (field[i, j] == -1)
                        newField[i, j] = -1;
                    else
                        newField[i, j] = -3;
            Point currentPoint = finish;
            while (!isEnd)
            {
                bool oneWay = true;
                if (currentPoint==start)
                {
                    isEnd=true;
                    break;
                }
                int i = currentPoint.X;
                int j = currentPoint.Y;
                if (field.GetLength(0) > i + 1&&oneWay)
                {
                    if (field[i + 1, j] == d)
                    {
                        currentPoint = new Point(i + 1, j);
                        newField[i + 1, j] = -4;
                        oneWay = false;
                    }
                }
                if (-1 < i - 1 && oneWay)
                {
                    if (field[i - 1, j] == d)
                    {
                        currentPoint = new Point(i - 1, j);
                        newField[i - 1, j] = -4;
                        oneWay = false;
                    }
                }
                if (field.GetLength(1) > j + 1 && oneWay)
                {
                    if (field[i , j+1] == d)
                    {
                        currentPoint = new Point(i, j+1);
                        newField[i , j+1] = -4;
                        oneWay = false;
                    }
                }
                if (-1 < j - 1 && oneWay)
                {
                    if (field[i , j-1] == d)
                    {
                        currentPoint = new Point(i, j-1);
                        newField[i , j-1] = -4;
                        oneWay = false;
                    }
                }
                d--;

            }
            Console.Out.Write("\n");
            for (int i = 0; i < newField.GetLength(0); i++)
            {
                for (int j = 0; j < newField.GetLength(1); j++)
                    if (i == start.X && j == start.Y)
                        Console.Write("s ");
                    else if (i == finish.X && j == finish.Y)
                        Console.Write("f ");
                    else if (newField[i,j]==-4)
                        Console.Out.Write("@ ");
                else if (newField[i, j] == -1)
                        Console.Out.Write("0 ");
                else
                        Console.Out.Write("1 ");
                Console.Out.Write("\n");
            }
        }
        static void printPachAstar(List<Point> dlist, Point start, Point finish, int [,] field)
        {
            for (int i = 0; i < field.GetLength(0); i++)
            {
                for (int j = 0; j < field.GetLength(1); j++)
                    if (i == start.X && j == start.Y)
                        Console.Write("s ");
                    else if (i == finish.X && j == finish.Y)
                        Console.Write("f ");
                    else if (dlist.Contains(new Point(i, j)))
                        Console.Write("@ ");
                    else if (field[i, j] == 1)
                        Console.Write("1 ");
                    else
                        Console.Write("0 ");
                Console.Out.Write("\n");
            }
        }
        static int[,] Input(out int n)
        {
            StreamReader file = new StreamReader("d:/t.txt");
            string s = file.ReadToEnd();
            file.Close();
            string[] строка = s.Split('\n');
            string[] столбец = строка[0].Split(' ');
            int[,] a = new int[строка.Length, столбец.Length];
            int t = 0;
            n = 0;
            for (int i = 0; i < строка.Length; i++)
            {
                столбец = строка[i].Split(' ');
                for (int j = 0; j < столбец.Length; j++)
                {
                    t = Convert.ToInt32(столбец[j]);
                    a[i, j] = t;
                }
            }

            return a;
        }
        public static Point setPoint()
        {
            int x, y;
            string num;
            num = Console.ReadLine();
            x = Convert.ToInt32(num);
            num = Console.ReadLine();
            y = Convert.ToInt32(num);
            return new Point(x, y);
        }
        static void Main(string[] args)
        {
            int n;
            int[,] field = Input(out n);
            for (int g = 0; g < field.GetLength(0); g++)
            {
                for (int j = 0; j < field.GetLength(1); j++)
                    if (field[g, j] == 1)
                        Console.Write("1 ");
                    else
                        Console.Write("0 ");
                Console.Out.Write("\n");
            }
            Console.Out.Write("\n");
            int[,] nfield = new int[field.GetLength(0), field.GetLength(1)];   
            Console.WriteLine("Введите начальную точку");            
            Point start = setPoint();
            Console.WriteLine("Введите конечную точку");            
            Point finish = setPoint();
            if (start.X > -1 && start.X < field.GetLength(0) && start.Y > -1 && start.Y < field.GetLength(1) && finish.X > -1 && finish.X < field.GetLength(0) && finish.Y > -1 && finish.Y < field.GetLength(1))
            {
                field[start.X, start.Y] = 1;
                field[finish.X, finish.Y] = 1;
                long ellapledTicks = DateTime.Now.Millisecond;
                List<Point> dlist = FindPath(field, start, finish);
                ellapledTicks = DateTime.Now.Millisecond - ellapledTicks;
                if (dlist != null)
                {
                    field[start.X, start.Y] = 1;
                field[finish.X, finish.Y] = 1;
                long ellapledTicks = DateTime.Now.Ticks;
                List<Point> dlist = FindPath(field, start, finish);
                ellapledTicks = DateTime.Now.Ticks - ellapledTicks;
                if (dlist != null)
                {
                    Console.WriteLine("Алгоритм А-стар");
                    printPachAstar(dlist, start, finish, field);
                    Console.WriteLine($"Время выполнения {ellapledTicks*100} нс");
                    int d = 0;
                    Console.WriteLine("Волновой алгоритм");
                    ellapledTicks = DateTime.Now.Ticks;
                    nfield = waveAlg(field, start, finish, ref d);
                    ellapledTicks = DateTime.Now.Ticks - ellapledTicks;
                    Console.Out.Write("\n");
                    printPachAlg(nfield, start, finish, ref d);
                    Console.WriteLine($"Время выполнения {ellapledTicks*100} нс");              
  }
                else Console.WriteLine("Нет такого пути");
            }
            else Console.WriteLine("Нет таких точек");
        }
    }
        
}
