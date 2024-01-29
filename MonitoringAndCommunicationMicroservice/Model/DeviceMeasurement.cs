using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MonitoringAndCommunicationMicroservice.Model
{
	[Table("measurementData")]
	public class DeviceMeasurement
	{
		[Key]
		[Column("id")]
		public int Id { get; set; }

		[Required]
		[Column("timestampV")]
		public DateTime Timestampt { get; set; }

		[Required]
		[Column("device_id")]
		public int Device_Id { get; set; }

		[Required]
		[Column("measurement_value")]
		public float Measurement_Value { get; set; }

	}
}
