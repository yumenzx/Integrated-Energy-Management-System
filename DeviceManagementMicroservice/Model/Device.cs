using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DeviceManagementMicroservice.Model
{
	[Table("devicesData")]
	public class Device
	{
		[Key]
		[Column("id")]
		public int Id { get; set; }

		[Required]
		[Column("description")]
		public string Description { get; set; } = string.Empty;

		[Required]
		[Column("address")]
		public string Address { get; set; } = string.Empty;

		[Required]
		[Column("maxHourlyConsumption")]
		public int MaxHourlyConsumption { get; set; }

		[Column("ownerId")]
		public int? OwnerId { get; set; }

	}
}
