using System.Windows.Input;
using NobleIPT2WPF.Commands;
using NobleIPT2WPF.Services;

namespace NobleIPT2WPF.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        public ICommand NavigateAddSensorsCommand { get; }

        public HomeViewModel(INavigationService addSensorsNavigationService)
        {
            NavigateAddSensorsCommand = new OpenAddSensorsCommand(addSensorsNavigationService);
        }
    }
}
