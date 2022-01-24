using System;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

var stopwatch = new Stopwatch();
stopwatch.Start();

var testData = new List<TestData>();
using (var reader = new StreamReader("test.csv"))
{
    reader.ReadLine();
    while (!reader.EndOfStream)
    {
        var s = reader.ReadLine();
        var columns = s.Split(',');

        testData.Add(new TestData(
            columns[0],
            columns[1],
            double.Parse(columns[2]),
            double.Parse(columns[3])));
    }
}
Console.WriteLine(stopwatch.Elapsed.TotalSeconds);

var result = testData
    .GroupBy(d => d.a)
    .Select(g => new ResultData(a: g.Key, sum: g.Sum(d => MultiplyToInt(d.x, d.y))));
using (FileStream file = File.OpenWrite("result.json"))
{
    JsonSerializer.Serialize(file, result, SourceGenerationContext.Default.IEnumerableResultData);
}

var ts = stopwatch.Elapsed;
Console.WriteLine($"処理時間: {ts.TotalSeconds}秒");

int MultiplyToInt(double x, double y)
{
    if (x > 0)
        return (int)(x * y + 0.0000001);
    return (int)(x * y - 0.0000001);
}

[JsonSerializable(typeof(IEnumerable<ResultData>))]
internal partial class SourceGenerationContext : JsonSerializerContext { }

record struct TestData(string a, string b, double x, double y);
record struct ResultData(string a, int sum);
