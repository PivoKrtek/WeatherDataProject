using System;
using System.ComponentModel.DataAnnotations;

namespace WeatherDataLibrary.Models
{
    public class WeatherData
    {
        [Key] public int Id { get; set; }
        [Required] public int Thermometer { get; set; }
        [Required] public DateTime DateAndTime { get; set; }
        [Required] public double Temperature { get; set; }
        public int? Humidity { get; set; }
        public WeatherData(int thermometer, DateTime dateAndTime, double temperature, int? humidity)
        {
            Thermometer = thermometer;
            DateAndTime = dateAndTime;
            Temperature = temperature;
            Humidity = humidity;
        }
    }
}
