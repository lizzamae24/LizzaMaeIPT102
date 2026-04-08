
namespace NobleIPT2Domain.Models
{
    public class Sensors
    {
        public int SensorsId { get; set; }
        public string SensorName { get; set; } = string.Empty;
        public string SensorType { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string SensorStatus { get; set; } = string.Empty;
    }
}
