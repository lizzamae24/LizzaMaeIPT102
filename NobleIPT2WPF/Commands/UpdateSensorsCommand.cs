using System;
using NobleIPT2WPF.ViewModels;

namespace NobleIPT2WPF.Commands
{
    public class UpdateSensorsCommand : BaseCommand
    {
        private readonly AddSensorsViewModel _viewModel;

        public UpdateSensorsCommand(AddSensorsViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override void Execute(object? parameter)
        {
            _ = _viewModel.UpdateSensors();
        }
    }
}
