namespace DeviceManagementMicroservice.DataTransferObject
{
	public class DeviceUpdateDatas
	{
		public int DeviceId { get; set; }
		public string NewDescription { get; set; } = string.Empty;
		public string NewAddress { get; set; } = string.Empty;
		public int NewConsumption { get; set; }
	}
}
