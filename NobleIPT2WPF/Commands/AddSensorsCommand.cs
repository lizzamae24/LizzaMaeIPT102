using System;
using NobleIPT2WPF.ViewModels;

namespace NobleIPT2WPF.Commands
{
    public class AddSensorsCommand : BaseCommand
    {
        private readonly AddSensorsViewModel _viewModel;

        public AddSensorsCommand(AddSensorsViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override void Execute(object? parameter)
        {
            _viewModel.SaveSensors();
        }
    }
}
