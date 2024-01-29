namespace MonitoringAndCommunicationMicroservice.DataTransferObject
{
	public class ComputedDeviceConsumptionByHour
	{
		public int DeviceId {  get; set; }
		public double[] Consumptions {  get; set; }

		public ComputedDeviceConsumptionByHour(int deviceId) 
		{
			DeviceId = deviceId;
			Consumptions = new double[24];

           for(int i = 0; i < 24; i++)
			{
				Consumptions[i] = 0;
			}
        }
	}
}
