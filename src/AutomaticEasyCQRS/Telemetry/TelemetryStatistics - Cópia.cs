using System;
using System.Data;
using AutomaticEasyCQRS.Commands;
using AutomaticEasyCQRS.Events;
using AutomaticEasyCQRS.Queries;
using System.Data.SqlClient;

namespace AutomaticEasyCQRS.Telemetry
{
    public class TelemetryStatistics
    {
        private readonly string _connectionString;

        public TelemetryStatistics(string connectionString)
        {
            _connectionString = connectionString;
        }

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
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string updateQuery = @"
                    UPDATE Statistics
                    SET
                        TotalCommandsRegistered = TotalCommandsRegistered + 1,
                        TotalQueriesRegistered = TotalQueriesRegistered + 1,
                        TotalEventsRegistered = TotalEventsRegistered + 1,
                        TotalCommandsExecuted = CASE WHEN @isCommand AND NOT @hasError THEN TotalCommandsExecuted + 1 ELSE TotalCommandsExecuted END,
                        TotalQueriesExecuted = CASE WHEN @isQuery AND NOT @hasError THEN TotalQueriesExecuted + 1 ELSE TotalQueriesExecuted END,
                        TotalEventsPublished = CASE WHEN @isEvent AND NOT @hasError THEN TotalEventsPublished + 1 ELSE TotalEventsPublished END,
                        TotalErrors = CASE WHEN @hasError THEN TotalErrors + 1 ELSE TotalErrors END,
                        LastErrorMessage = CASE WHEN @hasError THEN @errorMessage ELSE LastErrorMessage END
                ";

                using (SqlCommand command = new SqlCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@isCommand", typeof(ICommand).IsAssignableFrom(commandType));
                    command.Parameters.AddWithValue("@isQuery", typeof(IQuery).IsAssignableFrom(commandType));
                    command.Parameters.AddWithValue("@isEvent", typeof(IEvent).IsAssignableFrom(commandType));
                    command.Parameters.AddWithValue("@hasError", hasError);
                    command.Parameters.AddWithValue("@errorMessage", errorMessage ?? null);

                    command.ExecuteNonQuery();
                }
            }

        }
    }
}
