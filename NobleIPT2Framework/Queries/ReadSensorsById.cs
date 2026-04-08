using Dapper;
using NobleIPT2Domain.Models;
using NobleIPT2Domain.Queries;
namespace Framework.Queries
{
    public class ReadSensorsById : IReadSensorsById
    {
        private readonly Repository _repository;

        public ReadSensorsById(Repository repository)
        {
            _repository = repository;
        }

        public async Task<Sensors?> ExecuteAsync(int SensorsId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@SensorsId", SensorsId);

            var data = await _repository.GetDataAsync<Sensors>(
                "DefaultConnection",
                "[dbo].[ReadSensorsById]",
                parameters
            );

            return data?.FirstOrDefault();
        }
    }
}