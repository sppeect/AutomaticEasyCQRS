using AutomaticEasyCQRS.Commands;
using AutomaticEasyCQRS.Events;
using AutomaticEasyCQRS.Queries;

namespace AutomaticEasyCQRS.Telemetry
{
    public class TelemetryStatistics
    {
        public virtual int TotalCommandsRegistered { get; set; }
        public virtual int TotalQueriesRegistered { get; set; }
        public virtual int TotalEventsRegistered { get; set; }
        public virtual int TotalCommandsExecuted { get; set; }
        public virtual int TotalQueriesExecuted { get; set; }
        public virtual int TotalEventsPublished { get; set; }
        public virtual int TotalErrors { get; set; }
        public virtual string? LastErrorMessage { get; set; }

        public void UpdateTelemetryStatistics(Type commandType, bool hasError, string errorMessage = null)
        {
            if (hasError == false)
            {
                if (typeof(ICommand).IsAssignableFrom(commandType))
                {
                    TotalCommandsExecuted++;
                }
                else if (typeof(IQuery).IsAssignableFrom(commandType))
                {
                    TotalQueriesExecuted++;
                }
                else if (typeof(IEvent).IsAssignableFrom(commandType))
                {
                    TotalEventsPublished++;
                }
            }
            else
            {
                LastErrorMessage = errorMessage;
                TotalErrors++; 
            }

            // Lógica adicional para capturar erros e atualizar a estatística de erros, se necessário.
        }
    }
}
