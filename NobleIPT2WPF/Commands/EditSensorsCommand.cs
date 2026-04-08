using System;
using NobleIPT2Domain.Models;
using NobleIPT2WPF.ViewModels;

namespace NobleIPT2WPF.Commands
{
    public class EditSensorsCommand : BaseCommand
    {
        private readonly AddSensorsViewModel _viewModel;

        public EditSensorsCommand(AddSensorsViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override void Execute(object? parameter)
        {
            if (parameter is Sensors Sensors)
            {
                _viewModel.LoadSensorsForEdit(Sensors);
            }
        }
    }
}
