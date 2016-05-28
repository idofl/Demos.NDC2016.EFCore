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

        public ConsoleLoggerProvider() : this (LogLevel.Verbose)
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

            public IDisposable BeginScopeImpl(object state)
            {
                return null;
            }

            public bool IsEnabled(LogLevel logLevel)
            {
                return logLevel >= _minLogLevel;
            }

            public void Log(LogLevel logLevel, int eventId, object state, Exception exception, Func<object, Exception, string> formatter)
            {
                ConsoleColor currentColor = Console.ForegroundColor;
                switch (logLevel)
                {
                    case LogLevel.Debug:
                    case LogLevel.Information:
                        Console.ForegroundColor = ConsoleColor.White;
                        break;
                    case LogLevel.Warning:
                        Console.ForegroundColor = ConsoleColor.Yellow;
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
