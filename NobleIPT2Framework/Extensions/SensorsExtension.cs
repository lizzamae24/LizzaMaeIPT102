using System.Data;
using Dapper;
using NobleIPT2Domain.Models;

namespace Framework.Extensions
{
    public static class SensorsExtension
    {
        public static DynamicParameters ToSensorsDynamicParameters(this Sensors model)
        {
            var param = new DynamicParameters();
            param.Add("@SensorsId", model.SensorsId, DbType.Int32, ParameterDirection.Input);
            param.Add("@SensorName", model.SensorName, DbType.String, ParameterDirection.Input);
            param.Add("@SensorType", model.SensorType, DbType.String, ParameterDirection.Input);
            param.Add("@Location", model.Location, DbType.String, ParameterDirection.Input);
            param.Add("@SensorStatus", model.SensorStatus, DbType.String, ParameterDirection.Input);
            return param;
        }

        public static DynamicParameters ToCreateSensorsDynamicParameters(this Sensors model)
        {
            var param = new DynamicParameters();
            param.Add("@SensorName", model.SensorName, DbType.String, ParameterDirection.Input);
            param.Add("@SensorType", model.SensorType, DbType.String, ParameterDirection.Input);
            param.Add("@Location", model.Location, DbType.String, ParameterDirection.Input);
            param.Add("@SensorStatus", model.SensorStatus, DbType.String, ParameterDirection.Input);
            return param;
        }

        public static DynamicParameters ToDeleteSensorsDynamicParameters(this Sensors model)
        {
            var param = new DynamicParameters();
            param.Add("@SensorsId", model.SensorsId, DbType.Int32, ParameterDirection.Input);
            return param;
        }
    }
}
