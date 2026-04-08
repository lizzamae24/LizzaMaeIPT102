CREATE PROCEDURE [dbo].[DeleteSensors]
    @SensorsId INT
AS
BEGIN
    DELETE FROM [dbo].[Sensors] WHERE SensorsId = @SensorsId;
END
