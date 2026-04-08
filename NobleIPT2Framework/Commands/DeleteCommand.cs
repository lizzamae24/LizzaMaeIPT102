using System;
using System.Collections.Generic;
using System.Text;
using Framework.Extensions;
using NobleIPT2Domain.Models;
using NobleIPT2Domain.Commands;
namespace Framework.Commands
{
    public class DeleteCommand : IDeleteCommand
    {
        private readonly Repository _repository;

        public DeleteCommand(Repository repository)
        {
            _repository = repository;
        }

        public async Task ExecuteAsync(Sensors model)
        {
            var parameters = model.ToDeleteSensorsDynamicParameters();
            await _repository.SaveDataAsync("DefaultConnection", "[dbo].[DeleteSensors]", parameters);
        }
    }
}
