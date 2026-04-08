using System;
using System.Collections.Generic;
using System.Text;
using NobleIPT2Domain.Models;
using NobleIPT2Domain.Commands;  
using Framework.Extensions;
namespace Framework.Commands
{
        public class UpdateCommand : IUpdateCommand
        {
            private readonly Repository _repository;

            public UpdateCommand(Repository repository)
            {
                _repository = repository;
            }

            public async Task ExecuteAsync(Sensors model)
            {
                var parameters = model.ToSensorsDynamicParameters();
                await _repository.SaveDataAsync("DefaultConnection", "[dbo].[UpdateSensors]", parameters);
            }
        }
}
