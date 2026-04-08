CREATE PROCEDURE [dbo].[GetAllSensorss]
AS
BEGIN
    SELECT * FROM [dbo].[Sensors] ORDER BY SensorsId;
END
