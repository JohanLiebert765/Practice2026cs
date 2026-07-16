using System.Diagnostics;
using task17;

namespace task17tests
{
    public class TestCommand: ICommand
    {
        public bool Executed = false;
        public void Execute()
        {
            Executed = true;
        }
    }
    public class FailCommand : ICommand
    {
        public void Execute()
        {
            throw new Exception("Команда сломана!");
        }
    }
    public class LongCommand: ICommand
    {
        private readonly ServerThread Server;
        private readonly int Steps;
        public int Step = 0;
        public bool Finished = false;
        public LongCommand(ServerThread server, int steps)
        {
            Server = server;
            Steps = steps;
        }
        public void Execute()
        {
            Step++;
            if (Step >= Steps)
            {
                Finished = true;
            }
            else
            {
                Server.QueueCommand(this);
            }
        }
    }
    public class ServerThreadTests
    {
        [Fact]
        public void HardStop_SkipsRemainingCommands()
        {
            var server = new ServerThread();
            var FirstCommand = new TestCommand();
            var SecondCommand = new TestCommand();
            server.QueueCommand(FirstCommand);
            server.QueueCommand(new HardStopCommand(server));
            server.QueueCommand(SecondCommand);
            server.Start();
            server.Thread.Join(5000);
            Assert.True(FirstCommand.Executed);
            Assert.False(SecondCommand.Executed);
        }

        [Fact]
        public void SoftStop_ExecutesAllRemainingCommands()
        {
            var server = new ServerThread();
            var FirstCommand = new TestCommand();
            var SecondCommand = new TestCommand();
            server.QueueCommand(FirstCommand);
            server.QueueCommand(new SoftStopCommand(server));
            server.QueueCommand(SecondCommand);
            server.Start();
            server.Thread.Join(5000);
            Assert.True(FirstCommand.Executed);
            Assert.True(SecondCommand.Executed);
        }

        [Fact]
        public void HardStop_ThrowsFromWrongThread()
        {
            var server = new ServerThread();
            server.Start();
            var HardStop = new HardStopCommand(server);
            Assert.Throws<InvalidOperationException>(() => HardStop.Execute());
            server.QueueCommand(new HardStopCommand(server));
            server.Thread.Join(5000);
        }

        [Fact]
        public void SoftStop_ThrowsFromWrongThread()
        {
            var server = new ServerThread();
            server.Start();
            var SoftStop = new SoftStopCommand(server);
            Assert.Throws<InvalidOperationException>(() => SoftStop.Execute());
            server.QueueCommand(new HardStopCommand(server));
            server.Thread.Join(5000);
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
        public void LongCommand_CompletesAllSteps()
        {
            var server = new ServerThread();
            var command = new LongCommand(server, 10);
            server.QueueCommand(command);
            server.QueueCommand(new SoftStopCommand(server));
            server.Start();
            server.Thread.Join(5000);
            Assert.True(command.Finished);
            Assert.Equal(10, command.Step);
        }

        [Fact]
        public void TwoLongCommands_Completes()
        {
            var server = new ServerThread();
            var FirstCommand = new LongCommand(server, 5);
            var SecondCommand = new LongCommand(server, 5);
            server.QueueCommand(FirstCommand);
            server.QueueCommand(SecondCommand);
            server.QueueCommand(new SoftStopCommand(server));
            server.Start();
            server.Thread.Join(5000);
            Assert.True(FirstCommand.Finished);
            Assert.True(SecondCommand.Finished);
            Assert.Equal(5, FirstCommand.Step);
            Assert.Equal(5, SecondCommand.Step);
        }

        [Fact]
        public void HardStop_InterruptsLongCommand()
        {
            var server = new ServerThread();
            var Command = new LongCommand(server, 10);
            server.QueueCommand(Command);
            server.QueueCommand(new HardStopCommand(server));
            server.Start();
            server.Thread.Join(5000);
            Assert.False(Command.Finished);
            Assert.True(Command.Step < 10);
        }

        [Fact]
        public void CommandsWithLongCommands_Completes()
        {
            var server = new ServerThread();
            var Command = new TestCommand();
            var longCommand = new LongCommand(server, 5);
            server.QueueCommand(Command);
            server.QueueCommand(longCommand);
            server.QueueCommand(new SoftStopCommand(server));
            server.Start();
            server.Thread.Join(5000);
            Assert.True(Command.Executed);
            Assert.True(longCommand.Finished);
            Assert.Equal(5, longCommand.Step);  
        }

        [Fact]
        public void Chart()
        {
            int[] CommandCounts = {1, 5, 10, 20, 50, 100};
            double[] times = new double[CommandCounts.Length];
            double[] effectiveness = new double[CommandCounts.Length];
            int Steps = 50;
            int runs = 1000;
            for (int i = 0; i < CommandCounts.Length; i++)
            {
                double totalTime = 0;
                for (int r = 0; r < runs; r++)
                {
                    var server = new ServerThread();
                    for (int j = 0; j < CommandCounts[i]; j++)
                    {
                        var command = new LongCommand(server, Steps);
                        server.QueueCommand(command);
                    }
                    server.QueueCommand(new SoftStopCommand(server));
                    var stop = Stopwatch.StartNew();
                    server.Start();
                    server.Thread.Join(10000);
                    stop.Stop();
                    totalTime += stop.Elapsed.TotalMilliseconds;
                }
                times[i] = totalTime / runs;
                double Time = times[0] * CommandCounts[i];
                effectiveness[i] = (Time / times[i]) * 100;
                Console.WriteLine($"Количество команд: {CommandCounts[i]}, Среднее время: {times[i]:F2} миллисекунд, Эффективность: {effectiveness[i]:F1}%");
            }
            double[] CountsDouble = CommandCounts.Select(x => (double)x).ToArray();
            var plot = new ScottPlot.Plot();
            plot.Add.Scatter(CountsDouble, effectiveness);
            plot.XLabel("Количество команд");
            plot.YLabel("Эффективность (%)");
            plot.Title("Зависимость эффективности от количества команд");
            plot.SavePng("chart_task18.png", 800, 600);
            Console.WriteLine("График успешно сохранён!");        
        }
    }
}

