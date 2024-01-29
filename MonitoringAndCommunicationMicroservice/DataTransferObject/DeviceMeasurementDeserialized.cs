namespace MonitoringAndCommunicationMicroservice.DataTransferObject
{
	public class DeviceMeasurementDeserialized
	{
		public DateTime Timestampt { get; set; }
		public int Device_Id { get; set; }
		public float Measurement_Value { get; set; }
	}
}
