using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCore.Services
{
    public class ConsoleLoggerProvider : ILoggerProvider
    {
        private LogLevel _minLogLevel;

        public ConsoleLoggerProvider() : this (LogLevel.Trace)
        {

        }
        public ConsoleLoggerProvider(LogLevel minLogLevel)
        {
            _minLogLevel = minLogLevel;
        }
        public ILogger CreateLogger(string categoryName)
        {
            return new ConsoleLogger(_minLogLevel);
        }

        public void Dispose()
        {           
        }

        private class ConsoleLogger : ILogger
        {
            private LogLevel _minLogLevel;

            public ConsoleLogger(LogLevel minLogLevel)
            {
                _minLogLevel = minLogLevel;
            }

            public IDisposable BeginScope<TState>(TState state)
            {
                return null;
            }          

            public bool IsEnabled(LogLevel logLevel)
            {
                return logLevel >= _minLogLevel;
            }

            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
            {
                ConsoleColor currentColor = Console.ForegroundColor;
                switch (logLevel)
                {
                    case LogLevel.Debug:
                        Console.ForegroundColor = ConsoleColor.Green;
                        break;
                    case LogLevel.Information:
                        Console.ForegroundColor = ConsoleColor.White;
                        break;
                    case LogLevel.Warning:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        break;
                    case LogLevel.Error:
                    case LogLevel.Critical:
                        Console.ForegroundColor = ConsoleColor.Red;
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Gray;
                        break;
                }
                Console.WriteLine(formatter(state, exception));
                Console.WriteLine();
                Console.ForegroundColor = currentColor;
            }
        }
    }
}
