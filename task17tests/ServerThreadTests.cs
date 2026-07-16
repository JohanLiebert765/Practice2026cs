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
    }
}

