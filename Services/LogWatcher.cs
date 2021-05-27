using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Services
{
    public class LogWatcher : ILogger
    {
        private readonly string _name;
        private readonly LogWatcherConfig _config;

        private Dictionary<string, ConsoleColor> console_color = new Dictionary<string, ConsoleColor>()
        {
            { "Trace", ConsoleColor.DarkRed },
            { "Debug", ConsoleColor.Green },
            { "Information", ConsoleColor.White },
            { "Warning", ConsoleColor.Yellow },
            { "Error", ConsoleColor.DarkMagenta }
        };

        public LogWatcher
            (string name,
            LogWatcherConfig config) =>
                (_name, _config) = (name, config);

        public IDisposable BeginScope<TState>(TState state) => default;

        public bool IsEnabled(LogLevel logLevel) => _config.is_ready;

        public void Log<TState>(LogLevel log_level, EventId event_id, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(log_level))
            {
                Console.WriteLine($"[CONSOLE ONLY][{event_id.Id,2}: {log_level,-12}]");
                Console.WriteLine($"               {formatter(state, exception)}");

                return;
            }

            if (log_level == LogLevel.Error)
            {
                _config.LogError($"[{DateTime.Now}]>[{_name}]>{formatter(state, exception)}");
                if (exception != null)
                {
                    _config.LogError($"                           {exception.Message}");
                    _config.LogError($"                           {exception.StackTrace}");
                }
            }
            else if (log_level == LogLevel.Warning)
            {
                _config.LogWarning($"[{DateTime.Now}]>[{_name}]>{formatter(state, exception)}");
                if (exception != null)
                {
                    _config.LogWarning($"                           {exception.Message}");
                    _config.LogWarning($"                           {exception.StackTrace}"); 
                }
            }
            else 
            {
                Console.WriteLine($"[{event_id.Id,2}: {log_level,-12}]");
                Console.WriteLine($"     {formatter(state, exception)}");
            }
        }
    }
}
