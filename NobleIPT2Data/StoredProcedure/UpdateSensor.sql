CREATE PROCEDURE [dbo].[UpdateSensors]
    @SensorsId    INT,
    @SensorName   NVARCHAR(100),
    @SensorType   NVARCHAR(100),
    @Location     INT,
    @SensorStatus NVARCHAR(100)
AS
BEGIN
    UPDATE [dbo].[Sensors]
    SET SensorName   = @SensorName,
        SensorType   = @SensorType,
        Location     = @Location,
        SensorStatus = @SensorStatus
    WHERE SensorsId = @SensorsId;
END
