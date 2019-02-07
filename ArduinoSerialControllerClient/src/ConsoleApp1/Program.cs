using ArduinoSerialControllerClient;
using Samdoss.CommonLayer;
using Samdoss.DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ArduinoDataReaderConsole
{
	class Program
	{
		static void Main(string[] args)
		{
			string portName = string.Empty;
			foreach (string s in System.IO.Ports.SerialPort.GetPortNames())
			{
				if (s.ToLower().Contains("com"))
				{ 
					portName = s;
				}
			}

			if(string.IsNullOrEmpty(portName))
			{
				Console.WriteLine("Port Not exists");
				return;
			}


			ArduinoSerialDevice device = new ArduinoSerialDevice(portName, 9600);
			device.Connect();
			DataStore ds = new DataStore();
			ds.ScreenMode = ScreenMode.Add;
			while (true)
			{
				string data = device.AnalogRead(3).ToString();
				if (!string.IsNullOrEmpty(data))
				{
					ds.DataValue = data;
					ds.Commit();
					Console.WriteLine(data);
				}
			}
		}
	}
}
