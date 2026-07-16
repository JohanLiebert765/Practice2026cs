using System.Diagnostics;
using task17;

namespace task17tests
{
    public class FailCommand : ICommand
    {
        public bool IsComplete => true;
        public void Execute()
        {
            throw new Exception("Команда сломана!");
        }
    }
    public class CountingCommand : ICommand
    {
        private readonly int _id;
        private readonly int _Calls;
        private int _counter = 0;
        public CountingCommand(int id, int Calls)
        {
            _id = id;
            _Calls = Calls;
        }
        public int Counter => _counter;
        public bool IsComplete => _counter >= _Calls;
        public void Execute()
        {
           Console.WriteLine($"Поток {_id} вызов {++_counter}");
        }
    }
    public class ServerThreadTests
    {
        [Fact]
        public void FiveCommands_ThreeCalls_HardStop()
        {
            var server = new ServerThread();
            var commands = new List<CountingCommand>();
            for (int i = 1; i <= 5; i++)
            {
                var command = new CountingCommand(i, 3);
                commands.Add(command);
                server.QueueCommand(command);
            }
            server.Start();
            Thread.Sleep(100);
            server.QueueCommand(new HardStopCommand(server));
            server.Thread.Join(5000);
            foreach (var command in commands)
            {
                Console.WriteLine($"Команда {command.Counter} вызовов");
                Assert.Equal(3, command.Counter);
            }
        }

        [Fact]
        public void HardStop_InterruptsCountingCommand()
        {
            var server = new ServerThread();
            var command = new CountingCommand(1, 10);
            server.QueueCommand(command);
            server.QueueCommand(new HardStopCommand(server));
            server.Start();
            server.Thread.Join(5000);
            Assert.True(command.Counter < 10);
        }
        
         [Fact]
        public void ExceptionHandler_CatchesError()
        {
            var server = new ServerThread();
            Exception caught = null;
            server.ExceptionHandler = (command, exception) =>
            {
                caught = exception;
            };
            server.QueueCommand(new FailCommand());
            server.QueueCommand(new HardStopCommand(server));
            server.Start();
            server.Thread.Join(5000);
            Assert.NotNull(caught);
            Assert.Equal("Команда сломана!", caught.Message);
        }

        [Fact]
        public void Chart()
        {
            int[] CallCounts = {1, 5, 10, 20, 50, 100, 200, 500};
            double[] times = new double[CallCounts.Length];
            int CommandCount = 5;
            int runs = 100;
            for (int i = 0; i < CallCounts.Length; i++)
            {
                double Time = 0;
                for (int r = 0; r < runs; r++)
                {
                    var server = new ServerThread();
                    for (int j = 0; j < CommandCount; j++)
                    {
                        server.QueueCommand(new CountingCommand(j, CallCounts[i]));
                    }
                    server.QueueCommand(new SoftStopCommand(server));
                    var stop = Stopwatch.StartNew();
                    server.Start();
                    server.Thread.Join(10000);
                    stop.Stop();
                    Time += stop.Elapsed.TotalMilliseconds;
                }
                times[i] = Time / runs;
                Console.WriteLine($"Шагов на команду: {CallCounts[i]}, Среднее время: {times[i]:F2} миллисекунд");
            }
            double[] CallsDouble = CallCounts.Select(x => (double)x).ToArray();
            var plot = new ScottPlot.Plot();
            plot.Add.Scatter(CallsDouble, times);
            plot.XLabel("Количество шагов на команду");
            plot.YLabel("Время (мс)");
            plot.Title("Зависимость времени от длительности команд");
            plot.SavePng("chart_task19.png", 800, 600);
            Console.WriteLine("График сохранён!");
        }
    }
}

