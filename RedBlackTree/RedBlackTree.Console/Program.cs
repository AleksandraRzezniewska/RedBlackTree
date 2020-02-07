using RedBlackTree.Library.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using static System.IO.Path;

namespace RedBlackTree.Console
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateRandomTreeWithIntegerType(20);

            CreateTreeWithDoubleType();

            CreateTreeWithStringType();

            TimingTest<int>(1000);
        }

        private static void CreateRandomTreeWithIntegerType(int amount)
        {
            var tree = new RBTree<int>();
            var random = new System.Random();

            for (int i = 0; i < amount; i++)
            {
                tree.Add(random.Next(100));
            }

            System.Console.WriteLine(tree);
        }

        private static void CreateTreeWithDoubleType()
        {
            var tree = new RBTree<double>
            {
                2.3,
                5.6,
                9.2,
                13.88,
                92.02,
                0.5,
                20,
            };

            System.Console.WriteLine(tree);

            tree.Remove(13.88);

            System.Console.WriteLine(tree.Contains(13.88));

            tree.Clear();

            System.Console.WriteLine(tree);
        }

        private static void CreateTreeWithStringType()
        {
            var tree = new RBTree<string>
            {
                "a",
                "c",
                "f",
                "g",
                "b",
            };

            System.Console.WriteLine(tree);

            System.Console.WriteLine(tree.Contains("f"));

            tree.Remove("a");

            System.Console.WriteLine(tree.Count);
        }

        private static void TimingTest<T>(int amount) where T : IComparable
        {
            var path = GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\\TimingTest.csv";
            var csv = new StringBuilder();
            csv.AppendLine(string.Format("Time of Tree Add; Time of List Add; Time of Tree Contains; Time of List Contains"));

            for (var i = 10; i < amount; i++)
            {
                var tree = new RBTree<T>();
                var list = new List<T>();
                var stopWatch = new Stopwatch();

                stopWatch.Start();

                for (var j = 0; j < i; j++)
                {
                    tree.Add((T)Convert.ChangeType(j, typeof(T)));
                }

                stopWatch.Stop();
                var treeAddTime = stopWatch.ElapsedMilliseconds;

                stopWatch.Start();

                for (var j = 0; j < i; j++)
                {
                    list.Add((T)Convert.ChangeType(j, typeof(T)));
                }

                stopWatch.Stop();
                var listAddTime = stopWatch.ElapsedMilliseconds;

                stopWatch.Start();

                tree.Contains((T)Convert.ChangeType(900, typeof(T)));

                stopWatch.Stop();
                var treeContainsTime = stopWatch.ElapsedMilliseconds;

                stopWatch.Start();

                list.Contains((T)Convert.ChangeType(900, typeof(T)));

                stopWatch.Stop();
                var listContainsTime = stopWatch.ElapsedMilliseconds;

                var newLine = string.Format("{0}; {1}; {2}; {3}", treeAddTime, listAddTime, treeContainsTime, listContainsTime);
                csv.AppendLine(newLine);
            }

            File.WriteAllText(path, csv.ToString());
        }
    }
}


