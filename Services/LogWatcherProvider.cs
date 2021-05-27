using System;
using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CoreWebApi.Services
{
    public sealed class LogWatcherProvider : ILoggerProvider
    {
        private readonly IDisposable _onChangeToken;
        private LogWatcherConfig _currentConfig;
        private readonly ConcurrentDictionary<string, LogWatcher> _loggers = new ConcurrentDictionary<string, LogWatcher>();

        public LogWatcherProvider(
            IOptionsMonitor<LogWatcherConfig> config)
        {
            _currentConfig = config.CurrentValue;
            _onChangeToken = config.OnChange(updatedConfig => _currentConfig = updatedConfig);
        }

        public ILogger CreateLogger(string categoryName) =>
            _loggers.GetOrAdd(categoryName, name => new LogWatcher(name, _currentConfig));

        public void Dispose()
        {
            _loggers.Clear();
            _onChangeToken.Dispose();
        }
    }
}
