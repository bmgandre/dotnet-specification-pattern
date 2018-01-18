/*
   https://blogs.msdn.microsoft.com/dbrowne/2017/09/22/simple-logging-for-ef-core/
 */

namespace Microsoft.Samples.EFLogging
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Infrastructure;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;

    public enum LoggingCategories
    {
        All = 0,
        SQL = 1
    }

    public static class DbContextLoggingExtensions
    {
        public static void ConfigureLogging(this DbContext db, Action<String> logger, Func<string, LogLevel, bool> filter)
        {
            var serviceProvider = db.GetInfrastructure<IServiceProvider>();
            var loggerFactory = (ILoggerFactory)serviceProvider.GetService(typeof(ILoggerFactory));

            LogProvider.CreateOrModifyLoggerForDbContext(db.GetType(), loggerFactory, logger, filter);
        }

        public static void ConfigureLogging(this DbContext db, Action<String> logger,
                                           LoggingCategories categories = LoggingCategories.All)
        {
            var serviceProvider = db.GetInfrastructure<IServiceProvider>();
            var loggerFactory = (LoggerFactory)serviceProvider.GetService(typeof(ILoggerFactory));

            if (categories == LoggingCategories.SQL)
            {
                var SqlCategories = new List<string> { DbLoggerCategory.Database.Command.Name,
                                                       DbLoggerCategory.Query.Name,
                                                       DbLoggerCategory.Update.Name };
                LogProvider.CreateOrModifyLoggerForDbContext(db.GetType(),
                                                             loggerFactory,
                                                             logger,
                                                             (c, l) => SqlCategories.Contains(c));
            }
            else if (categories == LoggingCategories.All)
            {
                LogProvider.CreateOrModifyLoggerForDbContext(db.GetType(),
                                                             loggerFactory, logger,
                                                             (c, l) => true);
            }
        }
    }

    internal class LogProvider : ILoggerProvider
    {
        //volatile to allow the configuration to be switched without locking
        public volatile LoggingConfiguration Configuration;

        private static bool DefaultFilter(string CategoryName, LogLevel level) => true;

        private static ConcurrentDictionary<Type, LogProvider> providers = new ConcurrentDictionary<Type, LogProvider>();

        public static void CreateOrModifyLoggerForDbContext(Type DbContextType,
                                                            ILoggerFactory loggerFactory,
                                                            Action<string> logger,
                                                            Func<string, LogLevel, bool> filter = null)
        {
            bool isNew = false;
            var provider = providers.GetOrAdd(DbContextType, t =>
              {
                  var p = new LogProvider(logger, filter ?? DefaultFilter);
                  loggerFactory.AddProvider(p);
                  isNew = true;
                  return p;
              }
              );
            if (!isNew)
            {
                provider.Configuration = new LoggingConfiguration(logger, filter ?? DefaultFilter);
            }
        }

        public class LoggingConfiguration
        {
            public LoggingConfiguration(Action<string> logger, Func<string, LogLevel, bool> filter)
            {
                this.logger = logger;
                this.filter = filter;
            }

            public readonly Action<string> logger;
            public readonly Func<string, LogLevel, bool> filter;
        }

        private LogProvider(Action<string> logger, Func<string, LogLevel, bool> filter)
        {
            this.Configuration = new LoggingConfiguration(logger, filter);
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new Logger(categoryName, this);
        }

        public void Dispose()
        { }

        private class Logger : ILogger
        {
            private readonly string categoryName;
            private readonly LogProvider provider;

            public Logger(string categoryName, LogProvider provider)
            {
                this.provider = provider;
                this.categoryName = categoryName;
            }

            public bool IsEnabled(LogLevel logLevel)
            {
                return true;
            }

            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state,
                                    Exception exception, Func<TState, Exception, string> formatter)
            {
                //grab a reference to the current logger settings for consistency,
                //and to eliminate the need to block a thread reconfiguring the logger
                var config = provider.Configuration;
                if (config.filter(categoryName, logLevel))
                {
                    config.logger(formatter(state, exception));
                }
            }

            public IDisposable BeginScope<TState>(TState state)
            {
                return null;
            }
        }
    }
}