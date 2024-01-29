namespace DeviceSimulator
{
	partial class DeviceSimulationForm
	{
		/// <summary>
		///  Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		///  Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		///  Required method for Designer support - do not modify
		///  the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			button_SendMeasurement = new Button();
			textBox_Consumption = new TextBox();
			textBox_DeviceIDManual = new TextBox();
			label_DeviceIDManual = new Label();
			label_ConsumptionValue = new Label();
			button_LoadMeasurementsFile = new Button();
			button_StartSimulation = new Button();
			button_StopSimulation = new Button();
			label_SimulationDelay = new Label();
			textBox_SimulationDelay = new TextBox();
			checkBox_FileLoaded = new CheckBox();
			label_Manual = new Label();
			label_Automatic = new Label();
			label_DeviceIDAutomatic = new Label();
			textBox_DeviceIDAutomatic = new TextBox();
			SuspendLayout();
			// 
			// button_SendMeasurement
			// 
			button_SendMeasurement.Location = new Point(52, 175);
			button_SendMeasurement.Name = "button_SendMeasurement";
			button_SendMeasurement.Size = new Size(189, 49);
			button_SendMeasurement.TabIndex = 0;
			button_SendMeasurement.Text = "Send measurement";
			button_SendMeasurement.UseVisualStyleBackColor = true;
			button_SendMeasurement.Click += button_SendMeasurement_Click;
			// 
			// textBox_Consumption
			// 
			textBox_Consumption.Location = new Point(135, 119);
			textBox_Consumption.Name = "textBox_Consumption";
			textBox_Consumption.Size = new Size(125, 27);
			textBox_Consumption.TabIndex = 1;
			// 
			// textBox_DeviceIDManual
			// 
			textBox_DeviceIDManual.Location = new Point(135, 86);
			textBox_DeviceIDManual.Name = "textBox_DeviceIDManual";
			textBox_DeviceIDManual.Size = new Size(125, 27);
			textBox_DeviceIDManual.TabIndex = 2;
			// 
			// label_DeviceIDManual
			// 
			label_DeviceIDManual.AutoSize = true;
			label_DeviceIDManual.Location = new Point(32, 93);
			label_DeviceIDManual.Name = "label_DeviceIDManual";
			label_DeviceIDManual.Size = new Size(73, 20);
			label_DeviceIDManual.TabIndex = 3;
			label_DeviceIDManual.Text = "Device ID";
			// 
			// label_ConsumptionValue
			// 
			label_ConsumptionValue.AutoSize = true;
			label_ConsumptionValue.Location = new Point(32, 122);
			label_ConsumptionValue.Name = "label_ConsumptionValue";
			label_ConsumptionValue.Size = new Size(97, 20);
			label_ConsumptionValue.TabIndex = 4;
			label_ConsumptionValue.Text = "Consumption";
			// 
			// button_LoadMeasurementsFile
			// 
			button_LoadMeasurementsFile.Location = new Point(347, 60);
			button_LoadMeasurementsFile.Name = "button_LoadMeasurementsFile";
			button_LoadMeasurementsFile.Size = new Size(188, 49);
			button_LoadMeasurementsFile.TabIndex = 5;
			button_LoadMeasurementsFile.Text = "Load Measurements File";
			button_LoadMeasurementsFile.UseVisualStyleBackColor = true;
			button_LoadMeasurementsFile.Click += button_LoadMeasurementsFile_Click;
			// 
			// button_StartSimulation
			// 
			button_StartSimulation.Enabled = false;
			button_StartSimulation.Location = new Point(347, 256);
			button_StartSimulation.Name = "button_StartSimulation";
			button_StartSimulation.Size = new Size(150, 29);
			button_StartSimulation.TabIndex = 6;
			button_StartSimulation.Text = "Start Simulation";
			button_StartSimulation.UseVisualStyleBackColor = true;
			button_StartSimulation.Click += button_StartSimulation_Click;
			// 
			// button_StopSimulation
			// 
			button_StopSimulation.Enabled = false;
			button_StopSimulation.Location = new Point(503, 256);
			button_StopSimulation.Name = "button_StopSimulation";
			button_StopSimulation.Size = new Size(150, 29);
			button_StopSimulation.TabIndex = 7;
			button_StopSimulation.Text = "Stop Simulation";
			button_StopSimulation.UseVisualStyleBackColor = true;
			button_StopSimulation.Click += button_StopSimulation_Click;
			// 
			// label_SimulationDelay
			// 
			label_SimulationDelay.AutoSize = true;
			label_SimulationDelay.Location = new Point(347, 175);
			label_SimulationDelay.Name = "label_SimulationDelay";
			label_SimulationDelay.Size = new Size(226, 20);
			label_SimulationDelay.TabIndex = 9;
			label_SimulationDelay.Text = "Simulation Read Delay (minutes)";
			// 
			// textBox_SimulationDelay
			// 
			textBox_SimulationDelay.Location = new Point(426, 198);
			textBox_SimulationDelay.Name = "textBox_SimulationDelay";
			textBox_SimulationDelay.Size = new Size(125, 27);
			textBox_SimulationDelay.TabIndex = 8;
			// 
			// checkBox_FileLoaded
			// 
			checkBox_FileLoaded.AutoCheck = false;
			checkBox_FileLoaded.AutoSize = true;
			checkBox_FileLoaded.Location = new Point(541, 73);
			checkBox_FileLoaded.Name = "checkBox_FileLoaded";
			checkBox_FileLoaded.Size = new Size(108, 24);
			checkBox_FileLoaded.TabIndex = 10;
			checkBox_FileLoaded.Text = "File Loaded";
			checkBox_FileLoaded.UseVisualStyleBackColor = true;
			// 
			// label_Manual
			// 
			label_Manual.AutoSize = true;
			label_Manual.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
			label_Manual.Location = new Point(96, 21);
			label_Manual.Name = "label_Manual";
			label_Manual.Size = new Size(83, 28);
			label_Manual.TabIndex = 11;
			label_Manual.Text = "Manual";
			// 
			// label_Automatic
			// 
			label_Automatic.AutoSize = true;
			label_Automatic.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
			label_Automatic.Location = new Point(440, 21);
			label_Automatic.Name = "label_Automatic";
			label_Automatic.Size = new Size(111, 28);
			label_Automatic.TabIndex = 12;
			label_Automatic.Text = "Automatic";
			// 
			// label_DeviceIDAutomatic
			// 
			label_DeviceIDAutomatic.AutoSize = true;
			label_DeviceIDAutomatic.Location = new Point(347, 129);
			label_DeviceIDAutomatic.Name = "label_DeviceIDAutomatic";
			label_DeviceIDAutomatic.Size = new Size(73, 20);
			label_DeviceIDAutomatic.TabIndex = 14;
			label_DeviceIDAutomatic.Text = "Device ID";
			// 
			// textBox_DeviceIDAutomatic
			// 
			textBox_DeviceIDAutomatic.Location = new Point(426, 126);
			textBox_DeviceIDAutomatic.Name = "textBox_DeviceIDAutomatic";
			textBox_DeviceIDAutomatic.Size = new Size(125, 27);
			textBox_DeviceIDAutomatic.TabIndex = 13;
			// 
			// DeviceSimulationForm
			// 
			AutoScaleDimensions = new SizeF(8F, 20F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(672, 328);
			Controls.Add(label_DeviceIDAutomatic);
			Controls.Add(textBox_DeviceIDAutomatic);
			Controls.Add(label_Automatic);
			Controls.Add(label_Manual);
			Controls.Add(textBox_SimulationDelay);
			Controls.Add(checkBox_FileLoaded);
			Controls.Add(label_SimulationDelay);
			Controls.Add(button_StopSimulation);
			Controls.Add(button_StartSimulation);
			Controls.Add(button_LoadMeasurementsFile);
			Controls.Add(label_ConsumptionValue);
			Controls.Add(label_DeviceIDManual);
			Controls.Add(textBox_DeviceIDManual);
			Controls.Add(textBox_Consumption);
			Controls.Add(button_SendMeasurement);
			FormBorderStyle = FormBorderStyle.FixedSingle;
			MaximizeBox = false;
			Name = "DeviceSimulationForm";
			Text = "Device Simulator";
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private Button button_SendMeasurement;
		private TextBox textBox_Consumption;
		private TextBox textBox_DeviceIDManual;
		private Label label_DeviceIDManual;
		private Label label_ConsumptionValue;
		private Button button_LoadMeasurementsFile;
		private Button button_StartSimulation;
		private Button button_StopSimulation;
		private Label label_SimulationDelay;
		private TextBox textBox_SimulationDelay;
		private CheckBox checkBox_FileLoaded;
		private Label label_Manual;
		private Label label_Automatic;
		private Label label_DeviceIDAutomatic;
		private TextBox textBox_DeviceIDAutomatic;
	}
}