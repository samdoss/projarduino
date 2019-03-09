﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using SignalR.Hubs;
using SignalR.Hosting.AspNet;
using SignalR.Infrastructure;
using SignalR;
using Samdoss.DataLayer;

namespace ArduinoWebChart
{
    /// <summary>
    /// To get the latest point for chart and broadcast it
    /// </summary>
    public class ChartData
    {
        private readonly static Lazy<ChartData> _instance = new Lazy<ChartData>(() => new ChartData());
        private readonly ConcurrentQueue<int> _points = new ConcurrentQueue<int>();    
        private readonly int _updateInterval = 250; //ms        
        private Timer _timer;
        private readonly object _updatePointsLock = new object();
        private bool _updatingData = false;
        private readonly Random _updateOrNotRandom = new Random();
        private int _startPoint = 31, _minPoint = 0, _maxPoint = 1200;
		DataStore ds = new DataStore();

		private ChartData()
        {
            
        }

        public static ChartData Instance
        {
            get
            {
                return _instance.Value;
            }
        }


        /// <summary>
        /// To initialize timer and data
        /// </summary>
        /// <returns></returns>
        public IEnumerable<int> initData()
        {
            _points.Enqueue(_startPoint); 
            _timer = new Timer(TimerCallBack, null, _updateInterval, _updateInterval);
            return _points.ToArray();
        }

       /// <summary>
       /// Timer callback method
       /// </summary>
       /// <param name="state"></param>
        private void TimerCallBack(object state)
        {
            // This function must be re-entrant as it's running as a timer interval handler
            if (_updatingData)
            {
                return;
            }
            lock (_updatePointsLock)
            {
                if (!_updatingData)
                {
                    _updatingData = true;

                    // Randomly choose whether to udpate this point or not           
                    if (_updateOrNotRandom.Next(0, 3) == 1)
                    {
                        BroadcastChartData(UpdateData());
                    }                    
                    _updatingData = false;
                }
            }
        }

        /// <summary>
        /// To update data (Generate random point in our case)
        /// </summary>
        /// <returns></returns>
        private int UpdateData()
        {
            int point = _startPoint;
            if (_points.TryDequeue(out point))
            {
				// Update the point price by a random factor of the range percent
				//var random = new Random();               
				//var pos = random.NextDouble() > .51;
				//var change = //random.Next((int)point/2);
				//change = pos ? change : -change;
				//point += change;
				
				ds.GetDataStored();
				string dataValue = ds.DataValue.ToString();
				if (string.IsNullOrEmpty(dataValue))
					dataValue = "0";

				point = Convert.ToInt32(dataValue);

				if (point < _minPoint) {
                    point = _minPoint;
                }
                if (point > _maxPoint) {
                    point = _maxPoint; 
                }
                _points.Enqueue(point);
            }           
            return point;
        }
       

        private void BroadcastChartData(int point)
        {
            GetClients().updateData(point);
        }

        private static dynamic GetClients()
        {
            return GlobalHost.ConnectionManager.GetHubContext<ChartDataHub>().Clients;
        }
    }

   
}