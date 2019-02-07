using System;
using System.Threading;
using duinocom;

namespace ArduinoSerialControllerClient
{
    public class ArduinoSerialDevice
    {
        public SerialClient Client;
        
        public bool IsConnected = false;
        
        public ArduinoSerialDevice(string portName, int baudRate)
        {
            Client = new SerialClient(portName, baudRate);
        }
        
        public void Connect()
        {
            Client.Open();
            
            IsConnected = true;
        }
        
        public void Disconnect()
        {
            Client.Close();
            
            IsConnected = false;
        }
        
        public bool DigitalRead(int pinNumber)
        {
            var cmd = String.Format("D{0}:R", pinNumber);
            
            Client.WriteLine(cmd);

            Thread.Sleep(1000);

            var output = Client.Read();

            var digitalValue = Convert.ToInt32(output.Trim()) == 1;

            return digitalValue;
        }
        
        public string AnalogRead(int pinNumber)
        {
            var cmd = String.Format("A{0}:R", pinNumber);
            
            Client.WriteLine(cmd);
			
            //Thread.Sleep(1000);

            var output = Client.ReadLine();

			var analogValue = output.Trim();

			if (analogValue.Contains("\r\n"))
			{
				string correctValue = RightString(analogValue.ToString(), "\r\n");

				correctValue = LeftString(correctValue.ToString(), "\r\n");

				correctValue = LeftRemove(correctValue.ToString(), "\n");

				return correctValue;
			}
			else
			{
				return analogValue;
			}
			
        }

		public static string RightString(string fullString, string splitString)
		{
			fullString = string.IsNullOrEmpty(fullString) ? string.Empty : fullString;
			splitString = string.IsNullOrEmpty(splitString) ? string.Empty : splitString;
			string result = fullString;

			if (fullString != string.Empty && splitString != string.Empty)
			{
				int startPos = fullString.IndexOf(splitString);
				result = (startPos == -1 || startPos == 0) ? fullString : fullString.Substring(startPos + 1);
			}
			return result;
		}

		public static string LeftString(string fullString, string splitString)
		{
			fullString = string.IsNullOrEmpty(fullString) ? string.Empty : fullString;
			splitString = string.IsNullOrEmpty(splitString) ? string.Empty : splitString;
			string result = fullString;

			if (fullString != string.Empty && splitString != string.Empty)
			{
				int startPos = fullString.IndexOf(splitString);
				result = startPos == -1 ? fullString : fullString.Substring(0, startPos);
			}
			return result;
		}


		public string LeftRemove(string fullString, string splitString)
		{
			fullString = string.IsNullOrEmpty(fullString) ? string.Empty : fullString;
			splitString = string.IsNullOrEmpty(splitString) ? string.Empty : splitString;
			string result = fullString;

			if (fullString != string.Empty && splitString != string.Empty)
			{
				int startPos = fullString.IndexOf(splitString) + splitString.Length;
				result = startPos == -1 ? fullString : fullString.Substring(startPos);
			}
			return result;
		}

		public static string RightRemove(string fullString, string splitString)
		{
			fullString = string.IsNullOrEmpty(fullString) ? string.Empty : fullString;
			splitString = string.IsNullOrEmpty(splitString) ? string.Empty : splitString;
			string result = fullString;

			if (fullString != string.Empty && splitString != string.Empty)
			{
				int startPos = fullString.LastIndexOf(splitString);
				result = startPos == -1 ? fullString : fullString.Substring(0, startPos);
			}
			return result;
		}

		public void DigitalWrite(int pinNumber, bool value)
        {
            var cmd = String.Format("A{0}:{1}", pinNumber, value);
            
            Client.WriteLine(cmd);
        }
        
        public void AnalogWrite(int pinNumber, int value)
        {
            var cmd = String.Format("A{0}:{1}", pinNumber, value);
            
            Client.WriteLine(cmd);
        }
        
        public void AnalogWritePercentage(int pinNumber, int value)
        {
            CheckConnected();
            
            Console.WriteLine("Analog writing percentage: " + value);
            
            var pwmValue = ArduinoConvert.PercentageToPWM(value);
            
            Console.WriteLine("Converted PWM value: " + pwmValue);
            
            var cmd = String.Format("A{0}:{1}", pinNumber, pwmValue);
            
            Console.WriteLine("Sending command: " + cmd);
            
            Client.WriteLine(cmd);
        }
        
        public void CheckConnected()
        {
            if (!IsConnected)
                throw new Exception("Not connected. Call Connect() function before trying to communicate.");
        }
    }
}
