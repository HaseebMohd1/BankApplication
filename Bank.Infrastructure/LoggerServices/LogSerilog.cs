using Serilog;

namespace Bank.Infrastructure.LoggerServices
{
    internal class LogSerilog : ISerilog
    {
        public void Error(string message)
        {
            throw new NotImplementedException();
        }

        public void Information(string message)
        {
            throw new NotImplementedException();
        }

        public void Warning(string message)
        {
            throw new NotImplementedException();
        }
    }
}
