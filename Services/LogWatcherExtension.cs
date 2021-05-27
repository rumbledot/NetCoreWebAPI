using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;

namespace CoreWebApi.Services
{
    public static class LogWatcherExtension
    {
        public static ILoggingBuilder AddLogWatcher(this ILoggingBuilder builder)
        {
            builder.AddConfiguration();

            builder.Services.TryAddEnumerable(
                ServiceDescriptor.Singleton<ILoggerProvider, LogWatcherProvider>());

            LoggerProviderOptions.RegisterProviderOptions
                <LogWatcherConfig, LogWatcherProvider>(builder.Services);

            return builder;
        }

        public static ILoggingBuilder AddLogWatcher(this ILoggingBuilder builder, Action<LogWatcherConfig> configure)
        {
            builder.AddLogWatcher();
            builder.Services.Configure(configure);

            return builder;
        }
    }
}
