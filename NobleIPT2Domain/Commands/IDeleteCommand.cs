using NobleIPT2Domain.Models;
namespace NobleIPT2Domain.Commands
{
    public interface IDeleteCommand
    {
        Task ExecuteAsync(Sensors model);

    }
}
