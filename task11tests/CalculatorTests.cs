using task11;
using Xunit;

namespace task11tests
{
    public class CalculatorTests
    {
        private const string Code = @"
            using task11;
            public class Calculator: ICalculator
            {
                public int Add(int a, int b) => a + b;
                public int Minus(int a, int b) => a - b;
                public int Mul(int a, int b) => a * b;
                public int Div(int a, int b) => a / b;
            }";

        [Fact]
        public void Add_ShouldReturnSum()
        {
            var calculator = Generator.CreateCalculator(Code);
            Assert.Equal(10, calculator.Add(4, 6));
        }

        [Fact]
        public void Minus_ShouldReturnDifference()
        {
            var calculator = Generator.CreateCalculator(Code);
            Assert.Equal(1, calculator.Minus(3, 2));
        }

        [Fact]
        public void Mul_ShouldReturnProduct()
        {
            var calculator = Generator.CreateCalculator(Code);
            Assert.Equal(24, calculator.Mul(4, 6));
        }

        [Fact]
        public void Div_ShouldReturnQuotient()
        {
            var calculator = Generator.CreateCalculator(Code);
            Assert.Equal(6, calculator.Div(24, 4));
        }
    }
}

