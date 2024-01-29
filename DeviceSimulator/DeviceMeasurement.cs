using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceSimulator
{
	public class DeviceMeasurement
	{
        public DateTime Timestampt { get; set; }
		public int Device_Id { get; set; }
		public float Measurement_Value { get; set; }

		public DeviceMeasurement(DateTime timestampt, int device_id, float measurement_value)
		{
			Timestampt = timestampt;
			Device_Id = device_id;
			Measurement_Value = measurement_value;
		}
    }
}
