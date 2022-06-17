using Microsoft.Data.SqlClient;
using System.Diagnostics.Tracing;

public class SqlClientListener : EventListener
{
    protected override void OnEventSourceCreated(EventSource eventSource)
    {
        if (eventSource.Name.Equals("Microsoft.Data.SqlClient.EventSource"))
            EnableEvents(eventSource, EventLevel.Verbose, EventKeywords.All);
    }

    protected override void OnEventWritten(EventWrittenEventArgs eventData)
    {
        Console.WriteLine(eventData.Payload?.First());
    }
}
class Program
{
    public static void Main()
    {
        try
        {
            using SqlClientListener listener = new();

            var connectionString = Environment.GetEnvironmentVariable("DB");
            Console.WriteLine($"Attempting connection with connection string {connectionString}");

            using var connection = new SqlConnection(connectionString);
            connection.Open();

            Console.WriteLine($"Successfully connected!");

            var command = new SqlCommand("SELECT system_user, auth_scheme FROM sys.dm_exec_connections WHERE session_id=@@spid;", connection);

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Console.WriteLine($"SQL Server User: {reader[0]}; Authenticated Using: {reader[1]}");
                }
            }

            connection.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Failed to connect to the database!");
            Console.WriteLine(ex);
        }
    }
}