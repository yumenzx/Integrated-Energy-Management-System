using DeviceManagementMicroservice.DataTransferObject;
using DeviceManagementMicroservice.Model;
using Microsoft.EntityFrameworkCore;

namespace DeviceManagementMicroservice.DataAccess
{
	public class DeviceDbContext : DbContext
	{
		public DbSet<Device> Devices { get; set; }

		public DeviceDbContext(DbContextOptions<DeviceDbContext> options) : base(options) { }


		public IEnumerable<Device> GetDevices(int userId)
		{
			var devices = Devices.Where(d => d.OwnerId == userId).ToList();
			return devices;
		}

		public bool InsertDevice(Device device) 
		{
			Devices.Add(device);
			SaveChanges();
			return true;
		}
		
		public bool UpdateDevice(DeviceUpdateDatas newDatas)
		{
			var d = Devices.FirstOrDefault(d => d.Id == newDatas.DeviceId);

			if (d == null)
				return false;

			d.Description = newDatas.NewDescription;
			d.Address = newDatas.NewAddress;
			d.MaxHourlyConsumption = newDatas.NewConsumption;
			SaveChanges();

			return true;
		}

		public bool DeleteDevice(int id) 
		{
			var device = Devices.FirstOrDefault(d => d.Id == id);

			if (device == null)
				return false;

			Devices.Remove(device);
			SaveChanges();

			return true;
		}

		public bool MapDevice(int deviceId,int ownerId)
		{
			var device = Devices.FirstOrDefault(d => d.Id == deviceId);

			if (device == null)
				return false;

			device.OwnerId = ownerId;
			SaveChanges();

			return true;
		}

		public bool UnMapDevice(int deviceId)
		{
			var device = Devices.FirstOrDefault(d => d.Id == deviceId);

			if (device == null)
				return false;

			device.OwnerId = null;
			SaveChanges();

			return true;
		}

		public bool UnMapUserDevices(int ownerId)
		{
			var devices = Devices.Where(d => d.OwnerId == ownerId).ToList();
			
			if (devices == null)
				return false;

			if (devices.Count == 0)
				return true;

			foreach (var device in devices)
				device.OwnerId = null;

			SaveChanges();

			return true;
		}

		public int GetDeviceOwner(int deviceId)
		{
			var device = Devices.FirstOrDefault(d => d.Id == deviceId);

			if(device == null)
			{
				return -1;
			}

			int? ownerId = device.OwnerId;
			if(ownerId == null)
			{
				return -1;
			}
			return ownerId.Value;
		}

		public IEnumerable<int>? GetDevicesForUser(int userId)
		{
			var devices = Devices
				.Where(d => d.OwnerId == userId)
				.Select(d => d.Id)
				.ToArray();

			return devices;
		}
	}
}
