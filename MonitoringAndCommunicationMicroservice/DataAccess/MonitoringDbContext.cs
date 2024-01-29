using Microsoft.EntityFrameworkCore;
using MonitoringAndCommunicationMicroservice.Model;

namespace MonitoringAndCommunicationMicroservice.DataAccess
{
	public class MonitoringDbContext : DbContext
	{
		public DbSet<DeviceMeasurement> DeviceMeasurements { get; set; }

		public MonitoringDbContext(DbContextOptions<MonitoringDbContext> options) : base(options) { }

		public bool InsertMeasurement(DeviceMeasurement measurement)
		{
			DeviceMeasurements.Add(measurement);
			SaveChanges();
			return true;
		}

		public bool DeleteMeasurement(int deviceId)
		{
			var measurements = DeviceMeasurements.Where(m => m.Device_Id == deviceId).ToList();

			if(measurements == null)
				return false;

			if (measurements.Count == 0)
				return true;

			foreach (var measurement in measurements)
				DeviceMeasurements.Remove(measurement);

			SaveChanges();

			return true;
		}

		public DeviceMeasurement? GetLatestMeasurement(int deviceId)
		{
			var m = DeviceMeasurements
				.Where(m => m.Device_Id == deviceId)
				.OrderByDescending(m => m.Timestampt)
				.FirstOrDefault();

			return m;
		}

		public List<DeviceMeasurement> GetDeviceMeasurements(int deviceId, DateTime date)
		{
			var nextDay = date.AddDays(1);
			var measurements = DeviceMeasurements
				.Where(m => m.Device_Id == deviceId && m.Timestampt >= date && m.Timestampt < nextDay)
				.OrderBy(m => m.Timestampt)
				.ToList();
			if(measurements == null)
				return new List<DeviceMeasurement>();
			return measurements;
		}
	}
}
