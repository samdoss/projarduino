using Samdoss.CommonLayer;
using Samdoss.DataLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArduinoDataReader
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		Thread demoThread; // = new Thread(new ThreadStart(CallToChildThread));

		private void Form1_Load(object sender, EventArgs e)
		{

			DataStore ds = new DataStore();
			ds.ScreenMode = ScreenMode.Add;
			while (true)
			{
				string data = "31";
				if (!string.IsNullOrEmpty(data))
				{
					ds.DataValue = data;
					ds.Commit();
					Console.WriteLine(data);
				}
			}
		}

		public void CallToChildThread()
		{
			while (true)
			{
				if (arduinoChart.IsHandleCreated)
				{

					this.Invoke((MethodInvoker)delegate { arduinoCharts(); });
				}
			}						
		}

		private void arduinoCharts()
		{
			
			DataStore ds = new DataStore();
			ds.GetDataStored();
			arduinoChart.Series["Series1"].Points.AddY(Convert.ToInt32(ds.DataValue));
		}


		private void btnClose_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void btnStop_Click(object sender, EventArgs e)
		{
			this.demoThread.Abort();
			btnStart.Enabled = true;
		}

		private void btnStart_Click(object sender, EventArgs e)
		{
			arduinoChart.Series["Series1"].Points.Clear();
			this.demoThread = new Thread(new ThreadStart(this.CallToChildThread));
			this.demoThread.IsBackground = true;
			this.demoThread.Start();
			btnStart.Enabled = false;
		}
	}
}
