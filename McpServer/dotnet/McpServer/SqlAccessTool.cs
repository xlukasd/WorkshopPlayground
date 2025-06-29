using ModelContextProtocol.Server;
using System.ComponentModel;

using Microsoft.Data.SqlClient;

namespace McpSqlTools
{
    [McpServerToolType]
    public static class SqlAccessTool
    {
        [McpServerTool, Description("Gets the column names from a specified Table in specified Database on specified SQL server.")]
        public static List<string> GetColumnNamesInSqlTable(string serverName, string databaseName, string tableName)
        {
            var connectionString = $"Server={serverName};Database={databaseName};;Integrated Security=True;TrustServerCertificate=True";
            using var connection = new SqlConnection(connectionString);
            connection.Open();
            var command = new SqlCommand($"SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = @TableName", connection);
            command.Parameters.AddWithValue("@TableName", tableName);
            using var reader = command.ExecuteReader();
            var columnNames = new List<string>();
            while (reader.Read())
            {
                columnNames.Add(reader.GetString(0));
            }
            return columnNames;
        }
    }
}
