using System.Threading;

public class DefiniteIntegral
{
    public static double Solve(double a, double b, Func<double, double> function, double step, int threadsnumber)
    {
        if (function == null)
        {
            throw new ArgumentNullException(nameof(function));
        }
        if (threadsnumber <= 0)
        {
            throw new ArgumentException("Ошибка! Число потоков должно быть больше нуля", nameof(threadsnumber));
        }
        if (step <= 0)
        {
            throw new ArgumentException("Ошибка! Шаг должен быть больше нуля", nameof(step));
        }
        double result = 0.0;
        double Length = b - a;
        double PartOfLength = Length / threadsnumber;
        using var barrier = new Barrier(threadsnumber + 1);
        for(int i = 0; i < threadsnumber; i++)
        {
            int index = i;
            var thread = new Thread(() =>
            {
                double left = a + index * PartOfLength;
                double right = left + PartOfLength;
                if (index == threadsnumber - 1)
                {
                    right = b;
                }
                double PartialSum = 0.0;
                double x = left;
                while (x + step <= right)
                {
                    PartialSum += (function(x) + function(x + step)) / 2.0 * step;
                    x += step;
                }
                double initial, computed;
                do
                {
                    initial = result;
                    computed = initial + PartialSum;
                }
                while (initial != Interlocked.CompareExchange(ref result, computed, initial));
                barrier.SignalAndWait();
            });
            thread.Start();
        }
        barrier.SignalAndWait();
        return result;
    }
}

