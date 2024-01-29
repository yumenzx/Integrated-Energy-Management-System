using DeviceManagementMicroservice.DataAccess;
using DeviceManagementMicroservice.DataTransferObject;
using DeviceManagementMicroservice.Model;

namespace DeviceManagementMicroservice.Repositories
{
	public class DeviceRepository
	{
		private readonly DeviceDbContext _context;

		public DeviceRepository(DeviceDbContext context)
		{
			_context = context;
		}


		public IEnumerable<Device> GetAllDevices() 
		{
			return _context.Devices;
		}


		public IEnumerable<Device> GetDevices(int userId)
		{
			return _context.GetDevices(userId);
		}

		public bool InsertDevice(Device device)
		{
			return _context.InsertDevice(device);
		}

		public bool UpdateDevice(DeviceUpdateDatas newDatas)
		{
			return _context.UpdateDevice(newDatas);
		}

		public bool DeleteDevice(int id)
		{
			return _context.DeleteDevice(id);
		}

		public bool MapDevice(int deviceId, int ownerId)
		{
			return _context.MapDevice(deviceId, ownerId);
		}

		public bool UnMapDevice(int deviceId)
		{
			return _context.UnMapDevice(deviceId);
		}

		public bool UnMapUserDevices(int userId)
		{
			return _context.UnMapUserDevices(userId);
		}

		public int GetDeviceOwner(int deviceId)
		{
			return _context.GetDeviceOwner(deviceId);
		}

		public IEnumerable<int>? GetDevicesForUser(int userId)
		{
			return _context.GetDevicesForUser(userId);
		}
	}
}
