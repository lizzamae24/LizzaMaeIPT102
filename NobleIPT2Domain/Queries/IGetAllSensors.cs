using System;
using System.Collections.Generic;
using System.Text;
using NobleIPT2Domain.Models;
namespace NobleIPT2Domain.Queries
{
    public interface IGetAllSensors
    {
        Task<IEnumerable<Sensors>?> ExecuteAsync();

    }
}
