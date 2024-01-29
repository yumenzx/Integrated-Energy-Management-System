namespace MonitoringAndCommunicationMicroservice.DataTransferObject
{
	public class DeviceChangeTopicDeserialized
	{
		public string ChangeType { get; set; } = string.Empty;
		public int DeviceId { get; set; }
	}
}
