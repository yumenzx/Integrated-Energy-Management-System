using System.Windows.Forms;
using static System.Windows.Forms.LinkLabel;

namespace DeviceSimulator
{
	public partial class DeviceSimulationForm : Form
	{
		private readonly RabbitMQProducer _producer;

		private System.Threading.Timer timer;
		//private string[] measurements;
		//private int currentIndex;
		private StreamReader streamReader;
		private int deviceId;
		

		public DeviceSimulationForm(RabbitMQProducer producer)
		{
			_producer = producer;
			InitializeComponent();
			//measurements = new string[0];
		}

		private void ShowWarning()
		{
			string title = "Warning";
			string message = "Invalid format";
			MessageBoxButtons buttons = MessageBoxButtons.OK;
			DialogResult result = MessageBox.Show(message, title, buttons, MessageBoxIcon.Warning);
		}

		private void button_SendMeasurement_Click(object sender, EventArgs e)
		{
			var deviceID = default(int);
			var measurementValue = default(float);
			try
			{
				deviceID = int.Parse(textBox_DeviceIDManual.Text);
				measurementValue = float.Parse(textBox_Consumption.Text);
			}
			catch (Exception)
			{
				ShowWarning();
				return;
			}

			var timestamp = DateTime.Now.AddHours(2);
			
			var deviceMeasurement = new DeviceMeasurement(timestamp, deviceID, measurementValue);

			_producer.SendMessage(deviceMeasurement);
		}

		private void button_LoadMeasurementsFile_Click(object sender, EventArgs e)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog
			{
				InitialDirectory = @"D:\",
				Title = "Browse Text Files",

				CheckFileExists = true,
				CheckPathExists = true,

				DefaultExt = "txt",
				Filter = "All files (*.*)|*.*",
				FilterIndex = 2,
				RestoreDirectory = true,

				ReadOnlyChecked = true,
				ShowReadOnly = true
			};

			if (openFileDialog.ShowDialog() != DialogResult.OK)
				return;

			var filePath = openFileDialog.FileName;

			if (Path.GetExtension(filePath).Equals(".csv", StringComparison.OrdinalIgnoreCase) == false)
			{
				MessageBox.Show("File does not have .csv extension", "CSV file extension", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			/*try
			{
				measurements = File.ReadAllLines(filePath);
			}
			catch(Exception ex) {
				MessageBox.Show("Exception occured: " + ex.Message, "Read Exception Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}*/
			try
			{
				streamReader = new StreamReader(filePath);
			}
			catch (Exception ex)
			{
				MessageBox.Show("Exception occured: " + ex.Message, "Read Exception Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			checkBox_FileLoaded.Checked = true;
			button_StartSimulation.Enabled = true;
		}

		private void button_StartSimulation_Click(object sender, EventArgs e)
		{
			var simulationDelay = default(float);
			try
			{
				deviceId = int.Parse(textBox_DeviceIDAutomatic.Text);
				simulationDelay = float.Parse(textBox_SimulationDelay.Text);
			}
			catch (Exception)
			{
				ShowWarning();
				return;
			}

			button_StartSimulation.Enabled = false;
			button_StopSimulation.Enabled = true;

			float miliseconds = simulationDelay * 60 * 1000;
			int delay = (int)Math.Round(miliseconds);

			//currentIndex = 0;
			timer = new System.Threading.Timer(ProcessMeasurements, null, 0, delay);
		}

		private void button_StopSimulation_Click(object sender, EventArgs e)
		{
			button_StartSimulation.Enabled = true;
			button_StopSimulation.Enabled = false;

			//streamReader.Close();

			timer.Dispose();
		}

		private void ProcessMeasurements(object? state)
		{
			//if(currentIndex >= measurements.Length)
			if(streamReader.EndOfStream)
			{
				timer.Dispose();
				button_StartSimulation.Enabled = true;
				button_StopSimulation.Enabled = false;
				MessageBox.Show("Reached the end of the simulation file. The simulation ended", "Simulation Ended", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			
			var measurement = default(float);
			try
			{
				//measurement = float.Parse(measurements[currentIndex++]);
				var line = streamReader.ReadLine();
				if(line == null)
				{
					MessageBox.Show("The readed line from the stream reader is null", "Read error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}
				measurement = float.Parse(line);
			}
			catch(Exception ex) {
				MessageBox.Show("Exception occured: " + ex.Message, "Parse Exception Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}

			var timestamp = DateTime.Now.AddHours(2);
			var deviceMeasurement = new DeviceMeasurement(timestamp, deviceId, measurement);

			_producer.SendMessage(deviceMeasurement);
			//MessageBox.Show(measurements[currentIndex++]);
		}
	}
}