namespace ArduinoDataReader
{
	partial class Form1
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
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
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
			System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
			System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
			this.arduinoChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
			this.btnStart = new System.Windows.Forms.Button();
			this.btnStop = new System.Windows.Forms.Button();
			this.btnClose = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.arduinoChart)).BeginInit();
			this.SuspendLayout();
			// 
			// arduinoChart
			// 
			this.arduinoChart.BackColor = System.Drawing.Color.Silver;
			this.arduinoChart.BorderlineColor = System.Drawing.Color.Gray;
			chartArea1.Name = "ChartArea1";
			this.arduinoChart.ChartAreas.Add(chartArea1);
			legend1.Name = "Legend1";
			this.arduinoChart.Legends.Add(legend1);
			this.arduinoChart.Location = new System.Drawing.Point(12, 12);
			this.arduinoChart.Name = "arduinoChart";
			this.arduinoChart.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.SemiTransparent;
			series1.ChartArea = "ChartArea1";
			series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.StepLine;
			series1.Legend = "Legend1";
			series1.Name = "Series1";
			this.arduinoChart.Series.Add(series1);
			this.arduinoChart.Size = new System.Drawing.Size(708, 330);
			this.arduinoChart.TabIndex = 0;
			this.arduinoChart.Text = "chart1";
			// 
			// btnStart
			// 
			this.btnStart.Location = new System.Drawing.Point(413, 348);
			this.btnStart.Name = "btnStart";
			this.btnStart.Size = new System.Drawing.Size(86, 28);
			this.btnStart.TabIndex = 1;
			this.btnStart.Text = "Start";
			this.btnStart.UseVisualStyleBackColor = true;
			this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
			// 
			// btnStop
			// 
			this.btnStop.Location = new System.Drawing.Point(505, 348);
			this.btnStop.Name = "btnStop";
			this.btnStop.Size = new System.Drawing.Size(86, 28);
			this.btnStop.TabIndex = 2;
			this.btnStop.Text = "Stop";
			this.btnStop.UseVisualStyleBackColor = true;
			this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
			// 
			// btnClose
			// 
			this.btnClose.Location = new System.Drawing.Point(597, 348);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(86, 28);
			this.btnClose.TabIndex = 3;
			this.btnClose.Text = "Close";
			this.btnClose.UseVisualStyleBackColor = true;
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(732, 388);
			this.Controls.Add(this.btnClose);
			this.Controls.Add(this.btnStop);
			this.Controls.Add(this.btnStart);
			this.Controls.Add(this.arduinoChart);
			this.Name = "Form1";
			this.Text = "Form1";
			this.Load += new System.EventHandler(this.Form1_Load);
			((System.ComponentModel.ISupportInitialize)(this.arduinoChart)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.DataVisualization.Charting.Chart arduinoChart;
		private System.Windows.Forms.Button btnStart;
		private System.Windows.Forms.Button btnStop;
		private System.Windows.Forms.Button btnClose;
	}
}

