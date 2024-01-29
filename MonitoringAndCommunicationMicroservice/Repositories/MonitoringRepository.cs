using MonitoringAndCommunicationMicroservice.DataAccess;
using MonitoringAndCommunicationMicroservice.Model;

namespace MonitoringAndCommunicationMicroservice.Repositories
{
	public class MonitoringRepository
	{
		private readonly MonitoringDbContext _context;

		public MonitoringRepository(MonitoringDbContext context) { 
			_context = context;
		}


		public bool InsertMeasurement(DeviceMeasurement measurement)
		{
			return _context.InsertMeasurement(measurement);
		}

		public bool DeleteMeasurements(int deviceId)
		{
			return _context.DeleteMeasurement(deviceId);
		}

		public DeviceMeasurement? GetLeatestMeasurement(int deviceId)
		{
			return _context.GetLatestMeasurement(deviceId);
		}
	}
}
