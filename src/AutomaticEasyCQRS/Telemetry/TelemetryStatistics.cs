using System;
using AutomaticEasyCQRS.Commands;
using AutomaticEasyCQRS.Events;
using AutomaticEasyCQRS.Queries;
using System.Threading;

namespace AutomaticEasyCQRS.Telemetry
{
    public class TelemetryStatistics
    {
        private int _totalCommandsRegistered;
        private int _totalQueriesRegistered;
        private int _totalEventsRegistered;
        private int _totalCommandsExecuted;
        private int _totalQueriesExecuted;
        private int _totalEventsPublished;
        private int _totalErrors;
        private string? _lastErrorMessage;

        public virtual int TotalCommandsRegistered => _totalCommandsRegistered;
        public virtual int TotalQueriesRegistered => _totalQueriesRegistered;
        public virtual int TotalEventsRegistered => _totalEventsRegistered;
        public virtual int TotalCommandsExecuted => _totalCommandsExecuted;
        public virtual int TotalQueriesExecuted => _totalQueriesExecuted;
        public virtual int TotalEventsPublished => _totalEventsPublished;
        public virtual int TotalErrors => _totalErrors;
        public virtual string? LastErrorMessage => _lastErrorMessage;

        public void IncrementRegisteredCount(Type handlerType)
        {
            if (typeof(ICommand).IsAssignableFrom(handlerType))
            {
                Interlocked.Increment(ref _totalCommandsRegistered);
            }
            else if (typeof(IQuery).IsAssignableFrom(handlerType))
            {
                Interlocked.Increment(ref _totalQueriesRegistered);
            }
            else if (typeof(IEvent).IsAssignableFrom(handlerType))
            {
                Interlocked.Increment(ref _totalEventsRegistered);
            }
        }

        public void UpdateTelemetryStatistics(Type messageType, bool hasError, string? errorMessage = null)
        {
            if (!hasError)
            {
                if (typeof(ICommand).IsAssignableFrom(messageType))
                {
                    Interlocked.Increment(ref _totalCommandsExecuted);
                }
                else if (typeof(IQuery).IsAssignableFrom(messageType))
                {
                    Interlocked.Increment(ref _totalQueriesExecuted);
                }
                else if (typeof(IEvent).IsAssignableFrom(messageType))
                {
                    Interlocked.Increment(ref _totalEventsPublished);
                }
            }
            else
            {
                _lastErrorMessage = errorMessage;
                Interlocked.Increment(ref _totalErrors);
            }
        }

        public TelemetryStatisticsSnapshot CreateSnapshot()
        {
            return new TelemetryStatisticsSnapshot(
                TotalCommandsRegistered,
                TotalQueriesRegistered,
                TotalEventsRegistered,
                TotalCommandsExecuted,
                TotalQueriesExecuted,
                TotalEventsPublished,
                TotalErrors,
                LastErrorMessage);
        }
    }

    public record TelemetryStatisticsSnapshot(
        int TotalCommandsRegistered,
        int TotalQueriesRegistered,
        int TotalEventsRegistered,
        int TotalCommandsExecuted,
        int TotalQueriesExecuted,
        int TotalEventsPublished,
        int TotalErrors,
        string? LastErrorMessage);
}
