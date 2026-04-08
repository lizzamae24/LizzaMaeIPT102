CREATE PROCEDURE [dbo].[CreateSensors]
    @SensorName   NVARCHAR(100),
    @SensorType   NVARCHAR(100),
    @Location     INT,
    @SensorStatus NVARCHAR(100)
AS
BEGIN
    INSERT INTO [dbo].[Sensors] (SensorName, SensorType, Location, SensorStatus)
    VALUES (@SensorName, @SensorType, @Location, @SensorStatus);
END
