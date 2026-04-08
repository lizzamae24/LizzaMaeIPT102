CREATE TABLE [dbo].[Sensors](
    [SensorsId]    INT            IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [SensorName]   NVARCHAR(100)  NOT NULL,
    [SensorType]   NVARCHAR(100)  NOT NULL,
    [Location]     INT            NOT NULL,
    [SensorStatus] NVARCHAR(100)  NOT NULL
);
