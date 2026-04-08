using System;
using System.Collections.Generic;
using System.Text;
using NobleIPT2Domain.Models;
using NobleIPT2Domain.Commands;
using Framework.Extensions;
namespace Framework.Commands
{
    public class CreateCommand : ICreateCommand
    {
            private readonly Repository _repository;

            public CreateCommand(Repository repository)
            {
                _repository = repository;
            }

            public async Task ExecuteAsync(Sensors Sensors)
            {
                var parameters = Sensors.ToCreateSensorsDynamicParameters();
                await _repository.SaveDataAsync("DefaultConnection", "[dbo].[CreateSensors]", parameters);
            }
        }
}
