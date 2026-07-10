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
}

