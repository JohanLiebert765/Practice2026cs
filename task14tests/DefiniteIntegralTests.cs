using System.Diagnostics;
using Xunit;

public class DefiniteIntegralTests
{
    Func<double, double> X = (double x) => x;
    Func<double, double> SIN = (double x) => Math.Sin(x);

    [Fact]
    public void Integral_X_IsZero()
    {
        Assert.Equal(0, DefiniteIntegral.Solve(-1, 1, X, 1e-4, 2), 1e-4);
    }

    [Fact]
    public void Integral_Sin_IsZero()
    {
        Assert.Equal(0, DefiniteIntegral.Solve(-1, 1, SIN, 1e-5, 8), 1e-4);
    }

    [Fact]
    public void Integral_X_ZerotoFive()
    {
        Assert.Equal(12.5, DefiniteIntegral.Solve(0, 5, X, 1e-6, 8), 1e-4);
    }

    [Fact]
    public void FindOptimalStep()
    {
        double[] steps = {1e-1, 1e-2, 1e-3, 1e-4, 1e-5, 1e-6};
        double ExactValue = -Math.Cos(100) + Math.Cos(-100);
        foreach (var step in steps)
        {
            var stop = Stopwatch.StartNew();
            double result = DefiniteIntegral.Solve(-100, 100, SIN, step, 4);
            stop.Stop();
            double error = Math.Abs(result - ExactValue);
            Console.WriteLine($"Шаг: {step}, Погрешность: {error}, Время: {stop.ElapsedMilliseconds} миллисекунд");
        }
    }

    [Fact]
    public void Measurements()
    {
        int[] ThreadCount = { 1, 2, 4, 8, 16, 32 };
        int runs = 500;
        double BestTime = double.MaxValue;
        int BestThreads = 0;
        foreach (var threads in ThreadCount)
        {
            double Time = 0;
            for (int i = 0; i < runs; i++)
            {
                var stop = Stopwatch.StartNew();
                DefiniteIntegral.Solve(-100, 100, SIN, 1e-4, threads);
                stop.Stop();
                Time += stop.ElapsedMilliseconds;
            }
            double AverageTime = Time / runs;
            Console.WriteLine($"Потоков: {threads}, Среднее время: {AverageTime} миллисекунд");
            if (AverageTime < BestTime)
            {
                BestTime = AverageTime;
                BestThreads = threads;
            }
        }
        Console.WriteLine($"Лучший многопоточный вариант ({BestThreads} потоков): {BestTime} миллисекунд");
        double SingleTime = 0;
        for (int i = 0; i < runs; i++)
        {
            var stop = Stopwatch.StartNew();
            DefiniteIntegral.SolveSingleThread(-100, 100, SIN, 1e-4);
            stop.Stop();
            SingleTime += stop.ElapsedMilliseconds;
        }
        double SingleAverageTime = SingleTime / runs;
        Console.WriteLine($"Среднее время однопоточного варианта: {SingleAverageTime} миллисекунд");
        double difference = (SingleAverageTime - BestTime) / SingleAverageTime * 100;
        Console.WriteLine($"Разница: {difference:F1}%");
    }

    [Fact]
    public void Chart()
    {
        int[] ThreadCount = { 1, 2, 4, 8, 16, 32 };
        int runs = 500;
        double[] times = new double[ThreadCount.Length];
        for (int t = 0; t < ThreadCount.Length; t++)
        {
            double Time = 0;
            for (int i = 0; i < runs; i++)
            {
                var stop = Stopwatch.StartNew();
                DefiniteIntegral.Solve(-100, 100, SIN, 1e-4, ThreadCount[t]);
                stop.Stop();
                Time += stop.ElapsedMilliseconds;
            }
            times[t] = Time / runs;
        }
        var plot = new ScottPlot.Plot();
        double[] threadDoubles = ThreadCount.Select(x => (double)x).ToArray();
        plot.Add.Scatter(times, threadDoubles);
        plot.XLabel("Время (мс)");
        plot.YLabel("Количество потоков");
        plot.Title("Зависимость времени от количества потоков");
        plot.SavePng("chart.png", 800, 600);
        Console.WriteLine("График успешно сохранён!");
    }
}

