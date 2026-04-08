using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using NobleIPT2Domain.Commands;
using NobleIPT2WPF.Commands;
using NobleIPT2WPF.Services;
using NobleIPT2Domain.Queries;
using NobleIPT2Domain.Models;

namespace NobleIPT2WPF.ViewModels
{
    public class AddSensorsViewModel : BaseViewModel
    {
        private readonly ICreateCommand _createSensors;
        private readonly IGetAllSensors _getAllSensors;
        private readonly IUpdateCommand _updateSensors;
        private readonly IDeleteCommand _deleteSensors;

        private string _sensorName = string.Empty;
        private string _sensorType = string.Empty;
        private string _location = string.Empty;
        private string _sensorStatus = string.Empty;
        private string _searchText = string.Empty;
        private bool _isEditMode = false;
        private int _currentSensorsId;
        private ObservableCollection<Sensors> _allSensorsList = new();

        public AddSensorsViewModel(
            ICreateCommand createSensors,
            IGetAllSensors getAllSensors,
            IUpdateCommand updateSensors,
            IDeleteCommand deleteSensors,
            INavigationService homeNavigationService)
        {
            _createSensors = createSensors;
            _getAllSensors = getAllSensors;
            _updateSensors = updateSensors;
            _deleteSensors = deleteSensors;

            Sensorss = new ObservableCollection<Sensors>();
            SaveCommand = new AddSensorsCommand(this);
            UpdateCommand = new UpdateSensorsCommand(this);
            DeleteCommand = new DeleteCommand(this);
            EditCommand = new EditSensorsCommand(this);
            CancelCommand = new OpenHomeCommand(homeNavigationService);

            _ = LoadSensorssAsync();
        }

        public ObservableCollection<Sensors> Sensorss { get; }

        public string SensorName
        {
            get => _sensorName;
            set { _sensorName = value; OnPropertyChanged(nameof(SensorName)); }
        }

        public string SensorType
        {
            get => _sensorType;
            set { _sensorType = value; OnPropertyChanged(nameof(SensorType)); }
        }

        public string Location
        {
            get => _location;
            set { _location = value; OnPropertyChanged(nameof(Location)); }
        }

        public string SensorStatus
        {
            get => _sensorStatus;
            set { _sensorStatus = value; OnPropertyChanged(nameof(SensorStatus)); }
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
                FilterSensorss();
            }
        }

        public bool IsEditMode
        {
            get => _isEditMode;
            set
            {
                _isEditMode = value;
                OnPropertyChanged(nameof(IsEditMode));
                OnPropertyChanged(nameof(IsAddMode));
                OnPropertyChanged(nameof(SaveButtonText));
            }
        }

        public bool IsAddMode => !IsEditMode;
        public string SaveButtonText => IsEditMode ? "Update" : "Save";

        public ICommand SaveCommand { get; }
        public ICommand UpdateCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand CancelCommand { get; }

        public async void SaveSensors()
        {
            if (IsEditMode)
                await UpdateSensors();
            else
            {
                try
                {
                    var sensor = new Sensors
                    {
                        SensorName = SensorName,
                        SensorType = SensorType,
                        Location = Location,
                        SensorStatus = SensorStatus
                    };
                    await _createSensors.ExecuteAsync(sensor);
                    ClearForm();
                    await LoadSensorssAsync();
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show($"Error saving: {ex.Message}", "Error",
                        System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                }
            }
        }

        public async Task UpdateSensors()
        {
            try
            {
                var sensor = new Sensors
                {
                    SensorsId = _currentSensorsId,
                    SensorName = SensorName,
                    SensorType = SensorType,
                    Location = Location,
                    SensorStatus = SensorStatus
                };
                await _updateSensors.ExecuteAsync(sensor);
                ClearForm();
                IsEditMode = false;
                await LoadSensorssAsync();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error updating: {ex.Message}", "Error",
                    System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }

        public async void DeleteSensors(Sensors sensor)
        {
            try
            {
                await _deleteSensors.ExecuteAsync(sensor);
                await LoadSensorssAsync();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error deleting: {ex.Message}", "Error",
                    System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }

        public void LoadSensorsForEdit(Sensors sensor)
        {
            _currentSensorsId = sensor.SensorsId;
            SensorName = sensor.SensorName ?? string.Empty;
            SensorType = sensor.SensorType ?? string.Empty;
            Location = sensor.Location ?? string.Empty;
            SensorStatus = sensor.SensorStatus ?? string.Empty;
            IsEditMode = true;
        }

        private void ClearForm()
        {
            SensorName = string.Empty;
            SensorType = string.Empty;
            Location = string.Empty;
            SensorStatus = string.Empty;
            IsEditMode = false;
            _currentSensorsId = 0;
        }

        private async Task LoadSensorssAsync()
        {
            var sensors = await _getAllSensors.ExecuteAsync();
            _allSensorsList.Clear();
            Sensorss.Clear();
            if (sensors != null)
            {
                foreach (var s in sensors)
                {
                    _allSensorsList.Add(s);
                    Sensorss.Add(s);
                }
            }
        }

        private void FilterSensorss()
        {
            Sensorss.Clear();
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                foreach (var s in _allSensorsList)
                    Sensorss.Add(s);
            }
            else
            {
                var filtered = _allSensorsList.Where(s =>
                    s.SensorName.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                    s.SensorType.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                    s.SensorStatus.Contains(SearchText, StringComparison.OrdinalIgnoreCase));
                foreach (var s in filtered)
                    Sensorss.Add(s);
            }
        }
    }
}
