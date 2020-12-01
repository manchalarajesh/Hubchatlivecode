using Serilog.Core;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TelebuHubChat.LogClasses
{
    public class EnrichProp : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(
                    "ThreadId", Thread.CurrentThread.ManagedThreadId));

            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(
                    "MachineName", "HubChat"));
        }

        
    }
}
