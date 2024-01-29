using MonitoringAndCommunicationMicroservice.DataAccess;
using MonitoringAndCommunicationMicroservice.Model;

namespace MonitoringAndCommunicationMicroservice.Repositories
{
	public class ConsumptionRepository
	{
		private readonly MonitoringDbContext _context;

		public ConsumptionRepository(MonitoringDbContext context)
		{
			_context = context;
		}


		public List<DeviceMeasurement> GetDeviceMeasurements(int deviceId, DateTime dateTime)
		{
			return _context.GetDeviceMeasurements(deviceId, dateTime);
		} 
	}
}
