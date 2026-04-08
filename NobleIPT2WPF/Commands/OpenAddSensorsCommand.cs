using System;
using NobleIPT2WPF.Services;

namespace NobleIPT2WPF.Commands
{
    public class OpenAddSensorsCommand : BaseCommand
    {
        private readonly INavigationService _navigationService;

        public OpenAddSensorsCommand(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public override void Execute(object? parameter)
        {
            _navigationService.Navigate();
        }
    }
}
