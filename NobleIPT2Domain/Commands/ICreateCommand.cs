
    using NobleIPT2Domain.Models;

    namespace NobleIPT2Domain.Commands
    {
        public interface ICreateCommand
        {
            Task ExecuteAsync(Sensors model);

        }
    }
