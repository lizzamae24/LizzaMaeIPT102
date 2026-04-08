using NobleIPT2Domain.Models;
namespace NobleIPT2Domain.Queries
{
    public interface IReadSensorsById
    {
        Task<Sensors?> ExecuteAsync(int SensorsId);

    }
}
