using System;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Framework
{
    public class DatabaseAutoSetup
    {
        private readonly IConfiguration _configuration;

        public DatabaseAutoSetup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task EnsureDatabaseSetupAsync()
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            }

            // Extract database name
            var builder = new SqlConnectionStringBuilder(connectionString);
            var databaseName = builder.InitialCatalog;
            var masterConnectionString = connectionString.Replace($"Initial Catalog={databaseName}", "Initial Catalog=master");

            try
            {
                // Step 1: Create database if not exists
                await CreateDatabaseIfNotExistsAsync(masterConnectionString, databaseName);

                // Step 2: Create table if not exists
                await CreateTableIfNotExistsAsync(connectionString);

                // Step 3: Create stored procedures
                await CreateStoredProceduresAsync(connectionString);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Database auto-setup failed: {ex.Message}", ex);
            }
        }

        private async Task CreateDatabaseIfNotExistsAsync(string masterConnectionString, string databaseName)
        {
            using var connection = new SqlConnection(masterConnectionString);
            await connection.OpenAsync();

            var checkDbSql = $"SELECT database_id FROM sys.databases WHERE name = '{databaseName}'";
            using var checkCmd = new SqlCommand(checkDbSql, connection);
            var result = await checkCmd.ExecuteScalarAsync();

            if (result == null)
            {
                var createDbSql = $"CREATE DATABASE [{databaseName}]";
                using var createCmd = new SqlCommand(createDbSql, connection);
                await createCmd.ExecuteNonQueryAsync();
            }
        }

        private async Task CreateTableIfNotExistsAsync(string connectionString)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();

            var sql = @"
                IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Sensors')
                BEGIN
                    CREATE TABLE [dbo].[Sensors]
                    (
                        [SensorsId] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
                        [SensorName] NVARCHAR(100) NOT NULL,
                        [SensorType] NVARCHAR(100) NOT NULL,
                        [Location] NVARCHAR(100) NOT NULL,
                        [SensorStatus] NVARCHAR(100) NOT NULL
                    );
                END";

            using var command = new SqlCommand(sql, connection);
            await command.ExecuteNonQueryAsync();
        }

        private async Task CreateStoredProceduresAsync(string connectionString)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();

            var procedures = new[]
            {
                // CreateSensors
                @"IF OBJECT_ID('[dbo].[CreateSensors]', 'P') IS NOT NULL DROP PROCEDURE [dbo].[CreateSensors];",
                @"CREATE PROCEDURE [dbo].[CreateSensors]
                    @SensorName NVARCHAR(100),
                    @SensorType NVARCHAR(100),
                    @Location NVARCHAR(100),
                    @SensorStatus NVARCHAR(100)
                AS
                BEGIN
                    INSERT INTO [dbo].[Sensors] (SensorName, SensorType, Location, SensorStatus)
                    VALUES (@SensorName, @SensorType, @Location, @SensorStatus);
                END",

                // UpdateSensors
                @"IF OBJECT_ID('[dbo].[UpdateSensors]', 'P') IS NOT NULL DROP PROCEDURE [dbo].[UpdateSensors];",
                @"CREATE PROCEDURE [dbo].[UpdateSensors]
                    @SensorsId INT,
                    @SensorName NVARCHAR(100),
                    @SensorType NVARCHAR(100),
                    @Location NVARCHAR(100),
                    @SensorStatus NVARCHAR(100)
                AS
                BEGIN
                    UPDATE [dbo].[Sensors]
                    SET SensorName = @SensorName,
                        SensorType = @SensorType,
                        Location = @Location,
                        SensorStatus = @SensorStatus
                    WHERE SensorsId = @SensorsId;
                END",

                // DeleteSensors
                @"IF OBJECT_ID('[dbo].[DeleteSensors]', 'P') IS NOT NULL DROP PROCEDURE [dbo].[DeleteSensors];",
                @"CREATE PROCEDURE [dbo].[DeleteSensors]
                    @SensorsId INT
                AS
                BEGIN
                    DELETE FROM [dbo].[Sensors]
                    WHERE SensorsId = @SensorsId;
                END",

                // GetAllSensorss
                @"IF OBJECT_ID('[dbo].[GetAllSensorss]', 'P') IS NOT NULL DROP PROCEDURE [dbo].[GetAllSensorss];",
                @"CREATE PROCEDURE [dbo].[GetAllSensorss]
                AS
                BEGIN
                    SELECT * FROM Sensors ORDER BY SensorsId;
                END",

                // ReadSensorsById
                @"IF OBJECT_ID('[dbo].[ReadSensorsById]', 'P') IS NOT NULL DROP PROCEDURE [dbo].[ReadSensorsById];",
                @"CREATE PROCEDURE [dbo].[ReadSensorsById]
                    @SensorsId INT
                AS
                BEGIN
                    SELECT * FROM [dbo].[Sensors]
                    WHERE SensorsId = @SensorsId;
                END"
            };

            foreach (var sql in procedures)
            {
                if (!string.IsNullOrWhiteSpace(sql))
                {
                    using var command = new SqlCommand(sql, connection);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
