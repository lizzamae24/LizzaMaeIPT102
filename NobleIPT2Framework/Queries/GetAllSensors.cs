using System;
using System.Collections.Generic;
using System.Text;
using NobleIPT2Domain.Models;
using NobleIPT2Domain.Queries;
namespace Framework.Queries
{
    public class GetAllSensors : IGetAllSensors
    {
        private readonly Repository _repository;

        public GetAllSensors(Repository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Sensors>?> ExecuteAsync()
        {
            return await _repository.GetDataAsync<Sensors>("DefaultConnection", "[dbo].[GetAllSensorss]", null);
        }
    }
}
