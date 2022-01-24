﻿using System.Diagnostics;
using Newtonsoft.Json;

namespace MyApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var testData = new List<TestData>();
            using (var reader = new StreamReader("test.csv"))
            {
                reader.ReadLine();
                while (!reader.EndOfStream)
                {
                    string s = reader.ReadLine();
                    var columns = s.Split(',');
                    testData.Add(new TestData
                    {
                        a = columns[0],
                        b = columns[1],
                        x = double.Parse(columns[2]),
                        y = double.Parse(columns[3])
                    });
                }
            }

            var testData_0 = testData.Select(d => new { d.a, d.b, d.x, d.y, z = MultiplyToInt(d.x, d.y) });
            var testData_1 = testData_0.GroupBy(d => d.a)
                .Select(g => new { a = g.Key, sum = g.Sum(d => d.z) });
            using (StreamWriter file = File.CreateText("result.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, testData_1);
            }

            var ts = stopwatch.Elapsed;
            Console.WriteLine($"処理時間: {ts.TotalSeconds}秒");
        }

        static int MultiplyToInt(double x, double y)
        {
            if (x > 0)
                return (int)(x * y + 0.0000001);
            return (int)(x * y - 0.0000001);
        }
    }

    class TestData
    {
        public string a { get; set; }
        public string b { get; set; }
        public double x { get; set; }
        public double y { get; set; }
    }
}