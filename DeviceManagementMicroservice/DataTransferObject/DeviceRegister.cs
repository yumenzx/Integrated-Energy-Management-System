namespace DeviceManagementMicroservice.DataTransferObject
{
	public class DeviceRegister
	{
		public string Description { get; set; } = string.Empty;
		public string Address { get; set; } = string.Empty;
		public int MaxHourlyConsumption { get; set; }
	}
}
