using NobleIPT2Domain.Models;


namespace NobleIPT2Domain.Commands
{
    public interface IUpdateCommand
    {
        Task ExecuteAsync(Sensors model);
    }
}
