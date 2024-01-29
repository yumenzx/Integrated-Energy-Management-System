namespace DeviceManagementMicroservice.DataTransferObject
{
	public class DeviceChangeTopic
	{
		public string ChangeType { get; set; } = string.Empty;
		public int DeviceId { get; set; }

		public DeviceChangeTopic(string changeType, int deviceId)
		{
			ChangeType = changeType;
			DeviceId = deviceId;
		}
	}
}
