using Microsoft.AspNetCore.SignalR;
using SignalRDemoApp.Hubs;
using System.IO;
using Samdoss.DataLayer;
using System;

namespace SignalRDemoApp
{
    public interface IMyDataReceiver { }

    public class MyDataReceiver : IMyDataReceiver
	{
        private readonly IHubContext<ReceiveDataHub> _hubContext;
		int valueincrement = 100;

        public MyDataReceiver(IHubContext<ReceiveDataHub> hubContext)
        {
            _hubContext = hubContext;
			//var watcher = new FileSystemWatcher();            

			//watcher.Created += new FileSystemEventHandler(OnChanged);
			//watcher.Deleted += new FileSystemEventHandler(OnChanged);            

			//// tell the watcher where to look
			//watcher.Path = @"D:\";

			//// You must add this line - this allows events to fire.
			//watcher.EnableRaisingEvents = true;
			while (true)
			{
				arduinoCharts();
			}
		}

		private void arduinoCharts()
		{
			//DataStore ds = new DataStore();
			//ds.GetDataStored();
			//arduinoChart.Series["Series1"].Points.AddY(Convert.ToInt32(ds.DataValue));
			//if (!string.IsNullOrEmpty(ds.DataValue))
			//{
			//	_hubContext.Clients.All.SendAsync("onReceiveData", Convert.ToInt32(ds.DataValue));
			
			_hubContext.Clients.All.SendAsync("onReceiveData", Convert.ToInt32(valueincrement++).ToString());
			var file = new ChatDetails() { xAxis = valueincrement, yAxis = 1000 };
			_hubContext.Clients.All.SendAsync("onFileChange", file);
			//}
		}

		private void OnChanged(object sender, FileSystemEventArgs e)
        {
            var file = new FileDetails() { Name = e.Name, ChangeType = e.ChangeType.ToString() };
            _hubContext.Clients.All.SendAsync("onFileChange", file);
        }
    }
}
